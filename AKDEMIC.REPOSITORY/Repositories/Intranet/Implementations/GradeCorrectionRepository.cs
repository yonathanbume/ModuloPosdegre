using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class GradeCorrectionRepository : Repository<GradeCorrection>, IGradeCorrectionRepository
    {
        public GradeCorrectionRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<GradeCorrection>> GetAll(string teacherId = null, Guid? termId = null)
        {
            var query = _context.GradeCorrections
                .Include(x => x.Grade.StudentSection.Student.User)
                .Include(x => x.Grade.StudentSection.Section.CourseTerm.Course)
                .Include(x => x.Grade.Evaluation)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(teacherId))
            {
                var configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.ENABLE_STUDENT_GRADE_CORRECTION_REQUEST).FirstOrDefaultAsync();

                if(configuration == null)
                {
                    configuration = new ENTITIES.Models.Configuration
                    {
                        Key = ConstantHelpers.Configuration.IntranetManagement.ENABLE_STUDENT_GRADE_CORRECTION_REQUEST,
                        Value = ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.IntranetManagement.ENABLE_STUDENT_GRADE_CORRECTION_REQUEST]
                    };
                }

                if (bool.Parse(configuration.Value))
                {
                    query = query.Where(x => x.TeacherId == teacherId || (x.Grade.StudentSection.Section.TeacherSections.Any(y => y.TeacherId == teacherId) && x.RequestedByStudent));
                }
                else
                {
                    query = query.Where(x => x.TeacherId == teacherId);
                }

            }

            if (termId.HasValue)
               query = query.Where(x => x.Grade.Evaluation.CourseTerm.TermId == termId.Value);

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByRoleDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, string searchValue = null, int? state = null, ClaimsPrincipal user= null)
        {
            Expression<Func<GradeCorrection, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Grade.StudentSection.Section.CourseTerm.Course.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Grade.StudentSection.Section.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Grade.StudentSection.Student.User.UserName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Grade.StudentSection.Student.User.FullName;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Grade.Evaluation.Name;
                    break;
                case "5":
                    //orderByPredicate = (x) => x.NewGrade;
                    break;
            }

            var query = _context.GradeCorrections.Where(x => !x.ToPay)
                      .AsQueryable();

            if (termId.HasValue)
                query = query.Where(x => x.Grade.Evaluation.CourseTerm.TermId == termId.Value);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                    query = query.Where(x => x.Teacher.Teachers.Any(y => y.AcademicDepartment.AcademicDepartmentDirectorId == userId));

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR))
                    query = query.Where(x => x.Grade.StudentSection.Section.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.Career.CareerDirectorId == userId));

            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Grade.StudentSection.Student.User.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Grade.StudentSection.Student.User.PaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Grade.StudentSection.Student.User.MaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Grade.StudentSection.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Grade.StudentSection.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Grade.StudentSection.Section.Code.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.Grade.StudentSection.Section.CourseTerm.Course.Name.ToUpper().Contains(searchValue.ToUpper()));

            }

            if (state.HasValue) query = query.Where(x => x.State == state);

            var recordsTotal = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Grade.StudentSection.Student.User.UserName,
                    student = x.Grade.StudentSection.Student.User.FullName,
                    course = x.Grade.StudentSection.Section.CourseTerm.Course.Name,
                    section = x.Grade.StudentSection.Section.Code,
                    evaluation = x.Grade.Evaluation.Name,
                    grade = x.NewGrade.HasValue ? x.NewGrade.ToString() : "Sin Asignar",
                    state = x.State,
                    observations = x.Observations,
                    termActive = x.Grade.Evaluation.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE,
                    file = x.FilePath,
                    prevGrade = x.OldGrade,
                    teacher = $"{x.Teacher.UserName} - {x.Teacher.FullName}",
                    date = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "-",
                    approvedBy = x.UpdatedBy,
                    x.NotTaken
                })
                .ToListAsync();

            var recordsFiltered = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId = null, Guid? termId = null, string searchValue = null, int? state = null)
        {
            Expression<Func<GradeCorrection, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Grade.StudentSection.Section.CourseTerm.Course.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Grade.StudentSection.Section.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Grade.StudentSection.Student.User.UserName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Grade.StudentSection.Student.User.FullName;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Grade.Evaluation.Name;
                    break;
                case "5":
                    //orderByPredicate = (x) => x.NewGrade;
                    break;
            }

            var query = _context.GradeCorrections.Where(x => !x.ToPay)
                      .AsQueryable();

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.TeacherId == teacherId);

            if (termId.HasValue)
                query = query.Where(x => x.Grade.Evaluation.CourseTerm.TermId == termId.Value);

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Grade.StudentSection.Student.User.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Grade.StudentSection.Student.User.PaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Grade.StudentSection.Student.User.MaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Grade.StudentSection.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Grade.StudentSection.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()) ||
                                         x.Grade.StudentSection.Section.Code.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.Grade.StudentSection.Section.CourseTerm.Course.Name.ToUpper().Contains(searchValue.ToUpper()));

            }

            if (state.HasValue) query = query.Where(x => x.State == state);

            var recordsTotal = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Grade.StudentSection.Student.User.UserName,
                    student = x.Grade.StudentSection.Student.User.FullName,
                    course = x.Grade.StudentSection.Section.CourseTerm.Course.Name,
                    section = x.Grade.StudentSection.Section.Code,
                    evaluation = x.Grade.Evaluation.Name,
                    grade = x.NewGrade.HasValue ? x.NewGrade.ToString() : "Sin Asignar",
                    state = x.State,
                    observations = x.Observations,
                    termActive = x.Grade.Evaluation.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE,
                    file = x.FilePath,
                    prevGrade = x.OldGrade,
                    teacher = $"{x.Teacher.UserName} - {x.Teacher.FullName}",
                    date = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "-"
                })
                .ToListAsync();

            var recordsFiltered = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<GradeCorrection> GetByTeacherStudent(string teacherId, Guid studentId)
            => await _context.GradeCorrections.Where(x => x.TeacherId == teacherId && x.Grade.StudentSection.Student.Id == studentId).FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetGradeCorrectionsRequestedByStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? studentId, string search)
        {
            Expression<Func<GradeCorrection, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Grade.StudentSection.Section.CourseTerm.Course.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Grade.StudentSection.Section.CourseTerm.Course.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Grade.StudentSection.Section.Code;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Grade.StudentSection.Student.User.UserName;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Grade.StudentSection.Student.User.FullName;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Grade.Evaluation.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var query = _context.GradeCorrections.Where(x => x.RequestedByStudent && x.Grade.StudentSection.Section.CourseTerm.TermId == termId).AsNoTracking();

            if (studentId.HasValue)
                query = query.Where(x => x.Grade.StudentSection.StudentId == studentId);

            var recordsTotal = await query.CountAsync();

            var data = await query
              .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
              .Skip(sentParameters.PagingFirstRecord)
              .Take(sentParameters.RecordsPerDraw)
              .Select(x => new
              {
                  x.Id,
                  courseCode = x.Grade.StudentSection.Section.CourseTerm.Course.Code,
                  courseName = x.Grade.StudentSection.Section.CourseTerm.Course.Name,
                  section = x.Grade.StudentSection.Section.Code,
                  evaluation = x.Grade.Evaluation.Name,
                  x.NewGrade,
                  x.OldGrade,
                  x.Observations,
                  x.FilePath,
                  x.State,
                  statusStr = ConstantHelpers.GRADECORRECTION_STATES.VALUES[x.State]
              })
              .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<bool> AnyGradeCorrectionByFilters(Guid gradeId, int status)
            => await _context.GradeCorrections.AnyAsync(x => x.GradeId == gradeId && x.State == status);
    }
}
