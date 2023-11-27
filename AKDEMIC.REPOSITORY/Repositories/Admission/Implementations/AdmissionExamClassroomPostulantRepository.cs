using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.AdmissionExamClassroomPostulant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class AdmissionExamClassroomPostulantRepository : Repository<AdmissionExamClassroomPostulant>, IAdmissionExamClassroomPostulantRepository
    {
        public AdmissionExamClassroomPostulantRepository(AkdemicContext context) : base(context) { }

        public async Task<List<AdmissionExamClassroomPostulant>> GetClassroomPostulants(Guid id)
            => await _context.AdmissionExamClassroomPostulants
                .Include(x => x.Postulant.Career)
                .Where(x => x.AdmissionExamClassroomId == id)
                .ToListAsync();

        public async Task<List<AdmissionExamClassroomPostulant>> GetStudentClassroom(Guid id)
            => await _context.AdmissionExamClassroomPostulants
            .Where(x => x.AdmissionExamClassroom.AdmissionExamId == id)
            .Include(x => x.AdmissionExamClassroom.Classroom.Building.Campus)
            .Include(x => x.Postulant)
            .ToListAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            Expression<Func<Postulant, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Document;
                    break;
                case "2":
                    orderByPredicate = (x) => x.FullName;
                    break;
                default:
                    break;
            }
            var admissionExam = await _context.AdmissionExams
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            var studentClassroom = await _context.AdmissionExamClassroomPostulants
                .Where(x => x.AdmissionExamClassroom.AdmissionExamId == id)
                .Select(x => new
                {
                    x.PostulantId,
                    Classroom = $"{x.AdmissionExamClassroom.Classroom.Building.Name} - {x.AdmissionExamClassroom.Classroom.Description}",
                    Campus = x.AdmissionExamClassroom.Classroom.Building.Campus.Name
                })
                .ToListAsync();

            var query = _context.Postulants
                .Where(x => admissionExam.AdmissionExamApplicationTerms.Any(y => y.ApplicationTermId == x.ApplicationTermId))
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    document = x.Document,
                    postulant = x.FullName,
                    classroom = studentClassroom.Any(y => y.PostulantId == x.Id) ? studentClassroom.FirstOrDefault(y => y.PostulantId == x.Id).Classroom : "---",
                    campus = studentClassroom.Any(y => y.PostulantId == x.Id) ? studentClassroom.FirstOrDefault(y => y.PostulantId == x.Id).Campus : "---"
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<PostulantInformationTemplate> GetStudentClassroomInformation(Guid examId, string document)
        {
            var data = await _context.AdmissionExamClassroomPostulants
                .Where(x => x.AdmissionExamClassroom.AdmissionExamId == examId && x.Postulant.Document == document)
                .Select(x => new PostulantInformationTemplate
                {
                    Picture = x.Postulant.Picture,
                    Name = x.Postulant.FullName,
                    Document = x.Postulant.Document,
                    Classroom = x.AdmissionExamClassroom.Classroom.Code,
                    Floor = x.AdmissionExamClassroom.Classroom.Floor,
                    Building = x.AdmissionExamClassroom.Classroom.Building.Name,
                    Campus = x.AdmissionExamClassroom.Classroom.Building.Campus.Name,
                    CampusAddress = x.AdmissionExamClassroom.Classroom.Building.Campus.Address,
                    AdmissionType = x.Postulant.AdmissionType.Name,
                    ApplicationTerm = x.Postulant.ApplicationTerm.Name,
                    Faculty = x.Postulant.Career.Faculty.Name,
                    Career = x.Postulant.Career.Name,
                    ExamDate = x.AdmissionExamClassroom.AdmissionExam.DateEvaluation,
                    Seat = x.Seat
                }).FirstOrDefaultAsync();

            return data;                
        }
    }
}
