using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicHistory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class AcademicHistoryRepository : Repository<AcademicHistory>, IAcademicHistoryRepository
    {
        public AcademicHistoryRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE
        private Expression<Func<AcademicHistoryTemplate, dynamic>> GetAcademicHistoryDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Name);
                case "1":
                    return ((x) => x.Code);
                case "2":
                    return ((x) => x.Career);
                case "3":
                    return ((x) => x.AcademicProgram);
                case "4":
                    return ((x) => x.Intents);
                case "5":
                    return ((x) => x.Grade);
                case "6":
                    return ((x) => x.Approbed);
                default:
                    return ((x) => x.Name);
            }
        }
        private Func<AcademicHistoryTemplate, string[]> GetAcademicHistoryDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Name+"",
                x.Code+"",
                x.Career+"",
                x.AcademicProgram+"",
                x.Intents+"",
                x.Grade+"",
                x.Approbed+"",
        };
        }
        private async Task<DataTablesStructs.ReturnedData<AcademicHistoryTemplate>> GetAcademicHistoryDatatable(
          DataTablesStructs.SentParameters sentParameters, Guid careerId, Guid curriculumId, Guid courseId, Guid termId, string name,
          Expression<Func<AcademicHistoryTemplate, AcademicHistoryTemplate>> selectPredicate = null,
          Expression<Func<AcademicHistoryTemplate, dynamic>> orderByPredicate = null,
          Func<AcademicHistoryTemplate, string[]> searchValuePredicate = null)
        {
            var query = _context.AcademicHistories
                //.Where(x => x.CourseId == courseId && x.TermId == termId && x.Course.AcademicYearCourses.Any(y=>y.Curriculum.CareerId == careerId && y.CurriculumId == curriculumId))
                .Where(x => x.CourseId == courseId && x.TermId == termId && x.Student.CareerId == careerId && x.Student.CurriculumId == curriculumId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(name.Trim().ToLower()));

            var recordsTotal = await query.CountAsync();

            var result = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new AcademicHistoryTemplate
                {
                    Name = x.Student.User.FullName,
                    Code = x.Student.User.UserName,
                    Career = x.Student.Career.Name,
                    AcademicProgram = x.Student.AcademicProgram.Name,
                    Approbed = x.Approved,
                    Grade = x.Grade,
                    Intents = x.Try
                })
                .ToListAsync();



            //var queryclient = await query
            //    .Select(x => new AcademicHistoryTemplate
            //    {
            //        Name = x.Student.User.FullName,
            //        Code = x.Student.User.UserName,
            //        Career = x.Student.Career.Name,
            //        AcademicProgram = x.Student.AcademicProgram.Name,
            //        Approbed = x.Approved,
            //        Grade = x.Grade,
            //        Intents = x.Try,
            //        Curriculums = x.Section.CourseTerm.Course.Career.Curriculums
            //    })
            //     .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
            //     .ToListAsync();

            //var recordsFiltered = queryclient.Count();
            //var result = queryclient.Where(x=>x.Curriculums.Any(y=>y.Id == curriculumId))
            //    .Select(x => new AcademicHistoryTemplate
            //    {
            //        Name = x.Name,
            //        Code = x.Code,
            //        Career = x.Career,
            //        AcademicProgram = x.AcademicProgram,
            //        Approbed = x.Approbed,
            //        Grade = x.Grade,
            //        Intents = x.Intents
            //    })
            //   .ToList();
            return new DataTablesStructs.ReturnedData<AcademicHistoryTemplate>
            {
                Data = result,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        #endregion

        #region PUBLIC

        public async Task<List<AcademicHistoryTemplate>> GetAcademicHistoryTemplate(Guid careerId, Guid curriculumId, Guid courseId, Guid termId)
        {
            var query = _context.AcademicHistories
                     //.Where(x => x.CourseId == courseId && x.TermId == termId && x.Course.AcademicYearCourses.Any(y=>y.Curriculum.CareerId == careerId && y.CurriculumId == curriculumId))
                     .Where(x => x.CourseId == courseId && x.TermId == termId && x.Student.CareerId == careerId && x.Student.CurriculumId == curriculumId)
                     .AsQueryable();

            var result = await query
                .Select(x => new AcademicHistoryTemplate
                {
                    Name = x.Student.User.FullName,
                    Code = x.Student.User.UserName,
                    Career = x.Student.Career.Name,
                    AcademicProgram = x.Student.AcademicProgram.Name,
                    Approbed = x.Approved,
                    Grade = x.Grade,
                    Intents = x.Try
                })
                .ToListAsync();

            return result;

        }

        public async Task<AcademicHistory> GetWithData(Guid id)
            => await _context.AcademicHistories.Where(x => x.Id == id)
            .Include(x => x.Section)
            .Include(x => x.Student).ThenInclude(x => x.User).
            FirstOrDefaultAsync();
        public async Task<DataTablesStructs.ReturnedData<AcademicHistoryTemplate>> GetAcademicHistoryDatatable(DataTablesStructs.SentParameters parameters, Guid careerId, Guid curriculumId, Guid courseId, Guid termId, string name)
        {
            return await GetAcademicHistoryDatatable(parameters, careerId, curriculumId, courseId, termId, name, null, GetAcademicHistoryDatatableOrderByPredicate(parameters), GetAcademicHistoryDatatableSearchValuePredicate());
        }

        public async Task<int> CountFailedAcademicHistoriesByStudent(Guid studentId)
        {
            var query = _context.AcademicHistories
                .Where(x => x.StudentId == studentId)
                .Where(x => x.Grade < 7)
                .AsQueryable();

            return await query.CountAsync();
        }

        public async Task<IEnumerable<AcademicHistory>> GetAcademicHistoriesByStudent(Guid studentId, Guid? termId = null, bool? validated = null, bool? approved = null)
        {
            var query = _context.AcademicHistories
                .Include(x => x.Course)
                .Include(x => x.Term)
                .Include(x => x.EvaluationReport)
                .Where(x => x.StudentId == studentId)
                .AsQueryable();

            if (termId.HasValue)
                query = query.Where(x => x.TermId == termId.Value);

            if (validated.HasValue)
                query = query.Where(x => x.Validated == validated.Value);

            if (approved.HasValue)
                query = query.Where(x => x.Approved == approved.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<AcademicHistoryReportTemplate>> GetAcademicHistoriesReportAsData(Guid careerId, Guid curriculumId, Guid courseId, Guid termId)
        {
            var academicHistories = await _context.AcademicHistories.Where(x => x.CourseId == courseId && x.TermId == termId && x.Student.CareerId == careerId && x.Student.CurriculumId == curriculumId)
                .Select(x => new
                {
                    x.Approved,
                    x.CourseId
                })
                .ToListAsync();

            var result = academicHistories.GroupBy(x => x.CourseId)
                .Select(x => new AcademicHistoryReportTemplate
                {
                    disapprobedname = "Desaprobados",
                    disapprobeds = x.Count(c => c.Approved == false),
                    approbedname = "Aprobados",
                    approbeds = x.Count(c => c.Approved == true)
                })
                .ToList();

            return result;
        }

        public async Task<AcademicHistory> GetLastByStudentAndCourse(Guid studentId, Guid courseId)
        {
            return await _context.AcademicHistories.Include(x => x.Term)
                                  .Where(x => x.CourseId == courseId && x.StudentId == studentId)
                                   .OrderByDescending(x => x.Approved)
                                  .ThenByDescending(x => x.Term.StartDate)
                                  .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CurriculumCourseTemplate>> GetCurriculumCoursesByStudentId(Guid studentId, bool electiveCourses)
        {
            var student = await _context.Students.FindAsync(studentId);
            var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == student.Id).Include(x => x.Term).OrderBy(x => x.Term.Name).ToListAsync();

            var query = _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId && x.IsElective == electiveCourses)
                .Include(x => x.Course)
                .AsQueryable();

            var academicYearCourses = await query.ToListAsync();

            var courses = academicYearCourses
               .Select(x => new CurriculumCourseTemplate
               {
                   Year = x.AcademicYear.ToString("D2"),
                   Code = x.Course.Code,
                   Course = x.Course.Name,
                   Credits = x.Course.Credits,
                   Tries = academicHistories.Where(ah => ah.CourseId == x.CourseId).Max(ah => ah.Try),
                   Grade = academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Name).Select(ah => ah.Grade).FirstOrDefault(),
                   Term = academicHistories.Where(ah => ah.CourseId == x.CourseId).OrderByDescending(ah => ah.Term.Name).Select(ah => ah.Term.Name).FirstOrDefault(),
                   Status = academicHistories.Any(ah => ah.CourseId == x.CourseId && ah.Approved) ? "Aprobado" : "Pendiente"
               })
               .OrderBy(x => x.Year)
               .ThenBy(x => x.Credits)
               .ToList();

            return courses;
        }

        public async Task<IEnumerable<AcademicHistory>> GetAllWithSectionAndTeacherSections(Guid? studentId = null, Guid? termId = null, bool? validated = null, bool? approved = null)
        {
            var query = _context.AcademicHistories
                .Include(x => x.Term)
                .Include(x => x.Course)
                .Include(x => x.Section.TeacherSections)
                .ThenInclude(x => x.Teacher.User)
                .AsNoTracking();

            if (studentId.HasValue)
                query = query.Where(x => x.StudentId == studentId.Value);

            if (termId.HasValue)
                query = query.Where(x => x.TermId == termId.Value);

            if (validated.HasValue)
                query = query.Where(x => x.Validated == validated.Value);

            if (approved.HasValue)
                query = query.Where(x => x.Approved == approved.Value);

            return await query.ToListAsync();
        }

        public async Task<List<CertificateTemplate>> GetListCertificateByStudentAndCurriculum(Guid studentId, Guid curriculumId)
        {
            var query = _context.AcademicYearCourses.AsNoTracking();
            var student = await _context.Students.Include(x => x.Career).Where(x => x.Id == studentId).FirstOrDefaultAsync();

            var courses = await query.Where(x => x.CurriculumId == student.CurriculumId).Select(x => x.CourseId).ToListAsync();

            var lstCertificate = await _context.AcademicHistories
                .Where(x => x.StudentId == studentId && courses.Contains(x.CourseId) && x.Approved)
                .Select(x => new CertificateTemplate
                {
                    CourseId = x.CourseId,
                    AcademicYear = _context.AcademicYearCourses.Where(y => y.CurriculumId == curriculumId && y.CourseId == x.Course.Id).Select(x => x.AcademicYear).FirstOrDefault(),
                    Type = _context.AcademicYearCourses.Where(y => y.CurriculumId == curriculumId && y.CourseId == x.Course.Id).Select(x => x.IsElective).FirstOrDefault() ? "E" : "O",
                    CourseCode = x.Course.Code,
                    CourseName = x.Course.Name,
                    //CourseName = x.Type != ConstantHelpers.AcademicHistory.Types.REGULAR ? $"{x.Course.Name} {ConstantHelpers.AcademicHistory.Types.ABREVIATIONS[x.Type]}" : x.Course.Name,
                    Credits = x.Course.Credits.ToString("0.0"),
                    Grade = x.Grade,
                    Observations = x.Observations,
                    TermName = x.Term.Name,
                    TermMinGrade = x.Term.MinGrade,
                    TermId = x.TermId,
                    EvaluationReportDate = x.EvaluationReportId.HasValue ? x.EvaluationReport.CreatedAt.ToLocalDateFormat() : string.Empty,
                })
                .OrderBy(x => x.TermName)
                .ToListAsync();

            var grouped = lstCertificate
                .GroupBy(x => x.CourseId)
                .Select(x => new CertificateTemplate
                {
                    CourseId = x.Key,
                    AcademicYear = x.Select(y => y.AcademicYear).FirstOrDefault(),
                    CourseCode = x.Select(y => y.CourseCode).FirstOrDefault(),
                    CourseName = x.Select(y => y.CourseName).FirstOrDefault(),
                    Credits = x.Select(y => y.Credits).FirstOrDefault(),
                    Grade = x.OrderByDescending(y => y.Grade).Select(y => y.Grade).FirstOrDefault(),
                    Observations = x.OrderByDescending(y => y.Grade).Select(y => y.Observations).FirstOrDefault(),
                    TermName = x.Select(y => y.TermName).FirstOrDefault(),
                    TermMinGrade = x.Select(y => y.TermMinGrade).FirstOrDefault(),
                    TermId = x.Select(y => y.TermId).FirstOrDefault(),
                    EvaluationReportDate = x.OrderByDescending(y => y.Grade).Select(y => y.EvaluationReportDate).FirstOrDefault(),
                    Type = x.Select(y => y.Type).FirstOrDefault(),
                })
                .ToList();

            return grouped;
        }

        public async Task<int> GetCoursesDisapprovedByStudentId(Guid studentId)
        {
            var coursesDisapproved = await _context.AcademicHistories.Where(x => x.StudentId == studentId && !x.Approved).CountAsync();

            return coursesDisapproved;
        }

        public async Task<int> GetCoursesRecoveredByStudentId(Guid studentId)
        {
            var coursesRecovered = await _context.AcademicHistories.Where(x => x.StudentId == studentId && x.Try > 1).CountAsync();

            return coursesRecovered;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsFourth(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string userId, Guid? faculty = null, Guid? career = null, string search = null)
        {
            Expression<Func<AcademicHistory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.Career.Faculty.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.StudentId;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                default:
                    //orderByPredicate = (x) => x.Student.Career.Faculty.Name;
                    break;
            }

            var academicHistories = await _context.AcademicHistories
                 .Where(x => x.Student.Status == ConstantHelpers.Student.States.IRREGULAR
                 || x.Student.Status == ConstantHelpers.Student.States.REPEATER
                 || x.Student.Status == ConstantHelpers.Student.States.REGULAR
                 || x.Student.Status == ConstantHelpers.Student.States.UNBEATEN
                 || x.Student.Status == ConstantHelpers.Student.States.OBSERVED
                 || x.Student.Status == ConstantHelpers.Student.States.SANCTIONED)
                 .Where(x => x.Approved).ToArrayAsync();

            var query = _context.AcademicHistories
                  .Where(x => x.Student.Status == ConstantHelpers.Student.States.IRREGULAR
                || x.Student.Status == ConstantHelpers.Student.States.REPEATER
                || x.Student.Status == ConstantHelpers.Student.States.REGULAR
                || x.Student.Status == ConstantHelpers.Student.States.UNBEATEN
                || x.Student.Status == ConstantHelpers.Student.States.OBSERVED
                || x.Student.Status == ConstantHelpers.Student.States.SANCTIONED)
                .Where(x => !x.Withdraw && x.Try >= 3)
                .AsNoTracking();


            if (faculty.HasValue && faculty != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == faculty);

            if (career.HasValue && career != Guid.Empty) query = query.Where(x => x.Student.CareerId == career);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(search.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(search.ToUpper()));

            //query = query.Where(x => !academicHistories.Any(ah => ah.StudentId == x.StudentId && ah.CourseId == x.CourseId));

            //var recordsFiltered = await query.CountAsync();

            var totalData = await query
                .Select(x => new
                {
                    career = x.Student.Career.Name,
                    faculty = x.Student.Career.Faculty.Name,
                    studentId = x.StudentId,
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,
                    courseId = x.CourseId,
                    course = x.Course.Name,
                    academicYear = x.Student.CurrentAcademicYear,
                    approved = x.Approved,
                    @try = x.Try
                }).ToListAsync();

            totalData = totalData.Where(x => !academicHistories.Any(ah => ah.StudentId == x.studentId && ah.CourseId == x.courseId)).ToList();
            var recordsFiltered = totalData.Count;

            var students = totalData.GroupBy(y => new { y.courseId, y.studentId })
                //.Where(y => y.Count() == 1 && y.All(x => !x.approved))
                .Select(x => x.OrderByDescending(y => y.@try).First());

            //var data = await query
            //    .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
            //    .Skip(sentParameters.PagingFirstRecord)
            //    .Take(sentParameters.RecordsPerDraw)
            //    .Select(x => new
            //    {
            //        career = x.Student.Career.Name,
            //        faculty = x.Student.Career.Faculty.Name,
            //        studentId = x.StudentId,
            //        code = x.Student.User.UserName,
            //        name = x.Student.User.FullName,
            //        courseId = x.CourseId,
            //        course = x.Course.Name,
            //        academicYear = x.Student.CurrentAcademicYear,
            //        approved = x.Approved
            //    }).ToListAsync();

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    students = sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION ?
                        students.OrderByDescending(x => x.career) : students.OrderBy(x => x.career);
                    break;
                case "1":
                    students = sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION ?
                      students.OrderByDescending(x => x.faculty) : students.OrderBy(x => x.faculty);
                    break;
                case "2":
                    //orderByPredicate = (x) => x.StudentId;
                    //students = sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION ?
                    //  students.OrderByDescending(x => x.career) : students.OrderBy(x => x.career);
                    break;
                case "3":
                    students = sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION ?
                      students.OrderByDescending(x => x.code) : students.OrderBy(x => x.code);
                    break;
                default:
                    //orderByPredicate = (x) => x.Student.Career.Faculty.Name;
                    break;
            }

            var data = students
                //.OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<List<FourthExcelTemplate>> GetStudentToExcelFourth(Guid? faculty = null, Guid? career = null, ClaimsPrincipal user = null)
        {
            var query = _context.AcademicHistories
       .Where(x => x.Student.Status == ConstantHelpers.Student.States.IRREGULAR
                || x.Student.Status == ConstantHelpers.Student.States.REPEATER
                || x.Student.Status == ConstantHelpers.Student.States.REGULAR
                || x.Student.Status == ConstantHelpers.Student.States.UNBEATEN
                || x.Student.Status == ConstantHelpers.Student.States.OBSERVED
                || x.Student.Status == ConstantHelpers.Student.States.SANCTIONED)
                                .Where(x => !x.Validated && !x.Withdraw)
                .Where(x => x.Try >= 3)
                .AsQueryable();


            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (faculty.HasValue && faculty != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

                    if (career.HasValue && career != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == career);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (faculty != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == faculty);

                if (career != Guid.Empty) query = query.Where(x => x.Student.CareerId == career);
            }

            var data = await query
            .Select(x => new
            {
                career = x.Student.Career.Name,
                faculty = x.Student.Career.Faculty.Name,
                studentId = x.StudentId,
                code = x.Student.User.UserName,
                name = x.Student.User.FullName,
                courseId = x.CourseId,
                course = x.Course.Name,
                academicYear = x.Student.CurrentAcademicYear,
                approved = x.Approved
            }).ToListAsync();

            var students = data.GroupBy(y => new { y.courseId, y.studentId })
                .Where(y => y.Count() == 1 && y.All(x => !x.approved))
             .Select(x => x.First());

            var listStudent = new List<FourthExcelTemplate>();
            foreach (var item in students)
            {
                var student = new FourthExcelTemplate();
                student.Course = item.course;
                student.Code = item.code;
                student.Career = item.career;
                student.Faculty = item.faculty;
                student.Name = item.name;
                student.AcademicYear = item.academicYear;

                listStudent.Add(student);
            }

            return listStudent;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsThirdDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
        {
            Expression<Func<AcademicHistory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.Career.Faculty.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.StudentId;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                default:
                    break;
            }
            var academicHistories = await _context.AcademicHistories
                .Where(x => x.Student.Status == ConstantHelpers.Student.States.IRREGULAR
                || x.Student.Status == ConstantHelpers.Student.States.REPEATER
                || x.Student.Status == ConstantHelpers.Student.States.REGULAR
                || x.Student.Status == ConstantHelpers.Student.States.UNBEATEN
                || x.Student.Status == ConstantHelpers.Student.States.OBSERVED
                || x.Student.Status == ConstantHelpers.Student.States.SANCTIONED)
                .Where(x => x.Try > 2).ToListAsync();

            var query = _context.AcademicHistories
                  .Where(x => x.Student.Status == ConstantHelpers.Student.States.IRREGULAR
                || x.Student.Status == ConstantHelpers.Student.States.REPEATER
                || x.Student.Status == ConstantHelpers.Student.States.REGULAR
                || x.Student.Status == ConstantHelpers.Student.States.UNBEATEN
                || x.Student.Status == ConstantHelpers.Student.States.OBSERVED
                || x.Student.Status == ConstantHelpers.Student.States.SANCTIONED)
                .Where(x => !x.Approved && !x.Withdraw)
                .Where(x => x.Try == 2)
                .AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (faculty.HasValue && faculty != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

                    if (career.HasValue && career != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == career);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (faculty.HasValue && faculty != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == faculty);

                if (career.HasValue && career != Guid.Empty) query = query.Where(x => x.Student.CareerId == career);
            }

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(search.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(search.ToUpper()));

            //query = query.Where(x => academicHistories.Any(ah => ah.StudentId == x.StudentId && ah.CourseId == x.CourseId));

            //var academicHistoriesHash = academicHistories.Select(x => new { x.StudentId, x.CourseId }).ToHashSet();
            //query = query.Where(x => academicHistoriesHash.Contains(new { x.StudentId, x.CourseId }));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                //.Skip(sentParameters.PagingFirstRecord)
                //.Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    career = x.Student.Career.Name,
                    faculty = x.Student.Career.Faculty.Name,
                    studentId = x.StudentId,
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,
                    courseId = x.CourseId,
                    course = x.Course.Name,
                    academicYear = x.Student.CurrentAcademicYear,
                    approved = x.Approved
                }).ToListAsync();

            data = data
                .Where(x => !academicHistories.Any(ah => ah.StudentId == x.studentId && ah.CourseId == x.courseId))
                //.Skip(sentParameters.PagingFirstRecord)
                //.Take(sentParameters.RecordsPerDraw)
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

        public async Task<List<ThridExcelTemplate>> GetStudentToExcelThird(Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
        {
            var academicHistories = await _context.AcademicHistories
              .Where(x => x.Student.Status == ConstantHelpers.Student.States.IRREGULAR
              || x.Student.Status == ConstantHelpers.Student.States.REPEATER
              || x.Student.Status == ConstantHelpers.Student.States.REGULAR
              || x.Student.Status == ConstantHelpers.Student.States.UNBEATEN
              || x.Student.Status == ConstantHelpers.Student.States.OBSERVED
              || x.Student.Status == ConstantHelpers.Student.States.SANCTIONED)
              .Where(x => x.Try > 2).ToListAsync();

            var query = _context.AcademicHistories
                .Where(x => x.Student.Status == ConstantHelpers.Student.States.IRREGULAR
                || x.Student.Status == ConstantHelpers.Student.States.REPEATER
                || x.Student.Status == ConstantHelpers.Student.States.REGULAR
                || x.Student.Status == ConstantHelpers.Student.States.UNBEATEN
                || x.Student.Status == ConstantHelpers.Student.States.OBSERVED
                || x.Student.Status == ConstantHelpers.Student.States.SANCTIONED)
                .Where(x => !x.Validated && !x.Withdraw)
                .Where(x => x.Try == 2)
                .AsQueryable();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (faculty.HasValue && faculty != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

                    if (career.HasValue && career != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == career);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (faculty != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == faculty);

                if (career != Guid.Empty) query = query.Where(x => x.Student.CareerId == career);
            }

            var data = await query
            .Select(x => new
            {
                career = x.Student.Career.Name,
                faculty = x.Student.Career.Faculty.Name,
                studentId = x.StudentId,
                code = x.Student.User.UserName,
                name = x.Student.User.FullName,
                courseId = x.CourseId,
                course = x.Course.Name,
                academicYear = x.Student.CurrentAcademicYear,
                approved = x.Approved
            }).ToListAsync();

            data = data
              .Where(x => !academicHistories.Any(ah => ah.StudentId == x.studentId && ah.CourseId == x.courseId))
              .ToList();

            var listStudent = new List<ThridExcelTemplate>();
            foreach (var item in data)
            {
                var student = new ThridExcelTemplate();
                student.Course = item.course;
                student.Code = item.code;
                student.Career = item.career;
                student.Faculty = item.faculty;
                student.Name = item.name;
                student.AcademicYear = item.academicYear;

                listStudent.Add(student);
            }

            return listStudent;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExtraordinaryEvaluationHistoryDatatable(DataTablesStructs.SentParameters sentParameters, Guid? term = null, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
        {
            Expression<Func<AcademicHistory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Student.Career.Faculty.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Approved;
                    break;
                case "5":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                default:
                    break;
            }

            var query = _context.AcademicHistories
                  .Where(x => x.Type == ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION)
                  .AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (faculty.HasValue && faculty != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

                    if (career.HasValue && career != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == career);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (faculty.HasValue && faculty != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == faculty);

                if (career.HasValue && career != Guid.Empty) query = query.Where(x => x.Student.CareerId == career);
            }

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(search.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Student.User.FullName,
                    code = x.Student.User.UserName,
                    career = x.Student.Career.Name,
                    faculty = x.Student.Career.Faculty.Name,
                    date = x.ParsedCreatedAt,
                    approved = x.Approved
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

        public async Task<List<AcademicHistory>> GetByStudentId(Guid studentId)
        {
            var academicHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == studentId)
                .Include(x => x.Term)
                .Include(x => x.Course)
                .ToListAsync();

            return academicHistories;
        }

        public async Task<AcademicHistory> GetApprovedsNyCourseIdAndStudentId(Guid courseId, Guid studentid)
        {
            return await _context.AcademicHistories
                                .Include(x => x.Course)
                                .FirstOrDefaultAsync(x => x.Approved == true && x.CourseId == courseId && x.StudentId == studentid);
        }
        public async Task<int> GetDissaprovedCount(Guid studentId)
        {
            var aprovedAcademicHistories = await _context.AcademicHistories
                  .Where(x => x.StudentId == studentId && x.Approved)
                  .ToListAsync();
            var disaprovedAcademicHistories = await _context.AcademicHistories
                  .Where(x => x.StudentId == studentId && !x.Approved && !aprovedAcademicHistories.Any(y => y.Id == x.Id))
                  .ToListAsync();
            return await _context.TmpEnrollments
                    .Where(tmp => tmp.StudentId == studentId &&
                        tmp.Section.CourseTerm.Term.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.ACTIVE &&
                        disaprovedAcademicHistories.Any(d => d.CourseId == tmp.Section.CourseTerm.CourseId))
                    .CountAsync();
        }
        public async Task<bool> AnyDisaprovedAcademicHistories(Guid courseId, Guid studentId)
        {
            //var aprovedAcademicHistories = await _context.AcademicHistories
            //   .Where(x => x.StudentId == studentId && x.Approved)
            //   .ToListAsync();

            //var disaprovedAcademicHistories = await _context.AcademicHistories
            //    .Where(x => x.StudentId == studentId && !x.Approved && !aprovedAcademicHistories.Any(y => y.Id == x.Id))
            //    .ToListAsync();

            var any = await _context.DisapprovedCourses.AnyAsync(x => x.StudentId == studentId && x.CourseId == courseId);
            return any;
        }
        public async Task<byte> GetLowestAcademicYear(Guid studentId, Guid curriculumId)
        {
            var aprovedAcademicHistories = await _context.AcademicHistories
                  .Where(x => x.StudentId == studentId && x.Approved)
                  .ToListAsync();

            var disaprovedAcademicHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == studentId && !x.Approved && !aprovedAcademicHistories.Any(y => y.Id == x.Id))
                .ToListAsync();

            var lowestAcademicYear = disaprovedAcademicHistories
                 .Where(x => !x.Approved && x.StudentId == studentId)
                 .Select(x =>
                     _context.AcademicYearCourses
                         .Where(y => y.CourseId == x.CourseId && y.CurriculumId == curriculumId)
                         .Select(y => y.AcademicYear).FirstOrDefault()
                 )
                 .OrderBy(x => x)
                 .FirstOrDefault();

            return lowestAcademicYear;
        }
        public async Task<string> GetTimesByCourseAndUserId(Guid courseId, string userId)
        {
            var query = _context.AcademicHistories
                .Where(x => x.CourseId == courseId && x.Student.UserId == userId)
                .AsNoTracking();

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD)
                query = query.Where(x => x.Term.Number != "A" && !x.Term.IsSummer);
            else query = query.Where(x => x.Term.Number != "A" && x.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION);

            return ((await query.CountAsync()) + 1).ToString("D2");
        }
        public async Task<List<AcademicHistory>> GetApprovedByStudentId(Guid studentId)
        {
            return await _context.AcademicHistories
                            .Where(x => x.StudentId == studentId && x.Approved)
                            .ToListAsync();
        }

        public async Task UpdateAcademicHistoriesJob(string connectionString)
        {
            var academicHistories = _context.AcademicHistories
                            .OrderBy(x => x.CourseId)
                            .ThenBy(x => x.SectionId)
                            .ThenBy(x => x.StudentId)
                            .ThenBy(x => x.TermId)
                            .ThenByDescending(x => x.Type)
                            .GroupBy(x => new { x.CourseId, x.SectionId, x.StudentId, x.TermId });
            var academicHistoriesSecond = academicHistories.Select(x => x.FirstOrDefault());
            var academicHistoriesDuplicates = await _context.AcademicHistories
                .Except(academicHistoriesSecond)
                .ToListAsync();
            var academicHistoriesSecond2 = await academicHistoriesSecond.ToListAsync();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    using (var sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = $"UPDATE {ConstantHelpers.ENTITY_MODELS.INTRANET.ACADEMIC_HISTORY} SET Type = 1 WHERE Id = @Id";
                        sqlCommand.Transaction = sqlTransaction;

                        sqlCommand.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier);
                        sqlCommand.Prepare();

                        for (var i = 0; i < academicHistoriesSecond2.Count; i++)
                        {
                            var academicHistoriesSecondItem = academicHistoriesSecond2[i];
                            sqlCommand.Parameters["@Id"].Value = academicHistoriesSecondItem.Id;

                            await sqlCommand.ExecuteNonQueryAsync();
                        }
                    }

                    using (var sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = $"DELETE FROM {ConstantHelpers.ENTITY_MODELS.INTRANET.ACADEMIC_HISTORY} WHERE Id = @Id";
                        sqlCommand.Transaction = sqlTransaction;

                        sqlCommand.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier);
                        sqlCommand.Prepare();

                        for (var i = 0; i < academicHistoriesDuplicates.Count; i++)
                        {
                            var academicHistoriesDuplicate = academicHistoriesDuplicates[i];
                            sqlCommand.Parameters["@Id"].Value = academicHistoriesDuplicate.Id;

                            await sqlCommand.ExecuteNonQueryAsync();
                        }
                    }

                    sqlTransaction.Commit();
                }
            }
        }
        public async Task UpdateAcademicHistoryTryJob(string code)
        {
            var queryHistories = _context.AcademicHistories.AsQueryable();
            var queryStudents = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(code))
            {
                var career = await _context.Careers.FirstOrDefaultAsync(x => x.Code.ToUpper() == code.ToUpper());
                queryHistories = queryHistories.Where(x => x.Student.CareerId == career.Id);
                queryStudents = queryStudents.Where(x => x.CareerId == career.Id);
            }

            var students = await queryStudents.ToListAsync();
            var academicHistoriesTotal = await queryHistories.Include(x => x.Term).ToListAsync();
            var count = 0;
            var total = 0;

            foreach (var student in students)
            {
                var academicHistories = academicHistoriesTotal
                    .Where(x => x.StudentId == student.Id)
                    .OrderBy(x => x.Term.Year)
                    .ThenBy(x => x.Term.Number)
                    .ToList();

                var historyProgress = new List<AcademicHistory>();

                foreach (var item in academicHistories)
                {
                    item.Try = historyProgress.Count(x => x.CourseId == item.CourseId) + 1;

                    historyProgress.Add(item);

                    count++;
                }

                //if (academicHistories.Any(x => x.Try >= 3 && academicHistories.Where(y => y.CourseId == x.CourseId).All(y => !y.Approved)))
                //{
                //    student.Status = CORE.Helpers.ConstantHelpers.Student.States.SANCTIONED;
                //}

                total++;

                if (count >= 250)
                {
                    await _context.SaveChangesAsync();
                    count = 0;
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task StudentsCurriculumConvalidationJob(Guid careerId)
        {
            var curriculums = await _context.Curriculums.Where(x => x.CareerId == careerId).ToListAsync();
            var currentCurriculumId = curriculums.OrderBy(x => x.Year).ThenBy(x => x.Code).Last().Id;
            var prevCurriculumId = curriculums.OrderBy(x => x.Year).ThenBy(x => x.Code).SkipLast(1).Last().Id;

            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            var students = await _context.Students.Where(x => x.CurriculumId == currentCurriculumId).ToListAsync();

            var courseEquivalences = await _context.CourseEquivalences
                .Where(x => x.OldAcademicYearCourse.CurriculumId == prevCurriculumId)
                .Include(x => x.NewAcademicYearCourse).Include(x => x.OldAcademicYearCourse).AsNoTracking().ToListAsync();

            var courses = courseEquivalences.Select(x => x.NewAcademicYearCourse).Distinct().ToList();

            foreach (var student in students)
            {
                var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == student.Id && x.Approved).ToListAsync();

                var newAcademicHistories = new List<AcademicHistory>();

                foreach (var course in courses)
                {
                    var equivalences = courseEquivalences.Where(x => x.NewAcademicYearCourseId == course.Id).ToList();
                    var validate = equivalences.All(x => academicHistories.Any(ah => ah.CourseId == x.OldAcademicYearCourse.CourseId));

                    if (validate)
                    {
                        var wasConvalidated = await _context.AcademicHistories.AnyAsync(x => x.StudentId == student.Id && x.CourseId == course.CourseId && x.Approved);

                        if (!wasConvalidated)
                        {
                            var grade = -1;
                            var replaceEquivalence = equivalences.Where(x => x.ReplaceGrade).FirstOrDefault();

                            if (replaceEquivalence != null)
                            {
                                grade = academicHistories.Where(x => x.CourseId == replaceEquivalence.OldAcademicYearCourse.CourseId).First().Grade;
                            }

                            var academicHistory = new AcademicHistory
                            {
                                CourseId = course.CourseId,
                                Approved = true,
                                Validated = true,
                                TermId = term.Id,
                                Observations = $"Convalidado por llevar los cursos correspondientes de la malla anterior",
                                StudentId = student.Id,
                                Try = 1,
                                Type = CORE.Helpers.ConstantHelpers.AcademicHistory.Types.REGULAR,
                                Grade = grade,
                                CurriculumId = student.CurriculumId
                            };

                            newAcademicHistories.Add(academicHistory);
                        }
                    }
                }

                await _context.AcademicHistories.AddRangeAsync(newAcademicHistories);
                await _context.SaveChangesAsync();
            }
        }
        public async Task StudentsCurriculumConvalidationJob(Guid studentId, Guid prevCurriculumId)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            var student = await _context.Students.FindAsync(studentId);

            var courseEquivalences = await _context.CourseEquivalences
                .Where(x => x.OldAcademicYearCourse.CurriculumId == prevCurriculumId)
                .Include(x => x.NewAcademicYearCourse).Include(x => x.OldAcademicYearCourse).AsNoTracking().ToListAsync();

            var courses = courseEquivalences.Select(x => x.NewAcademicYearCourse).Distinct().ToList();

            var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == student.Id && x.Approved).ToListAsync();

            var newAcademicHistories = new List<AcademicHistory>();

            foreach (var course in courses)
            {
                var equivalences = courseEquivalences.Where(x => x.NewAcademicYearCourseId == course.Id).ToList();
                //var equivalence = equivalences.FirstOrDefault();
                //var academicHistory1 = academicHistories.Where(x => equivalence.OldAcademicYearCourse.CourseId == x.CourseId).ToList();
                var validate = equivalences.All(x => academicHistories.Any(ah => ah.CourseId == x.OldAcademicYearCourse.CourseId));

                if (validate)
                {
                    var wasConvalidated = await _context.AcademicHistories.AnyAsync(x => x.StudentId == student.Id && x.CourseId == course.CourseId && x.Approved);

                    if (!wasConvalidated)
                    {
                        var grade = -1;
                        var replaceEquivalence = equivalences.Where(x => x.ReplaceGrade).FirstOrDefault();

                        if (replaceEquivalence != null)
                        {
                            grade = academicHistories.Where(x => x.CourseId == replaceEquivalence.OldAcademicYearCourse.CourseId).First().Grade;
                        }

                        var academicHistory = new AcademicHistory
                        {
                            CourseId = course.CourseId,
                            Approved = true,
                            Validated = true,
                            TermId = term.Id,
                            Observations = $"Convalidado por llevar los cursos correspondientes de la malla anterior",
                            StudentId = student.Id,
                            Try = 1,
                            Type = CORE.Helpers.ConstantHelpers.AcademicHistory.Types.REGULAR,
                            Grade = grade,
                            CurriculumId = student.CurriculumId
                        };

                        newAcademicHistories.Add(academicHistory);
                    }
                }
            }

            await _context.AcademicHistories.AddRangeAsync(newAcademicHistories);
            await _context.SaveChangesAsync();
        }

        public async Task<AcademicHistory> GetByStudentIdAndCourseId(Guid studentId, Guid courseId)
        {
            var recognitions = await _context.CoursesRecognition
               .Where(x => x.Recognition.StudentId == studentId)
               .Select(x => x.CourseId)
               .ToArrayAsync();

            var academicHistoriesHash = await _context.AcademicHistories
                .Where(x => x.StudentId == studentId &&
                                recognitions.Contains(x.CourseId))
                .ToListAsync();

            return academicHistoriesHash.FirstOrDefault();
        }

        public async Task<List<AcademicHistory>> GetAcademicHistoryValidated(Guid studentId, Guid termId)
            => await _context.AcademicHistories.Where(x => x.StudentId == studentId && x.TermId == termId && x.Validated).Include(x => x.Course).ToListAsync();

        public async Task<bool> IsTryHigherThan(Guid studentId, int number)
        {
            bool isLockedOut = false;

            if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAMAD)
            {
                var academicHistories = await _context.AcademicHistories
                    .Where(x => x.StudentId == studentId && x.Course.AcademicYearCourses.Any(y => y.CurriculumId == x.Student.CurriculumId) && !x.Withdraw)
                    .Select(x => new { x.CourseId, x.Approved, x.Term.Number, x.Term.IsSummer })
                    .ToListAsync();

                var approvedCourses = academicHistories.Where(x => x.Approved).Select(x => x.CourseId).Distinct().ToList();
                var pendingCourses = academicHistories.Where(x => !approvedCourses.Contains(x.CourseId)).ToList();

                var pendingCoursesTime = pendingCourses
                                .GroupBy(x => x.CourseId)
                                .Select(x => new
                                {
                                    x.Key,
                                    count = x.Count() + 1
                                }).ToList();

                isLockedOut = pendingCoursesTime.Any(x => x.count > number);
            }

            return isLockedOut;
        }

        public async Task<AcademicHistory> GetAcademicHistoryByStudentAndSectionId(Guid studentId, Guid sectionId)
            => await _context.AcademicHistories.Where(x => x.StudentId == studentId && x.SectionId == sectionId).FirstOrDefaultAsync();

        public async Task<AcademicHistory> GetAcademicHistoryBystudentAndCourseId(Guid studentId, Guid courseId, Guid termId)
            => await _context.AcademicHistories.Where(x => x.StudentId == studentId && x.CourseId == courseId && x.TermId == termId).FirstOrDefaultAsync();

        public async Task<Select2Structs.ResponseParameters> GetTAcademicHistoryByCourseAndTerm(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? courseId = null, Guid? termId = null)
        {
            var query = _context.AcademicHistories.AsNoTracking();

            //var teacherSections = _context.TeacherSections.AsNoTracking();


            if (termId != null)
                query = query.Where(x => x.TermId == termId);

            if (courseId.HasValue)
                query = query.Where(x => x.CourseId == courseId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.Student.User.FullName, searchValue));
                }
                else
                    query = query.Where(x => x.Student.User.FullName.Contains(searchValue));
            }

            //query = query.Where(x => teacherSections.Any(y => y.TeacherId == x.UserId));

            return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = $"{x.Student.User.UserName} - {x.Student.User.FullName} - {x.Grade}"

            }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE); ;

        }

        public async Task<List<AcademicHistoryYearTemplate>> GetAcademicHistoriesByYearRange(int startYear, int endYear)
        {
            var academichistories = await _context.AcademicHistories
                .Where(x => !x.Term.IsSummer && startYear <= x.Term.Year && x.Term.Year <= endYear
                && (x.Type != ConstantHelpers.AcademicHistory.Types.REGULAR || (x.Type == ConstantHelpers.AcademicHistory.Types.REGULAR && !x.Validated)))
                .Select(x => new AcademicHistoryYearTemplate
                {
                    Grade = x.Grade,
                    Term = x.Term.Name,
                    Number = x.Term.Number,
                    Year = x.Term.Year
                }).ToListAsync();

            return academichistories;
        }

        public async Task<List<TermDisapprovedGradeTemplate>> GetDisapprovedAcademicHistoriesByYearRange(int startYear, int endYear)
        {
            var academichistories = await _context.AcademicHistories
                .Where(x => !x.Term.IsSummer && startYear <= x.Term.Year && x.Term.Year <= endYear && !x.Approved)
                .Select(x => new AcademicHistoryYearTemplate
                {
                    Grade = x.Grade,
                    Term = x.Term.Name,
                    Number = x.Term.Number,
                    Year = x.Term.Year,
                    Try = x.Try
                }).ToListAsync();

            var terms = await _context.Terms
                .Where(x => !x.IsSummer && startYear <= x.Year && x.Year <= endYear)
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Number)
                .Select(x => new
                {
                    Term = x.Name,
                    FirstTry = x.AcademicHistories.Where(y => !y.Approved && y.Try == 1).Count(),
                    SecondTry = x.AcademicHistories.Where(y => !y.Approved && y.Try == 2).Count(),
                    ThirdTry = x.AcademicHistories.Where(y => !y.Approved && y.Try >= 3).Count(),
                    Total = x.AcademicHistories.Count()
                }).ToListAsync();

            var data = terms
                .Select(x => new TermDisapprovedGradeTemplate
                {
                    Term = x.Term,
                    FirstTry = x.FirstTry * 1.0 / x.Total,
                    SecondTry = x.SecondTry * 1.0 / x.Total,
                    ThirdOrMoreTry = x.ThirdTry * 1.0 / x.Total,
                }).ToList();

            return data;
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseRecognitionReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid term, Guid? faculty = null, Guid? career = null, string search = null)
        {
            Expression<Func<AcademicHistory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Term.Name; break;
                case "1":
                    orderByPredicate = (x) => x.Student.Career.Name; break;
                case "2":
                    orderByPredicate = (x) => x.Student.User.UserName; break;
                case "3":
                    orderByPredicate = (x) => x.Course.Code; break;
                case "4":
                    orderByPredicate = (x) => x.Course.Name; break;
                case "5":
                    orderByPredicate = (x) => x.Grade; break;
                case "6":
                    orderByPredicate = (x) => x.Observations; break;
                default:
                    break;
            }

            var query = _context.AcademicHistories
                  .Where(x => x.TermId == term && x.Type == ConstantHelpers.AcademicHistory.Types.CONVALIDATION && x.Validated)
                  .AsNoTracking();

            if (faculty.HasValue && faculty != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == faculty);
            if (career.HasValue && career != Guid.Empty) query = query.Where(x => x.Student.CareerId == career);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(search.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    name = x.Student.User.FullName,
                    code = x.Student.User.UserName,
                    career = x.Student.Career.Name,
                    courseCode = x.Course.Code,
                    courseName = x.Course.Name,
                    grade = x.Grade > 0 ? x.Grade.ToString() : "-",
                    observation = string.IsNullOrEmpty(x.Observations) ? string.Empty : x.Observations,
                    term = x.Term.Name,
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<StudentHistoryTemplate>> GetCourseRecognitionReportData(Guid term, Guid? faculty = null, Guid? career = null, string search = null)
        {
            var query = _context.AcademicHistories
                  .Where(x => x.TermId == term && x.Type == ConstantHelpers.AcademicHistory.Types.CONVALIDATION && x.Validated)
                  .AsNoTracking();

            if (faculty.HasValue && faculty != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == faculty);
            if (career.HasValue && career != Guid.Empty) query = query.Where(x => x.Student.CareerId == career);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(search.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new StudentHistoryTemplate
                {
                    FullName = x.Student.User.FullName,
                    UserName = x.Student.User.UserName,
                    Career = x.Student.Career.Name,
                    CourseCode = x.Course.Code,
                    CourseName = x.Course.Name,
                    Grade = x.Grade,
                    Observations = string.IsNullOrEmpty(x.Observations) ? string.Empty : x.Observations,
                    Term = x.Term.Name,
                }).ToListAsync();

            return data;
        }

        public async Task<object> GetDisapprovedCoursesByStudentDataDatatable(Guid termid, Guid? facultyId = null, Guid? careerId = null, int? disapprovedCourses = null)
        {
            var query = _context.AcademicHistories
                .Where(x => x.TermId == termid
                && x.Type == ConstantHelpers.AcademicHistory.Types.REGULAR
                && !x.Validated && !x.Approved && x.SectionId != null)
                .AsNoTracking();

            if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == facultyId);
            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Student.CareerId == careerId);

            var data = await query
                .Select(x => new
                {
                    studentId = x.StudentId,
                    termId = x.TermId,
                    username = x.Student.User.UserName,
                    fullname = x.Student.User.FullName,
                    faculty = x.Student.Career.Faculty.Name,
                    career = x.Student.Career.Name,
                    term = x.Term.Name,
                })
                .ToListAsync();

            var result = data
                .GroupBy(x => new { x.studentId, x.termId, x.username, x.fullname, x.faculty, x.career, x.term })
                .Select(x => new
                {
                    x.Key.termId,
                    x.Key.studentId,
                    x.Key.term,
                    x.Key.career,
                    x.Key.faculty,
                    x.Key.username,
                    x.Key.fullname,
                    count = x.Count()
                })
                .ToList();

            if (disapprovedCourses.HasValue && disapprovedCourses > 0)
                result = result.Where(x => x.count == disapprovedCourses).ToList();

            return result;
        }

        public async Task<List<DisapprovedCoursesByStudentTemplate>> GetDisapprovedCoursesByStudentData(Guid termid, Guid? facultyId = null, Guid? careerId = null, int? disapprovedCourses = null)
        {
            var query = _context.AcademicHistories
                .Where(x => x.TermId == termid && x.SectionId.HasValue && x.Type == ConstantHelpers.AcademicHistory.Types.REGULAR && !x.Validated && !x.Approved)
                .AsNoTracking();

            if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == facultyId);
            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Student.CareerId == careerId);

            var data = await query
                .Select(x => new
                {
                    studentId = x.StudentId,
                    termId = x.TermId,
                    username = x.Student.User.UserName,
                    fullname = x.Student.User.FullName,
                    faculty = x.Student.Career.Faculty.Name,
                    career = x.Student.Career.Name,
                    term = x.Term.Name,
                })
                .ToListAsync();

            var result = data
                .GroupBy(x => new { x.studentId, x.termId, x.username, x.fullname, x.faculty, x.career, x.term })
                .Select(x => new DisapprovedCoursesByStudentTemplate
                {
                    Career = x.Key.career,
                    Term = x.Key.term,
                    Faculty = x.Key.faculty,
                    UserName = x.Key.username,
                    Name = x.Key.fullname,
                    DisapprovedCourses = x.Count()
                })
                .ToList();

            if (disapprovedCourses.HasValue && disapprovedCourses > 0)
                result = result.Where(x => x.DisapprovedCourses == disapprovedCourses).ToList();

            return result;
        }



        public async Task<List<CourseEquivalenceTemplate>> GetCourseEquivalenceDataByStudent(Guid studentId, Guid? termId = null)
        {
            var student = await _context.Students
                .Where(x => x.Id == studentId)
                .Select(x => new
                {
                    x.User.FullName,
                    x.CurriculumId
                })
                .FirstOrDefaultAsync();

            var curriculumId = student.CurriculumId;
            if (termId.HasValue && termId != Guid.Empty)
            {
                var summary = await _context.AcademicSummaries
                    .Where(x => x.StudentId == studentId && x.TermId == termId
                    && x.CurriculumId.HasValue)
                    .Select(x => new
                    {
                        CurriculumId = x.CurriculumId.Value
                    })
                    .FirstOrDefaultAsync();
                if (summary != null)
                    curriculumId = summary.CurriculumId;
            }

            var equivalences = await _context.CourseEquivalences
                .Where(x => x.NewAcademicYearCourse.CurriculumId == curriculumId)
                .Select(x => new
                {
                    courseId = x.NewAcademicYearCourse.CourseId,
                    courseCode = x.NewAcademicYearCourse.Course.Code,
                    courseName = x.NewAcademicYearCourse.Course.Name,
                    oldCourseId = x.OldAcademicYearCourse.CourseId,
                    oldCourseCode = x.OldAcademicYearCourse.Course.Code,
                    oldCourseName = x.OldAcademicYearCourse.Course.Name
                })
                .ToListAsync();

            //var academicHistories = await _context.AcademicHistories
            //    .Where(x => x.StudentId == studentId && x.Type == ConstantHelpers.AcademicHistory.Types.REGULAR
            //    && x.Validated && x.Observations.Contains("Convalidado"))
            //    .Select(x => new
            //    {
            //        x.CourseId,
            //        x.Grade,
            //        Term = x.Term.Name
            //    })
            //    .ToListAsync();

            var academicHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == studentId)
                .Select(x => new
                {
                    x.Validated,
                    x.CourseId,
                    x.Grade,
                    Term = x.Term.Name,
                    x.Term.Year,
                    x.Term.Number,
                    x.Approved,
                    x.Type,
                    Date = x.EvaluationReportId.HasValue && x.EvaluationReport.ReceptionDate.HasValue ? x.EvaluationReport.ReceptionDate : x.CreatedAt
                })
                .ToListAsync();

            var data = equivalences
                .Where(x => academicHistories.Any(y => y.CourseId == x.courseId && y.Type == ConstantHelpers.AcademicHistory.Types.REGULAR && y.Validated)
                && academicHistories.Any(y => y.CourseId == x.oldCourseId))
                .OrderBy(x => x.courseCode)
                .ThenBy(x => x.oldCourseCode)
                .Select(x => new CourseEquivalenceTemplate
                {
                    CourseCode = x.courseCode,
                    CourseName = x.courseName,
                    Grade = academicHistories.Where(y => y.CourseId == x.oldCourseId)
                    .OrderByDescending(y => y.Approved)
                    .ThenByDescending(y => y.Year)
                    .ThenByDescending(y => y.Number)
                    .Select(y => y.Grade)
                    .FirstOrDefault(),
                    OldCourseCode = x.oldCourseCode,
                    OldCourseName = x.oldCourseName,
                    Term = academicHistories.Where(y => y.CourseId == x.oldCourseId)
                    .OrderByDescending(y => y.Approved)
                    .ThenByDescending(y => y.Year)
                    .ThenByDescending(y => y.Number)
                    .Select(y => y.Term)
                    .FirstOrDefault(),
                    Type = academicHistories.Where(y => y.CourseId == x.oldCourseId)
                    .OrderByDescending(y => y.Approved)
                    .ThenByDescending(y => y.Year)
                    .ThenByDescending(y => y.Number)
                    .Select(y => ConstantHelpers.AcademicHistory.Types.VALUES[y.Type])
                    .FirstOrDefault(),
                    Year = academicHistories.Where(y => y.CourseId == x.oldCourseId)
                    .OrderByDescending(y => y.Approved)
                    .ThenByDescending(y => y.Year)
                    .ThenByDescending(y => y.Number)
                    .Select(y => y.Year.ToString())
                    .FirstOrDefault(),
                    Date = academicHistories.Where(y => y.CourseId == x.oldCourseId)
                    .OrderByDescending(y => y.Approved)
                    .ThenByDescending(y => y.Year)
                    .ThenByDescending(y => y.Number)
                    .Select(y => y.Date.HasValue ? y.Date.Value.ToLocalDateFormat() : "")
                    .FirstOrDefault(),
                })
                .ToList();

            return data;
        }
        #endregion
    }
}
