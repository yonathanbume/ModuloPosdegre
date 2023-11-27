using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class AdmissionExamClassroomRepository : Repository<AdmissionExamClassroom>, IAdmissionExamClassroomRepository
    {
        public AdmissionExamClassroomRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetClassrooms(Guid id)
        {
            var data = await _context.AdmissionExamClassrooms
                .Where(x => x.AdmissionExamId == id)
                .Select(x => new
                {
                    id = x.Id,
                    classroomId = x.ClassroomId,
                    classroom = x.Classroom.Description,
                    building = x.Classroom.Building.Name,
                    campus = x.Classroom.Building.Campus.Name,
                    capacity = x.Vacancies
                }).ToListAsync();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetClassroomsDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            Expression<Func<AdmissionExamClassroom, dynamic>> orderByPredicate = null;
            Expression<Func<AdmissionExamClassroom, dynamic>> thenByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Classroom.Description;
                    thenByPredicate = (x) => x.Classroom.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Classroom.Building.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Classroom.Building.Campus.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Vacancies;
                    break;
                default:
                    orderByPredicate = (x) => x.Classroom.Description;
                    break;
            }

            var query = _context.AdmissionExamClassrooms
                .Where(x => x.AdmissionExamId == id)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            if (sentParameters.RecordsPerDraw == 0)
            {
                query = query
                    .OrderByConditionThenBy(sentParameters.OrderDirection, orderByPredicate, thenByPredicate);
            }
            else
            {
                query = query
                .OrderByConditionThenBy(sentParameters.OrderDirection, orderByPredicate, thenByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);
            }

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    classroomId = x.ClassroomId,
                    classroom = $"{x.Classroom.Description} - {x.Classroom.Code}",
                    building = x.Classroom.Building.Name,
                    campus = x.Classroom.Building.Campus.Name,
                    capacity = x.Vacancies,
                    teachersNames = string.Join(", ", x.Teachers.Select(t => $"{t.User.RawFullName} ({t.User.UserName})").ToList()),
                    teachers = x.Teachers.Select(t => new
                    {
                        t.Id,
                        name = $"{t.User.UserName} - {t.User.FullName}"
                    }).ToList(),
                    careers = x.Careers.Select(t => new { t.CareerId, t.Career.Name }).ToList()
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
    
        public void Remove(AdmissionExamClassroom admissionExamClassroom)
            => _context.AdmissionExamClassrooms.Remove(admissionExamClassroom);

        public async Task<List<AdmissionExamClassroom>> GetClassroomsListById(Guid id)
            => await _context.AdmissionExamClassrooms
            .Where(x => x.AdmissionExamId == id)
            .Include(x => x.Careers)
            .ToListAsync();

        public async Task<bool> Any(Guid id, Guid classroomId)
        {
            return await _context.AdmissionExamClassrooms.AnyAsync(x => x.ClassroomId == classroomId && x.AdmissionExamId == id);
        }

        public async Task<AdmissionExamClassroom> GetWithClassroomBuildingAndCampus(Guid id)
        {
            return await _context.AdmissionExamClassrooms
                .Include(x => x.AdmissionExam)
                .Include(x => x.Classroom)
                    .ThenInclude(x => x.Building)
                        .ThenInclude(x => x.Campus)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<AdmissionExamClassroom>> GetAllByExam(Guid examId)
        {
            return await _context.AdmissionExamClassrooms
                .Where(x => x.AdmissionExamId == examId)
                .Include(x => x.Classroom)
                .ThenInclude(x => x.Building)
                .ThenInclude(x => x.Campus).ToListAsync();
        }

        public async Task<IEnumerable<AdmissionExamClassroom>> GetListByTeacherId(string userId, Guid? admissionTermId = null)
        {
            var query = _context.AdmissionExamClassrooms
                .Where(x => x.Postulants.Any())
                .Include(x => x.AdmissionExam)
                .Include(x => x.Classroom)
                .ThenInclude(x => x.Building)
                .ThenInclude(x => x.Campus)
                .AsNoTracking();

            if (admissionTermId.HasValue)
                query = query.Where(x => x.AdmissionExam.AdmissionExamApplicationTerms.Any(y => y.ApplicationTermId == admissionTermId));


            //#if !DEBUG
            //query = query.Where(x => x.Teachers.Any(y => y.TeacherId == userId));
            //#endif

            return await query.ToListAsync();
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid admissionExamnId)
        {
            Expression<Func<AdmissionExamClassroom, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Classroom.Building.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Classroom.Description;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Postulants.Where(y => y.Attended).Count();
                    break;
                case "3":
                    orderByPredicate = (x) => x.Postulants.Where(y => !y.Attended).Count();
                    break;
                default:
                    break;
            }

            var query = _context.AdmissionExamClassrooms
                .Where(x => x.AdmissionExamId == admissionExamnId)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    classroom = x.Classroom.Description,
                    building = x.Classroom.Building.Name,
                    campus = x.Classroom.Building.Campus.Name,
                    attended = x.Postulants.Where(y => y.Attended).Count(),
                    notAttended = x.Postulants.Where(y => !y.Attended).Count()
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetClassroomAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid classroomId)
        {
            Expression<Func<AdmissionExamClassroomPostulant, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Postulant.Document;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Postulant.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Attended;
                    break;
                default:
                    break;
            }

            var query = _context.AdmissionExamClassroomPostulants
                .Where(x => x.AdmissionExamClassroomId == classroomId)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    document = x.Postulant.Document,
                    name = x.Postulant.FullName,
                    status = x.Attended ? "Asistió" : "No asistió",
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
    }
}
