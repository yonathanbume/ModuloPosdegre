using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.DirectedCourses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class DirectedCourseRepository : Repository<DirectedCourse>, IDirectedCourseRepository
    {
        public DirectedCourseRepository(AkdemicContext context) : base(context)
        {
        }

        //private readonly Func<DirectedCourse, string[]> searchValuePredicate = (x) => new[] { x.Student.User.FullName, x.Student.User.UserName };
        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters,
            Guid? termId, Guid? careerId, Guid? facultyId, Guid? courseId, string searchValue = null, ClaimsPrincipal user = null)
        {
            Expression<Func<StudentSection, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Section.CourseTerm.Term.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.Code;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }
            var query = GetDataDirectedCourses(termId, careerId, facultyId, courseId, searchValue, user);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new DirectedCourseStudentTemplate
                {
                    Teacher = x.Section.TeacherSections.Select(x => x.Teacher.User.FullName).FirstOrDefault() ?? "--",
                    Id = x.Id,
                    CareerId = x.Section.CourseTerm.Course.CareerId.HasValue ? x.Section.CourseTerm.Course.CareerId.Value : Guid.Empty,
                    FacultyId = x.Section.CourseTerm.Course.CareerId.HasValue ? x.Section.CourseTerm.Course.Career.FacultyId : Guid.Empty,
                    Career = x.Section.CourseTerm.Course.CareerId.HasValue ? x.Section.CourseTerm.Course.Career.Name : "",
                    Faculty = x.Section.CourseTerm.Course.CareerId.HasValue ? x.Section.CourseTerm.Course.Career.Faculty.Name : "",
                    Term = x.Section.CourseTerm.Term.Name,
                    Course = x.Section.CourseTerm.Course.FullName,
                    Code = x.Student.User.UserName,
                    Name = x.Student.User.FullName,
                    Status = (byte)x.Status,
                    Grade = x.FinalGrade,
                    Courseid = x.Section.CourseTerm.CourseId,
                    Date = x.CreatedAt.ToLocalDateFormat()
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<List<DirectedCourseStudentTemplate>> GetDirectedCoursesDataReport(Guid? termId, Guid? careerId, Guid? facultyId, Guid? courseId, string searchValue, ClaimsPrincipal user)
        {
            var query = GetDataDirectedCourses(termId, careerId, facultyId, courseId, searchValue, user);
            return await query.Select(x => new DirectedCourseStudentTemplate
            {
                Teacher = x.Section.TeacherSections.Select(x => x.Teacher.User.FullName).FirstOrDefault() ?? "--",
                Id = x.Id,
                CareerId = x.Section.CourseTerm.Course.CareerId.HasValue ? x.Section.CourseTerm.Course.CareerId.Value : Guid.Empty,
                FacultyId = x.Section.CourseTerm.Course.CareerId.HasValue ? x.Section.CourseTerm.Course.Career.FacultyId : Guid.Empty,
                Career = x.Section.CourseTerm.Course.CareerId.HasValue ? x.Section.CourseTerm.Course.Career.Name : "",
                Faculty = x.Section.CourseTerm.Course.CareerId.HasValue ? x.Section.CourseTerm.Course.Career.Faculty.Name : "",
                Term = x.Section.CourseTerm.Term.Name,
                Course = x.Section.CourseTerm.Course.FullName,
                Code = x.Student.User.UserName,
                Name = x.Student.User.FullName,
                Status = (byte)x.Status,
                Grade = x.FinalGrade,
                Courseid = x.Section.CourseTerm.CourseId,
                Date = x.CreatedAt.ToLocalDateFormat(),
                Document = x.Student.User.Document
            })
            .ToListAsync();
        }
        private IQueryable<StudentSection> GetDataDirectedCourses(Guid? termId, Guid? careerId, Guid? facultyId, Guid? courseId, string searchValue, ClaimsPrincipal user)
        {
            var query = _context.StudentSections.Where(x => x.Section.IsDirectedCourse).AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                        .AsNoTracking();

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => x.Section.CourseTerm.Course.CareerId.HasValue && careers.Contains(x.Section.CourseTerm.Course.CareerId.Value));
                }
            }

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT) ))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                      .Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId || x.Faculty.AdministrativeAssistantId == userId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => x.Section.CourseTerm.Course.CareerId.HasValue && careers.Contains(x.Section.CourseTerm.Course.CareerId.Value));
                }

            }

            if (user.IsInRole(ConstantHelpers.ROLES.TEACHERS))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(x => x.Section.TeacherSections.Any(y => y.TeacherId == userId));
            }

            if (termId.HasValue && termId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);

            if (courseId.HasValue && courseId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.CourseId == courseId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x =>
                x.Section.CourseTerm.Course.Code.Contains(searchValue) ||
                x.Section.CourseTerm.Course.Name.Contains(searchValue) ||
                x.Student.User.UserName.Contains(searchValue) ||
                x.Student.User.Name.Contains(searchValue) ||
                x.Student.User.PaternalSurname.Contains(searchValue) ||
                x.Student.User.MaternalSurname.Contains(searchValue));
            }

            return query.OrderBy(x => x.Section.CourseTerm.Course.Code).ThenBy(x => x.Section.CourseTerm.Course.Name).ThenBy(x => x.Student.User.PaternalSurname);
        }

        public async Task<DirectedCourse> GetByFilters(Guid? term = null, Guid? careerid = null, Guid? courseId = null)
        {
            var query = _context.DirectedCourses.AsQueryable();

            if (term.HasValue && term != Guid.Empty)
                query = query.Where(x => x.TermId == term);

            if (careerid.HasValue && careerid != Guid.Empty)
                query = query.Where(x => x.CareerId == careerid);

            if (courseId.HasValue && courseId != Guid.Empty)
                query = query.Where(x => x.CourseId == courseId);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<int> CountAttempts(Guid? studentId = null, Guid? courseId = null)
        {
            var query = _context.DirectedCourses.AsQueryable();

            //if (studentId.HasValue && studentId != Guid.Empty) query = query.Where(x => x.StudentId == studentId);

            if (courseId.HasValue && courseId != Guid.Empty) query = query.Where(x => x.CourseId == courseId);

            return await query.CountAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string teacherId, ClaimsPrincipal user = null)
        {
            Expression<Func<DirectedCourse, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.Course.Name;
                    break;
            }

            //var query = _context.DirectedCourses
            //    .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
            //    .Where(x => x.TermId == termId)
            //    .Where(x => x.Teacher != null && x.Teacher.User != null)
            //    .GroupBy(x => new { x.Teacher.User, x.Course })
            //    .AsQueryable();

            var directedCourses = _context.DirectedCourses.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var academicDepartments = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartmentId).ToArrayAsync();
                    directedCourses = directedCourses.Where(x => x.Teacher.AcademicDepartmentId.HasValue && academicDepartments.Contains(x.Teacher.AcademicDepartmentId.Value));
                }
            }

            var query = directedCourses
                        //.OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                        .Where(x => x.TermId == termId)
                        .Where(x => x.Teacher != null && x.Teacher.User != null)
                        .Include(x => x.Teacher.User)
                        .Include(x => x.Course)
                        .AsEnumerable()
                        .GroupBy(x => new { x.Teacher.User, x.Course })
                        .ToList();

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.Key.User.Id == teacherId).ToList();

            var recordsFiltered = query.Count();

            var reports = await _context.EvaluationReports
                .Where(x => x.Course.CourseTerms.Any(y => y.TermId == termId)
                && x.Type == ConstantHelpers.Intranet.EvaluationReportType.DIRECTED_COURSE)
                .Select(x => new
                {
                    x.TeacherId,
                    x.CourseId,
                    x.Status,
                    x.PrintQuantity,
                    x.CreatedAt
                })
                .ToArrayAsync();

            var data = query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    courseId = x.Key.Course.Id,
                    teacherId = x.Key.User.Id,
                    course = x.Key.Course.FullName,
                    teacher = x.Key.User.FullName,
                    printQuantity = reports.Where(r => r.TeacherId == x.Key.User.Id && x.Key.Course.Id == r.CourseId).Select(r => r.PrintQuantity).FirstOrDefault() ?? 0,
                    wasGenerated = reports.Any(r => r.TeacherId == x.Key.User.Id && x.Key.Course.Id == r.CourseId),
                    status = reports.Any(r => r.TeacherId == x.Key.User.Id && x.Key.Course.Id == r.CourseId)
                        ? CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.NAMES[reports.FirstOrDefault(r => r.TeacherId == x.Key.User.Id && r.CourseId == x.Key.Course.Id).Status]
                        : CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.NAMES[CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.PENDING]
                })
                .ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDirectedCoursesByTeacherDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string teacherId)
        {
            Expression<Func<DirectedCourse, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.Course.Name;
                    break;
            }

            var query = _context.DirectedCourses
                .Where(x => x.TermId == termId && x.TeacherId == teacherId)
                .AsQueryable();

            var recordsFiltered = query.Count();

            var dataDB = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Course.Name,
                    x.Course.Code,
                    x.CourseId,
                    x.Course.TheoreticalHours,
                    students = x.Students.Count()
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.Id,
                    course = $"{x.Code}-{x.Name}",
                    totalhours = Math.Round((double)(x.TheoreticalHours / 2), 1, MidpointRounding.AwayFromZero),
                    cycles = string.Join(", ", _context.AcademicYearCourses.Where(y => y.CourseId == x.CourseId)
                      .Select(x => ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS.ContainsKey(x.AcademicYear) ? ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.AcademicYear] : "-").ToList()),
                    totalStudents = x.students
                })
                .ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IEnumerable<DirectedCourse>> GetAllByTeacherIdAndCourseId(Guid courseId, string teacherId)
            => await _context.DirectedCourses.Include(x => x.Students).Where(x => x.CourseId == courseId && x.TeacherId == teacherId).ToArrayAsync();

        public async Task<EnrollmentDirectedCourseDataTemplate> GetEnrollmentDirectedCourseData(Guid termId, Guid careerId, Guid curriculums)
        {
            var academicyears = await _context.AcademicYearCourses.Where(x => (x.Course.CareerId.HasValue) ? x.Course.CareerId.Value == careerId : false).ToListAsync();
            var studentDirected = await _context.DirectedCourseStudents
                  .Where(x => x.Student.CurriculumId == curriculums && x.DirectedCourse.TermId == termId)
                  .ToListAsync();

            var sqlData = _context.DirectedCourseStudents
                                            .Where(y => y.Student.CurriculumId == curriculums && y.DirectedCourse.TermId == termId && y.DirectedCourse.Course.CareerId.Value == careerId)
                                            .Include(y => y.DirectedCourse.Course)
                                            .ToList();

            var courses = sqlData
                .GroupBy(y => new { y.DirectedCourse.Course })
                .Select(x => new DirectedCourseDataTemplate
                {
                    AcademicYear = academicyears.FirstOrDefault(a => a.CourseId == x.Key.Course.Id) != null ? academicyears.FirstOrDefault(a => a.CourseId == x.Key.Course.Id).AcademicYear : Convert.ToByte(0),
                    CourseName = x.Key.Course.Name,
                    CourseCode = x.Key.Course.Code,
                    StudentDirectedCount = x.Count()
                })
                .ToList();

            var curriculum = await _context.Curriculums.Where(x => x.Id == curriculums).FirstOrDefaultAsync();

            return new EnrollmentDirectedCourseDataTemplate { StudentEnrollmentDirectedCourse = courses, CurriculumCode = int.Parse(curriculum.Code) };
        }
        public async Task<object> GetAllByStudentAndTerm(Guid termId, Guid studentId)
        {
            return await _context.StudentSections
                  .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId && x.Section.IsDirectedCourse)
                  .Select(x => new
                  {
                      id = x.Id,
                      coursename = x.Section.CourseTerm.Course.Name ?? "",
                      coursecode = x.Section.CourseTerm.Course.Code ?? "",
                      teacher = x.Section.TeacherSections.Select(y => y.Teacher.User.FullName).FirstOrDefault() ?? "",
                      credits = x.Section.CourseTerm.Course.Credits,
                  })
                  .ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable2(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null)
        {
            Expression<Func<DirectedCourse, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Term.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var query = _context.DirectedCourses.Where(x => x.Students.Count() > 0).AsNoTracking();

            if (user != null &&
                (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY) ||
                user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR)
                ))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId ||
                        x.CareerDirectorId == userId ||
                        x.AcademicDepartmentDirectorId == userId ||
                        x.AcademicSecretaryId == userId)
                        .AsNoTracking();

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

            //if (user.IsInRole(ConstantHelpers.ROLES.TEACHERS))
            //{
            //    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //    if (!string.IsNullOrEmpty(userId))
            //        query = query.Where(x => x.TeacherId == userId);
            //}

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Course.Name.ToUpper().Contains(searchValue.ToUpper())
                || x.Teacher.User.FullName.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    teacher = x.Teacher.User.FullName ?? "--",
                    id = x.Id,
                    term = x.Term.Name,
                    course = x.Course.FullName,
                    //code = x.Student.User.UserName,
                    //name = x.Student.User.FullName,
                    //status = x.Status,
                    //grade = x.Grade,
                    date = x.CreatedAt.ToLocalDateFormat()
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<DirectedCourse>> GetByCareerAndTerm(Guid careerid, Guid termId)
        {
            return await _context.DirectedCourses
                .Include(x => x.Course)
                .Where(x => x.CareerId == careerid &&
                            x.Students.Count > 0 &&
                            x.TermId == termId)
                .ToListAsync();
        }
    }
}
