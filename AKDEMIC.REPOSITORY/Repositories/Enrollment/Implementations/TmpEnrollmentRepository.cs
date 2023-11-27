using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class TmpEnrollmentRepository : Repository<TmpEnrollment>, ITmpEnrollmentRepository
    {
        private readonly IEnrollmentTurnRepository _enrollmentTurnRepository;
        public TmpEnrollmentRepository(AkdemicContext context,
            IEnrollmentTurnRepository enrollmentTurnRepository) : base(context)
        {
            _enrollmentTurnRepository = enrollmentTurnRepository;
        }

        public async Task<object> GetStudentDataDatatableClientSide(Guid studentId, Guid termId)
        {
            var student = await _context.Students.FindAsync(studentId);

            var courses = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId)
                .Select(x => new
                {
                    x.CourseId,
                    x.AcademicYear
                })
                .ToListAsync();

            var query = _context.TmpEnrollments
               .Where(ss => ss.StudentId == studentId
               && ss.Section.CourseTerm.TermId == termId)
                .AsNoTracking();

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    //year = courses.Any(y => y.CourseId == x.Section.CourseTerm.CourseId) ? courses.Where(y => y.CourseId == x.Section.CourseTerm.CourseId).Select(y => y.AcademicYear).First() : 99,
                    //year = x.Section.CourseTerm.Course.AcademicYearCourses.FirstOrDefault().AcademicYear.Number,

                    code = x.Section.CourseTerm.Course.Code,
                    courseId = x.Section.CourseTerm.CourseId,
                    course = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    vacancies = x.Section.Vacancies,
                    studentSections = x.Section.StudentSections.Count,
                    credits = x.Section.CourseTerm.Course.Credits
                })
                .ToListAsync();

            var result = data.Select(x => new
            {
                x.id,
                year = courses.Any(y => y.CourseId == x.courseId) ? courses.Where(y => y.CourseId == x.courseId).Select(y => y.AcademicYear).FirstOrDefault() : 99,
                //year = x.Section.CourseTerm.Course.AcademicYearCourses.FirstOrDefault().AcademicYear.Number,
                x.code,
                x.course,
                x.section,
                //x.vacancies,
                vacancies = x.vacancies - x.studentSections <= 0 ? 0 : x.vacancies - x.studentSections,
                x.credits,
                x.courseId
            }).OrderBy(x => x.year).ThenBy(x => x.code).ToList();

            return result;
        }

        public async Task<object> GetStudentDataFullCalendar(Guid studentId, Guid termId)
        {
            var tmpEnrollments = await _context.TmpEnrollments
             .Where(x => x.Section.CourseTerm.TermId == termId && x.StudentId == studentId)
             .Select(x => x.Section.ClassSchedules
                 .Where(y => !y.SectionGroupId.HasValue || y.SectionGroupId == x.SectionGroupId)
                 .Select(y => new
                 {
                     title = x.Section.CourseTerm.Course.Code + " - " + x.Section.Code,
                     description = y.Classroom.Description,
                     allDay = false,
                     start = y.StartTime.ToLocalDateTimeUtc().ToString("HH:mm"),
                     end = y.EndTime.ToLocalDateTimeUtc().ToString("HH:mm"),
                     dow = new[] { y.WeekDay + 1 }
                 }).ToList()
             )
             .ToListAsync();

            var result = tmpEnrollments.SelectMany(x => x).ToList();

            //var tmpEnrollments = _context.TmpEnrollments
            //    .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId)
            //    .Select(x => x.SectionId)
            //    .ToHashSet();

            //var result = await _context.ClassSchedules
            //  .Where(cs => cs.Section.CourseTerm.TermId == termId
            //  && tmpEnrollments.Contains(cs.SectionId))
            //  .Select(cs => new
            //  {
            //      title = cs.Section.CourseTerm.Course.Code + " - " + cs.Section.Code,
            //      description = cs.Classroom.Description,
            //      allDay = false,
            //      start = cs.StartTime.ToLocalDateTimeUtc().ToString("HH:mm"),
            //      end = cs.EndTime.ToLocalDateTimeUtc().ToString("HH:mm"),
            //      dow = new[] { cs.WeekDay + 1 }
            //  })
            //  .ToListAsync();

            return result;
        }

        public async Task RemoveActiveSections(Guid courseTermId, Guid studentId)
        {
            var courseTerm = await _context.CourseTerms.FindAsync(courseTermId);
            var activeSections = await _context.TmpEnrollments.Where(ss => ss.Section.CourseTerm.CourseId == courseTerm.CourseId
            && ss.Section.CourseTerm.TermId == courseTerm.TermId && ss.StudentId == studentId).ToListAsync();
            //var activeSections = await _context.TmpEnrollments.Where(ss => ss.Section.CourseTermId == courseTermId && ss.StudentId == studentId).ToListAsync();
            if (activeSections.Count != 0) _context.TmpEnrollments.RemoveRange(activeSections);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateIntersection(Guid studentId, Guid termId, Guid courseTermId, List<ClassSchedule> classSchedules)
        {
            var intersection = await _context.TmpEnrollments
                          .Where(ss => ss.StudentId == studentId
                          && ss.Section.CourseTerm.TermId == termId
                          && ss.Section.CourseTermId != courseTermId)
                          .AnyAsync(ss => ss.Section.ClassSchedules
                              .Any(cs => classSchedules
                                  .Any(scs => scs.WeekDay == cs.WeekDay && ((scs.StartTime <= cs.StartTime && cs.StartTime < scs.EndTime) || (scs.StartTime < cs.EndTime && cs.EndTime <= scs.EndTime) || (cs.StartTime <= scs.StartTime && scs.StartTime < cs.EndTime) || (cs.StartTime < scs.EndTime && scs.EndTime <= cs.EndTime)))
                              )
                          );
            return intersection;
        }

        public async Task<object> GetAllWithData(string userId, Guid termId)
        {
            var data = await _context.TmpEnrollments
            .Where(ss => ss.Student.UserId == userId && ss.Section.CourseTerm.TermId == termId)
            .Select(ss => new
            {
                id = ss.Id,
                code = ss.Section.CourseTerm.Course.Code,
                course = ss.Section.CourseTerm.Course.Name,
                section = ss.Section.Code,
                vacancies = ss.Section.Vacancies,
                students = ss.Section.StudentSections.Count,
                credits = ss.Section.CourseTerm.Course.Credits
            }).ToListAsync();

            var result = data
                .Select(x => new
                {
                    x.id,
                    x.code,
                    x.course,
                    x.section,
                    vacancies = x.vacancies - x.students <= 0 ? 0 : x.vacancies - x.students,
                    x.credits
                }).ToList();

            return result;
        }

        public async Task<decimal> GetUserCredits(string userId, int status, Guid courseId)
        {
            var usedCredits = await _context.TmpEnrollments
                .Where(tmp => tmp.Student.UserId == userId &&
                              tmp.Section.CourseTerm.Term.Status ==
                              status &&
                              tmp.Section.CourseTermId != courseId)
                .SumAsync(tmp => tmp.Section.CourseTerm.Course.Credits);

            return usedCredits;
        }

        public async Task<bool> GetIntersection(ICollection<ClassSchedule> classSchedules, Guid studentId, int status, Guid courseId)
        {
            var tmpEnrollmentSchedules = await _context.TmpEnrollments
                .Where(ss => ss.StudentId == studentId && ss.Section.CourseTerm.Term.Status == status && ss.Section.CourseTermId != courseId)
                .Select(ss => ss.Section.ClassSchedules)
                .ToListAsync();

            var intersection = tmpEnrollmentSchedules
                .Any(ss => ss
                    .Any(cs => classSchedules
                        .Any(scs => scs.WeekDay == cs.WeekDay && ((scs.StartTime <= cs.StartTime && cs.StartTime < scs.EndTime) || (scs.StartTime < cs.EndTime && cs.EndTime <= scs.EndTime) || (cs.StartTime <= scs.StartTime && scs.StartTime < cs.EndTime) || (cs.StartTime < scs.EndTime && scs.EndTime <= cs.EndTime)))
                    )
                );

            return intersection;
        }

        public async Task<List<TmpEnrollment>> GetActiveSections(Guid courseTermId)
        {
            var activeSections = await _context.TmpEnrollments.Where(ss => ss.Section.CourseTermId == courseTermId).ToListAsync();

            return activeSections;
        }

        public async Task<TmpEnrollment> GetStudentSection(Guid id, string userId)
        {
            var studentSection = await _context.TmpEnrollments.Include(x => x.Section.CourseTerm)
                .Where(ss => ss.Id == id && ss.Student.UserId == userId).FirstOrDefaultAsync();

            return studentSection;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPendingRectificactionsDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
        {

            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                default:
                    break;
            }
            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
            if (term == null)
                term = new Term();
            var tmpEnrollments = _context.TmpEnrollments
                .IgnoreQueryFilters()
                .Where(te => te.Section.CourseTerm.TermId == term.Id
                && te.IsAdminRectification && !te.WasApplied)
                .Select(x => x.StudentId).ToHashSet();

            var query = _context.Students
                .Where(x => tmpEnrollments.Contains(x.Id))
                .AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name
                })
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<bool> HasPendingRectificationChanges(Guid studentId)
        {
            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();

            var result = await _context.TmpEnrollments
                .IgnoreQueryFilters()
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == term.Id)
                .AnyAsync(x => x.IsAdminRectification && !x.WasApplied);

            return result;
        }

        public async Task<bool> ValidateParallelCoursesLimit(Guid studentId, Guid courseTermId)
        {
            var firstParallelCourseAcademicYear = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.FIRST_PARALLEL_COURSE_ACADEMIC_YEAR);
            var secondParallelCourseAcademicYear = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.SECOND_PARALLEL_COURSE_ACADEMIC_YEAR);

            var student = await _context.Students.FindAsync(studentId);
            var courseTerm = await _context.CourseTerms.Where(x => x.Id == courseTermId).FirstOrDefaultAsync();

            var academicYearCourse = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId && x.CourseId == courseTerm.CourseId)
                .Include(x => x.PreRequisites)
                .FirstOrDefaultAsync();

            if (academicYearCourse.AcademicYear != firstParallelCourseAcademicYear && academicYearCourse.AcademicYear != secondParallelCourseAcademicYear)
            {
                return true;
            }

            var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == studentId).Select(x => new { x.CourseId, x.Approved }).ToArrayAsync();
            var academicHistoriesHash = academicHistories.Select(x => x.CourseId).ToHashSet();

            var isParallelCourse = academicYearCourse.PreRequisites.Any(x => !academicHistoriesHash.Contains(x.CourseId));

            if (!isParallelCourse)
            {
                return true;
            }

            var tmpParallelCoursesCount = await _context.TmpEnrollments
                .Where(x => x.StudentId == studentId
                && x.Section.CourseTerm.TermId == courseTerm.TermId && x.IsParallelCourse
                && x.Section.CourseTermId != courseTerm.CourseId)
                .CountAsync();

            if (academicYearCourse.AcademicYear == firstParallelCourseAcademicYear)
            {
                var firstParallelCourseQuantity = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.FIRST_PARALLEL_COURSE_QUANTITY);

                if (tmpParallelCoursesCount + 1 <= firstParallelCourseQuantity)
                {
                    return true;
                }
            }

            if (academicYearCourse.AcademicYear == secondParallelCourseAcademicYear)
            {
                var secondParallelCourseQuantity = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.SECOND_PARALLEL_COURSE_QUANTITY);

                if (tmpParallelCoursesCount + 1 <= secondParallelCourseQuantity)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> IsParallelCourse(Guid studentId, Guid courseTermId)
        {
            var student = await _context.Students.FindAsync(studentId);
            var courseTerm = await _context.CourseTerms.Where(x => x.Id == courseTermId).FirstOrDefaultAsync();

            var academicYearCourse = await _context.AcademicYearCourses
               .Where(x => x.CurriculumId == student.CurriculumId && x.CourseId == courseTerm.CourseId)
               .Include(x => x.PreRequisites)
               .FirstOrDefaultAsync();

            var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == studentId).Select(x => new { x.CourseId, x.Approved }).ToArrayAsync();
            var academicHistoriesHash = academicHistories.Select(x => x.CourseId).ToHashSet();

            var isParallelCourse = academicYearCourse.PreRequisites.Any(x => !academicHistoriesHash.Contains(x.CourseId));

            if (!isParallelCourse)
            {
                return true;
            }

            return false;
        }

        public async Task<Tuple<bool, string>> InsertWithValidations(TmpEnrollment tmpEnrollment)
        {
            if (await _context.TmpEnrollments.AnyAsync(x => x.StudentId == tmpEnrollment.StudentId && x.SectionId == tmpEnrollment.SectionId && x.SectionGroupId == tmpEnrollment.SectionGroupId))
                return new Tuple<bool, string>(true, "");

            var student = await _context.Students
                .Where(x => x.Id == tmpEnrollment.StudentId)
                .Include(x => x.Curriculum)
                .Include(x => x.Career)
                .FirstOrDefaultAsync();

            var section = await _context.Sections
                .Where(x => x.Id == tmpEnrollment.SectionId)
                .Select(x => new
                {
                    x.CourseTerm.Course.Credits,
                    x.CourseTerm.CourseId,
                    x.CourseTerm.TermId,
                    x.Vacancies,
                    StudentSections = x.StudentSections.Count,
                    ClassSchedules = x.ClassSchedules.ToList()
                }).FirstOrDefaultAsync();

            if (await _context.TmpEnrollments.AnyAsync(x => x.StudentId == tmpEnrollment.StudentId && x.SectionId == tmpEnrollment.SectionId))
                return new Tuple<bool, string>(false, "Ya se encuentra matriculado en el curso seleccionado");

            var turn = await _context.EnrollmentTurns.Where(et => et.StudentId == student.Id && et.TermId == section.TermId).FirstOrDefaultAsync();

            var credits = 0M;
            if (turn != null) credits = turn.CreditsLimit;
            else credits = await _enrollmentTurnRepository.GetStudentCreditsWithoutTurn(tmpEnrollment.StudentId);

            if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNICA)
            {
                var usedCredits = await _context.TmpEnrollments
                .Where(tmp => tmp.StudentId == student.Id &&
                              tmp.Section.CourseTerm.TermId == section.TermId &&
                              tmp.Section.CourseTerm.CourseId != section.CourseId)
                .SumAsync(tmp => tmp.Section.CourseTerm.Course.Credits);

                if (usedCredits + section.Credits > credits) return new Tuple<bool, string>(false, "Excede el máximo de créditos disponibles");
            }

            //var tmpClassSchedules = await _context.TmpEnrollments
            //              .Where(ss => ss.StudentId == student.Id                          
            //              && ss.Section.CourseTerm.TermId == section.TermId
            //              && ss.Section.CourseTerm.CourseId != section.CourseId)
            //              .Select(x => x.Section.ClassSchedules)
            //              .ToListAsync();

            //var intersection = tmpClassSchedules
            //              .Any(ss => ss
            //                  .Any(cs => section.ClassSchedules
            //                      .Any(scs => scs.WeekDay == cs.WeekDay && ((scs.StartTime <= cs.StartTime && cs.StartTime < scs.EndTime) || (scs.StartTime < cs.EndTime && cs.EndTime <= scs.EndTime) || (cs.StartTime <= scs.StartTime && scs.StartTime < cs.EndTime) || (cs.StartTime < scs.EndTime && scs.EndTime <= cs.EndTime)))
            //                  ));

            //if (intersection && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAMAD) return new Tuple<bool, string>(false, "Presenta un cruce con otro curso");

            //VALIDA CURSO PARALELO

            #region Validar paralelos
            var academicYearCourse = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId && x.CourseId == section.CourseId)
                .Include(x => x.PreRequisites).ThenInclude(x => x.Course)
                .Include(x => x.Course)
                .FirstOrDefaultAsync();

            var academicHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == student.Id && x.Approved)
                .Select(x => x.CourseId)
                .ToListAsync();

            if (await _context.Terms.Where(x => x.Id == section.TermId).AnyAsync(x => x.Status == ConstantHelpers.TERM_STATES.INACTIVE))
            {
                var studentSections = await _context.StudentSections
                    .Where(x => x.StudentId == tmpEnrollment.StudentId && x.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                    .Select(x => x.Section.CourseTerm.CourseId)
                    .ToListAsync();

                academicHistories.AddRange(studentSections);
            }

            var academicHistoriesHash = academicHistories.ToHashSet();
            var isParallelCourse = academicYearCourse.PreRequisites.Where(x => !x.IsOptional).Any(x => !academicHistoriesHash.Contains(x.CourseId))
                || (academicYearCourse.PreRequisites.Where(x => x.IsOptional).Any() && academicYearCourse.PreRequisites.Where(x => x.IsOptional).All(x => !academicHistoriesHash.Contains(x.CourseId)));

            if (isParallelCourse)
            {
                var parallelCoursesLimit = await _context.CareerParallelCourses
                    .Where(x => x.CareerId == student.CareerId && x.AcademicYear == student.CurrentAcademicYear)
                    .FirstOrDefaultAsync();

                if (parallelCoursesLimit == null)
                    return new Tuple<bool, string>(false, "No puede llevar cursos paralelos");

                var tmpParallelCoursesCount = await _context.TmpEnrollments
                    .Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == section.TermId && x.IsParallelCourse
                    && x.Section.CourseTerm.CourseId != section.CourseId)
                    .CountAsync();

                if (tmpParallelCoursesCount + 1 > parallelCoursesLimit.Quantity)
                    return new Tuple<bool, string>(false, "Excede el máximo de cursos paralelos permitidos para su matrícula");

                tmpEnrollment.IsParallelCourse = true;
            }
            #endregion

            var activeSections = await _context.TmpEnrollments
                .Where(ss => ss.Section.CourseTerm.CourseId == section.CourseId
                && ss.Section.CourseTerm.TermId == section.TermId && ss.StudentId == student.Id)
                .ToListAsync();
            if (activeSections.Count != 0)
                _context.TmpEnrollments.RemoveRange(activeSections);

            var vacancies = section.Vacancies - section.StudentSections;
            if (vacancies <= 0) return new Tuple<bool, string>(false, "La sección no presenta vacantes disponibles");

            var exist = await _context.TmpEnrollments.FindAsync(tmpEnrollment.Id);
            if (exist == null)
                await _context.TmpEnrollments.AddAsync(tmpEnrollment);

            await _context.SaveChangesAsync();

            return new Tuple<bool, string>(true, "");
        }
        public async Task<List<Guid>> GetCoursesIdByStudentAndTermId(Guid studentId, Guid termId)
        {
            return await _context.TmpEnrollments
                .Where(x => x.StudentId == studentId
                && x.Section.CourseTerm.TermId == termId)
                .Select(x => x.Section.CourseTerm.CourseId)
                .ToListAsync();
        }
        public async Task<List<TmpEnrollment>> GetAllByStudentAndTermId(Guid studentId, Guid termId)
        {
            var model = await _context.TmpEnrollments
                 .Include(x => x.Section).ThenInclude(x => x.CourseTerm).ThenInclude(x => x.Course).ThenInclude(x => x.AcademicYearCourses)
                 .Include(x => x.Section).ThenInclude(x => x.CourseTerm).ThenInclude(x => x.Term)
                 .Include(x => x.Section).ThenInclude(x => x.CourseTerm).ThenInclude(x => x.Course)
                 .Include(x => x.Section).ThenInclude(x => x.StudentSections)
                 .Where(ss => ss.Section.CourseTerm.TermId == termId && ss.StudentId == studentId).ToListAsync();

            return model;
        }
        public async Task<int> GetUserCredits(Guid studentId, Guid curriculumId, Guid courseTermId, bool electives)
        {
            var academicYearCourses = await _context.AcademicYearCourses
                  .Include(x => x.Course)
                  .ThenInclude(x => x.AcademicProgram)
                  .Include(x => x.PreRequisites)
                  .Where(x => x.CurriculumId == curriculumId)
                  .ToListAsync();
            if (electives)
                return await _context.TmpEnrollments
                       .Where(tmp => tmp.StudentId == studentId &&
                                     tmp.Section.CourseTerm.Term.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.ACTIVE &&
                                     tmp.Section.CourseTermId != courseTermId &&
                                     academicYearCourses.FirstOrDefault(ayc => ayc.CourseId == tmp.Section.CourseTerm.CourseId).IsElective) /////////////////
                       .CountAsync();

            return await _context.TmpEnrollments
            .Where(tmp => tmp.StudentId == studentId &&
                          tmp.Section.CourseTerm.Term.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.ACTIVE &&
                          tmp.Section.CourseTermId != courseTermId &&
                          !academicYearCourses.FirstOrDefault(ayc => ayc.CourseId == tmp.Section.CourseTerm.CourseId).IsElective) /////////////////
            .CountAsync();
        }
        public async Task<int> ParallelCoursesCount(Guid studentId, Guid termId, Guid courseTermId)
        {
            return await _context.TmpEnrollments
                       .Where(x => x.StudentId == studentId
                       && x.Section.CourseTerm.TermId == termId && x.IsParallelCourse
                       && x.Section.CourseTermId != courseTermId)
                       .CountAsync();
        }

        public async Task<TmpEnrollment> GetByStudentAndSection(Guid studentId, Guid sectionId)
            => await _context.TmpEnrollments.Where(x => x.StudentId == studentId && x.SectionId == sectionId).FirstOrDefaultAsync();

        public async Task<Tuple<bool, string>> DeleteWithValidations(Guid tmpEnrollmentId, string userId = null)
        {
            var term = await _context.Terms.AsNoTracking().FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            if (term == null) return new Tuple<bool, string>(false, "Solo se puede hacer rectificación en periodo activo");

            var tmpEnrollment = await _context.TmpEnrollments.FindAsync(tmpEnrollmentId);
            if (tmpEnrollment == null) return new Tuple<bool, string>(false, "Matrícula no encontrada");

            var hasGrades = await _context.Grades
                .Where(x => x.StudentSection.StudentId == tmpEnrollment.StudentId 
                && x.StudentSection.SectionId == tmpEnrollment.SectionId/*&& x.StudentSection.Section.CourseTerm.TermId == term.Id*/)
                .AnyAsync();
            if (hasGrades)
                //return new Tuple<bool, string>(false, "No puede modificar el curso porque tiene notas relacionadas");
                return new Tuple<bool, string>(false, "No puede modificar la matrícula porque ya posee notas en el periodo");

            tmpEnrollment.IsAdminRectification = true;
            tmpEnrollment.WasApplied = false;

            if (!string.IsNullOrEmpty(userId))
            {
                var adminEnrollment = await _context.AdminEnrollments
                    .FirstOrDefaultAsync(x => !x.WasApplied && x.StudentId == tmpEnrollment.StudentId && x.UserId == userId && x.TermId == term.Id);

                var section = await _context.Sections
                    .Where(x => x.Id == tmpEnrollment.SectionId)
                    .Select(x => new {
                        Course = x.CourseTerm.Course.Name,
                        Code = x.CourseTerm.Course.Code
                    }).FirstOrDefaultAsync();

                var activityLog = $"Eliminar curso temporal: {section.Course} - {section.Code}; " + Environment.NewLine;

                if (adminEnrollment != null)
                {
                    adminEnrollment.IsRectification = true;
                    adminEnrollment.ActivityLog += activityLog;
                }
                else
                {
                    adminEnrollment = new AdminEnrollment
                    {
                        UserId = userId,
                        StudentId = tmpEnrollment.StudentId,
                        TermId = term.Id,
                        ActivityLog = activityLog,
                        IsRectification = true
                    };
                    await _context.AdminEnrollments.AddAsync(adminEnrollment);
                }
            }

            _context.TmpEnrollments.Remove(tmpEnrollment);
            await _context.SaveChangesAsync();
            return new Tuple<bool, string>(true, "");
        }
    }
}
