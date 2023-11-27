using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class ExtraordinaryEvaluationRepository : Repository<ExtraordinaryEvaluation>, IExtraordinaryEvaluationRepository
    {
        public ExtraordinaryEvaluationRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByCourseAndTermId(Guid courseId, Guid termId)
            => await _context.ExtraordinaryEvaluations.AnyAsync(x => x.CourseId == courseId && x.TermId == termId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetExtraordinaryEvaluationsDatatable(DataTablesStructs.SentParameters parameters, string searchValue, Guid? careerId, string teacherId, Guid? termId, ClaimsPrincipal user, bool? toEvalutionReport = null, byte? type = null)
        {
            Expression<Func<ExtraordinaryEvaluation, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Course.Code); break;
                case "1":
                    orderByPredicate = ((x) => x.Course.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.Teacher.User.FullName); break;
                default:
                    orderByPredicate = ((x) => x.Course.Code); break;
            }


            var query = _context.ExtraordinaryEvaluations.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var departments = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartmentId).ToArrayAsync();
                    var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();

                    query = query.Where(x => (x.Course.CareerId.HasValue && careers.Contains(x.Course.CareerId.Value)) || (x.Teacher.AcademicDepartmentId.HasValue && departments.Contains(x.Teacher.AcademicDepartmentId.Value)) || (x.Course.AcademicYearCourses.Any(y=>careers.Contains(y.Curriculum.CareerId))));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    query = query.Where(x => x.Course.Career.AcademicCoordinatorId == userId || x.Course.AcademicYearCourses.Any(y=>y.Curriculum.Career.AcademicCoordinatorId == userId));
                }
            }

            if (termId.HasValue && termId != Guid.Empty)
            {
                query = query.Where(x => x.TermId == termId);
            }
            else
            {
                var activeTermId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();
                query = query.Where(x => x.TermId == activeTermId);
            }

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Course.CareerId == careerId);

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.TeacherId == teacherId);

            if (type.HasValue && type != 0)
                query = query.Where(x => x.Type == type);

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => x.Course.Name.Contains(searchValue) || x.Teacher.User.FullName.Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      courseCode = x.Course.Code,
                      courseName = x.Course.Name,
                      teacher = x.Teacher.User.FullName,
                      career = x.Course.Career.Name,
                      type = ConstantHelpers.Intranet.ExtraordinaryEvaluation.TYPES[x.Type],
                      students = x.ExtraordinaryEvaluationStudents.Count(),
                      curriculum = string.Join(", ", x.Course.AcademicYearCourses.Select(x => x.Curriculum.Name).ToList()),
                      complete = !x.ExtraordinaryEvaluationStudents.Any(y => y.Status == ConstantHelpers.Intranet.ExtraordinaryEvaluationStudent.PENDING),
                      evaluationReport = toEvalutionReport.HasValue && toEvalutionReport.Value ? _context.EvaluationReports.Where(y => y.EntityId == x.Id && y.Type == ConstantHelpers.Intranet.EvaluationReportType.EXTRAORDINARY_EVALUATION).Select(z => new { status = ConstantHelpers.Intranet.EvaluationReport.NAMES[z.Status], z.PrintQuantity }).FirstOrDefault() : null
                  })
                  .ToListAsync();


            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExtraordinaryEvaluationsToTeacherDatatable(DataTablesStructs.SentParameters parameters, string searchValue, string teacherId)
        {
            Expression<Func<ExtraordinaryEvaluation, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Course.Code); break;
                case "1":
                    orderByPredicate = ((x) => x.Course.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.Teacher.User.FullName); break;
                default:
                    orderByPredicate = ((x) => x.Course.Code); break;
            }

            var activeTermId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();
            var query = _context.ExtraordinaryEvaluations.Where(x => x.TermId == activeTermId && x.TeacherId == teacherId).AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => x.Course.Name.Contains(searchValue) || x.Teacher.User.FullName.Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      courseCode = x.Course.Code,
                      courseName = x.Course.Name,
                      career = x.Course.Career.Name,
                      curriculum = string.Join(", ", x.Course.AcademicYearCourses.Select(x => x.Curriculum.Name).ToList()),
                      teacher = x.Teacher.User.FullName,
                  })
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

    }
}
