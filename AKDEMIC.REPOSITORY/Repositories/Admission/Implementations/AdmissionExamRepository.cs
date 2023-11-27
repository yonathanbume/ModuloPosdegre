using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
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
    public class AdmissionExamRepository : Repository<AdmissionExam>, IAdmissionExamRepository
    {
        public AdmissionExamRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetAmissionExam()
        {
            var result = await _context.AdmissionExams
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,               
                    applicationterm = x.AdmissionExamApplicationTerms.FirstOrDefault().ApplicationTerm.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<AdmissionExam> GetWithData(Guid id)
        {
            return await _context.AdmissionExams
                           .Include(x => x.AdmissionExamClassrooms)
                           .Include(x=>x.AdmissionExamApplicationTerms)
                           .Include(x => x.AdmissionExamChannels)
                           .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<AdmissionExam>> GetClassroomManagement(Guid id)
        {
            return await _context.AdmissionExams
                           .Where(x => x.Id == id)
                       .Include(x => x.AdmissionExamClassrooms).ThenInclude(x => x.Postulants).ToListAsync();
        }

        public async Task<List<AdmissionExam>> GetActiveApplicationTermsExams()
        {
            return await _context.AdmissionExams
                           .Where(x => x.AdmissionExamApplicationTerms.Any(y => y.ApplicationTerm.Status == ConstantHelpers.TERM_STATES.ACTIVE))
                           .OrderBy(x => x.Name)
                           .ToListAsync();
        }

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

        public async Task<object> GetAdmissionExams()
        {
            return await _context.AdmissionExams
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    applicationterm = "",//x.AdmissionExamApplicationTerms.FirstOrDefault().ApplicationTerm.Term.Name
                })
                .ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAdmissionExamDatatable(DataTablesStructs.SentParameters sentParameters, Guid? applicationTermId = null, string search = null)
        {
            Expression<Func<AdmissionExam, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
                default:
                    break;
            }

            var query = _context.AdmissionExams
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                var normalizedSearch = search.Normalize().ToUpper();
                query = query.Where(x => x.Name.ToUpper().Contains(normalizedSearch) || x.Code.ToUpper().Contains(normalizedSearch));
            }

            if (applicationTermId.HasValue && applicationTermId != Guid.Empty)
                query = query.Where(x => x.AdmissionExamApplicationTerms.Any(y => y.ApplicationTermId == applicationTermId));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    weight = x.Weight,
                    date = x.DateEvaluation.ToLocalDateFormat(),
                    isPrincipal = x.IsPrincipal
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

        public async Task<List<AdmissionExam>> GetByApplicationTermId(Guid applicationTermId)
        {
           return  await _context.AdmissionExams
                .Where(x => x.AdmissionExamApplicationTerms.Any(y => y.ApplicationTermId == applicationTermId))
                .ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantAssistanceReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid examId, byte status = 1)
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
                    orderByPredicate = (x) => x.Postulant.PaternalSurname;
                    break;
            }

            var query = _context.AdmissionExamClassroomPostulants
                .Where(x => x.AdmissionExamClassroom.AdmissionExamId == examId)
                .AsNoTracking();

            switch (status)
            {
                case 2: query = query.Where(x => x.Attended); break;
                case 3: query = query.Where(x => !x.Attended); break;
                default:
                    break;
            }

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
                    status = x.Attended ? "Asistió" : "No asistió"
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

        public async Task DeleteExamById(Guid id)
        {
            var exam = await _context.AdmissionExams.FirstOrDefaultAsync(x => x.Id == id);

            var channels = await _context.AdmissionExamChannels.Where(x => x.AdmissionExamId == id).ToListAsync();
            _context.AdmissionExamChannels.RemoveRange(channels);

            var applicationTerms = await _context.AdmissionExamApplicationTerms.Where(x => x.AdmissionExamId == id).ToListAsync();
            _context.AdmissionExamApplicationTerms.RemoveRange(applicationTerms);

            _context.AdmissionExams.Remove(exam);
            await _context.SaveChangesAsync();
        }
    }
}
