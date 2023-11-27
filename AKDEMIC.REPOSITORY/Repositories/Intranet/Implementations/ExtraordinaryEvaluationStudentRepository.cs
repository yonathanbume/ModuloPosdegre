using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
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
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class ExtraordinaryEvaluationStudentRepository : Repository<ExtraordinaryEvaluationStudent>, IExtraordinaryEvaluationStudentRepository
    {
        public ExtraordinaryEvaluationStudentRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ExtraordinaryEvaluationStudent>> GetByExtraordinaryEvaluationId(Guid extraordinaryEvaluationId)
            => await _context.ExtraordinaryEvaluationStudents.Where(x => x.ExtraordinaryEvaluationId == extraordinaryEvaluationId).ToListAsync();

        public async Task<IEnumerable<ExtraordinaryEvaluationStudent>> GetByExtraordinaryEvaluationIdWithData(Guid extraordinaryEvaluationId)
           => await _context.ExtraordinaryEvaluationStudents.Where(x => x.ExtraordinaryEvaluationId == extraordinaryEvaluationId).Include(x=>x.Student.User).ToListAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsDatatable(DataTablesStructs.SentParameters parameters, Guid extraordinaryEvaluationId, string searchValue)
        {
            Expression<Func<ExtraordinaryEvaluationStudent, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Student.User.FullName); break;
                case "1":
                    orderByPredicate = ((x) => x.Student.User.UserName); break;
                case "2":
                    orderByPredicate = ((x) => x.Student.Career.Name); break;
                default:
                    orderByPredicate = ((x) => x.Student.User.FullName); break;
            }

            var query = _context.ExtraordinaryEvaluationStudents.Where(x => x.ExtraordinaryEvaluationId == extraordinaryEvaluationId).AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Student.User.FullName.ToLower().Trim().Contains(searchValue.Trim().ToLower()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      x.Student.User.FullName,
                      x.Student.User.UserName,
                      career = x.Student.Career.Name,
                      x.Grade,
                      isPending = x.Status == ConstantHelpers.Intranet.ExtraordinaryEvaluationStudent.PENDING
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

        public async Task<object> GetStudentsDatatableClientSide(Guid extraordinaryEvaluationId)
        {
            var result = await _context.ExtraordinaryEvaluationStudents.Where(x => x.ExtraordinaryEvaluationId == extraordinaryEvaluationId)
                .Select(x => new
                {
                    x.Id,
                    x.Student.User.UserName,
                    x.Student.User.FullName,
                    x.Grade,
                    isPending = x.Status == ConstantHelpers.Intranet.ExtraordinaryEvaluationStudent.PENDING
                })
                .ToListAsync();

            return result;
        }

        //public async Task<ExtraordinaryEvaluationStudent> GetWithData(Guid id)
        //{
        //    var evaluation = await _context.ExtraordinaryEvaluationStudents
        //        .Include(x => x.Course)
        //        .Include(x => x.Student)
        //        .FirstOrDefaultAsync(x => x.Id == id);

        //    return evaluation;
        //}

        public async Task<object> GetEnrollmentDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId)
        {
            var query = _context.ExtraordinaryEvaluationStudents
                .Where(x => x.StudentId == studentId && x.ExtraordinaryEvaluation.TermId == termId)
                .AsNoTracking();

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    teacher = x.ExtraordinaryEvaluation.Teacher.User.FullName ?? "---",
                    code = x.ExtraordinaryEvaluation.Course.Code,
                    course = x.ExtraordinaryEvaluation.Course.Name
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        //public async Task<object> GetStudentsDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, string teacherId = null, Guid? courseId = null, string search = null)
        //{
        //    Expression<Func<ExtraordinaryEvaluationStudent, dynamic>> orderByPredicate = null;

        //    switch (sentParameters.OrderColumn)
        //    {
        //        case "0":
        //            orderByPredicate = (x) => x.Student.User.UserName;
        //            break;
        //        case "1":
        //            orderByPredicate = (x) => x.Student.User.FullName;
        //            break;
        //        default:
        //            break;
        //    }

        //    if (termId == null || termId == Guid.Empty)
        //        termId = (await _context.Terms
        //            .Where(x => x.Status != ConstantHelpers.TERM_STATES.INACTIVE)
        //            .OrderBy(x => x.Status).ThenByDescending(x => x.Year).ThenByDescending(x => x.Number).FirstOrDefaultAsync()).Id;

        //    var query = _context.ExtraordinaryEvaluationStudents
        //        .Where(x => x.TermId == termId)
        //        .AsNoTracking();

        //    if (!string.IsNullOrEmpty(teacherId) && teacherId == Guid.Empty.ToString())
        //        query = query.Where(x => x.TeacherId == teacherId);

        //    if (courseId != null && courseId == Guid.Empty)
        //        query = query.Where(x => x.CourseId == courseId);

        //    if (!string.IsNullOrEmpty(search))
        //        query = query.Where(x => x.Student.User.UserName.ToUpper().Contains(search.ToUpper()) || x.Student.User.FullName.ToUpper().Contains(search.ToUpper()));

        //    var recordsFiltered = query.Count();

        //    var data = await query
        //        .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
        //        .Skip(sentParameters.PagingFirstRecord)
        //        .Take(sentParameters.RecordsPerDraw)
        //        .Select(x => new
        //        {
        //            id = x.Id,
        //            teacher = x.Teacher.User.FullName ?? "---",
        //            code = x.Course.Code,
        //            course = x.Course.Name,
        //            username = x.Student.User.UserName,
        //            student = x.Student.User.FullName,
        //            status = x.Status,
        //            grade = x.Grade
        //        })
        //        .ToListAsync();

        //    var recordsTotal = data.Count;

        //    return new DataTablesStructs.ReturnedData<object>
        //    {
        //        Data = data,
        //        DrawCounter = sentParameters.DrawCounter,
        //        RecordsFiltered = recordsFiltered,
        //        RecordsTotal = recordsTotal
        //    };
        //}

        //public async Task<object> GetExtraordinaryEvaluationDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? careerId, string searchValue)
        //{
        //    Expression<Func<ExtraordinaryEvaluationStudent, dynamic>> orderByPredicate = null;

        //    switch (parameters.OrderColumn)
        //    {
        //        case "0":
        //            orderByPredicate = (x) => x.Student.User.UserName;
        //            break;
        //        case "1":
        //            orderByPredicate = (x) => x.Student.User.FullName;
        //            break;
        //        default:
        //            break;
        //    }

        //    if (!termId.HasValue || termId != Guid.Empty)
        //        termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();

        //    var query = _context.ExtraordinaryEvaluationStudents
        //        .Where(x => x.TermId == termId)
        //        .AsNoTracking();

        //    if (!careerId.HasValue || careerId != Guid.Empty)

        //        if (string.IsNullOrEmpty(searchValue))
        //            query = query.Where(x => x.Teacher.User.FullName.ToLower().Trim().Contains(searchValue.Trim().ToLower()) ||
        //            x.Course.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

        //    var recordsFiltered = query.Count();

        //    var data = await query
        //        .OrderByCondition(parameters.OrderDirection, orderByPredicate)
        //        .Skip(parameters.PagingFirstRecord)
        //        .Take(parameters.RecordsPerDraw)
        //        .Select(x => new
        //        {
        //            id = x.Id,
        //            teacher = x.Teacher.User.FullName ?? "-",
        //            code = x.Course.Code,
        //            course = x.Course.Name,
        //            username = x.Student.User.UserName,
        //            student = x.Student.User.FullName,
        //            status = x.Status,
        //            grade = x.Grade
        //        })
        //        .ToListAsync();

        //    var recordsTotal = data.Count;

        //    return new DataTablesStructs.ReturnedData<object>
        //    {
        //        Data = data,
        //        DrawCounter = parameters.DrawCounter,
        //        RecordsFiltered = recordsFiltered,
        //        RecordsTotal = recordsTotal
        //    };
        //}

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentCurrentEvaluationsClientSideDatatable(Guid studentId, string searchValue)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            if (term == null)
            {
                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = null,
                    DrawCounter = 0,
                    RecordsFiltered = 0,
                    RecordsTotal = 0
                };
            }
            
            var query = _context.ExtraordinaryEvaluationStudents
                .Where(x => x.StudentId == studentId && x.ExtraordinaryEvaluation.TermId == term.Id)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.ExtraordinaryEvaluation.Course.Code.ToUpper().Contains(searchValue.ToUpper())
                || x.ExtraordinaryEvaluation.Course.Name.ToUpper().Contains(searchValue.ToUpper()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    code = x.ExtraordinaryEvaluation.Course.Code,
                    course = x.ExtraordinaryEvaluation.Course.Name,
                    credits = x.ExtraordinaryEvaluation.Course.Credits,
                    term = x.ExtraordinaryEvaluation.Term.Name,
                    isPending = x.Status == ConstantHelpers.Intranet.ExtraordinaryEvaluationStudent.PENDING
                })
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = 0,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<bool> IsPendingFromQualify(Guid extraordinaryEvaluationId)
        {
            var extraordinaryEvaluationStudents = await _context.ExtraordinaryEvaluationStudents
                .Where(x => x.ExtraordinaryEvaluationId == extraordinaryEvaluationId)
                .Select(x => new 
                {
                    x.Id,
                    x.ExtraordinaryEvaluationId,
                    x.Status
                })
                .ToListAsync();

            //Si no se encontro ninguno, entonces no se mostrará el reporte
            if (extraordinaryEvaluationStudents.Count == 0)
                return true;

            return extraordinaryEvaluationStudents
                .Any(x => x.ExtraordinaryEvaluationId == extraordinaryEvaluationId && x.Status == ConstantHelpers.Intranet.ExtraordinaryEvaluationStudent.PENDING);
        }

        public async Task<ExtraordinaryEvaluationReportTemplate> GetEvaluationReportInformation(Guid extraordinaryEvaluationId)
        {
            var confiHeader = await _context.Configurations
                .Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER)
                .FirstOrDefaultAsync();

            if (confiHeader is null)
            {
                confiHeader = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER,
                    Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.EVALUATION_REPORT_HEADER]
                };

                await _context.Configurations.AddAsync(confiHeader);
                await _context.SaveChangesAsync();
            }

            var model = new ExtraordinaryEvaluationReportTemplate();

            model.Header = confiHeader.Value;

            var extraordinaryEvaluation = await _context.ExtraordinaryEvaluations
                .Where(x => x.Id == extraordinaryEvaluationId)
                .Select(x => new
                {
                    CourseName = x.Course.Name,
                    CourseCode = x.Course.Code,
                    CourseCredits = x.Course.Credits,
                    CourseCareer = x.Course.Career.Name,
                    TermName = x.Term.Name,
                    Curriculum = x.Course.AcademicYearCourses.Where(y => y.CourseId == x.CourseId).Select(y => y.Curriculum.Code).FirstOrDefault(),
                    TeacherFullName = x.Teacher.User.FullName,
                    TeacherAcademicDepartment = x.Teacher.AcademicDepartmentId == null ? "" : x.Teacher.AcademicDepartment.Name
                })
                .FirstOrDefaultAsync();

            var extraordinaryEvaluationStudents = await _context.ExtraordinaryEvaluationStudents
                .Where(x => x.ExtraordinaryEvaluationId == extraordinaryEvaluationId)
                .Select(x => new ExtraordinaryEvaluationStudentsTemplate
                {
                    UserName = x.Student.User.UserName,
                    FullName = x.Student.User.FullName,
                    Grade = (int)Math.Round(x.Grade, 0, MidpointRounding.AwayFromZero),
                })
                .ToListAsync();

            model.ExtraordinaryEvaluationStudents = extraordinaryEvaluationStudents;
            model.Course = new ExtraordinaryEvaluationReportCourseTemplate
            {
                Name = extraordinaryEvaluation.CourseName,
                Code = extraordinaryEvaluation.CourseCode,
                Credits = extraordinaryEvaluation.CourseCredits.ToString(),
                CareerName = extraordinaryEvaluation.CourseCareer,
                Curriculum = extraordinaryEvaluation.Curriculum
            };
            model.Term = new EvaluationReportTermTemplate
            {
                Name = extraordinaryEvaluation.TermName
            };
            model.TeacherPresident = new ExtraordinaryEvaluationReportTeacherPresidentTemplate
            {
                FullName = extraordinaryEvaluation.TeacherFullName,
                AcademicDepartment = extraordinaryEvaluation.TeacherAcademicDepartment
            };

            return model;
        }
    }
}
