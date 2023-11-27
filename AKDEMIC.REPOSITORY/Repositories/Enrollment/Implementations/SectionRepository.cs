using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Section;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.SubstituteExam;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class SectionRepository : Repository<Section>, ISectionRepository
    {
        public SectionRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE 

        private async Task<DataTablesStructs.ReturnedData<Section>> GetSectionsByTermAndStudentIdDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<Section, Section>> selectPredicate = null, Expression<Func<Section, dynamic>> orderByPredicate = null,
            Guid? termId = null, Guid? studentId = null, string searchValue = null)
        {
            var query = _context.Sections
                .Include(x => x.CourseTerm).ThenInclude(x => x.Course)
                .ThenInclude(x => x.Career)
                .Where(x => x.StudentSections.Any(y => y.StudentId == studentId) && x.CourseTerm.TermId == termId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Code.ToUpper().Contains(searchValue.ToUpper()));
            }

            query = query.AsNoTracking();

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC

        public async Task<IEnumerable<Section>> GetByCourseTermId(Guid courseTermId)
            => await _context.Sections.Where(x => x.CourseTermId == courseTermId).ToListAsync();

        public async Task<Section> GetWithCourseTermAndCourse(Guid sectionId)
            => await _context.Sections.Where(x => x.Id == sectionId)
            .Include(x => x.CourseTerm.Course)
            .Include(x => x.CourseTerm.Term)
            .FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<Section>> GetSectionsByTermAndStudentIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? studentId = null, string searchValue = null)
        {
            Expression<Func<Section, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CourseTerm.Course.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.CourseTerm.Course.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.StudentSections.Count());
                    break;
                case "4":
                    orderByPredicate = ((x) => x.CourseTerm.Course.Credits);
                    break;
            }

            return await GetSectionsByTermAndStudentIdDatatable(sentParameters, (x) => new Section
            {
                Id = x.Id,
                CourseTerm = new CourseTerm
                {
                    Course = new Course
                    {
                        Code = x.CourseTerm.Course.Code,
                        Name = x.CourseTerm.Course.Name,
                        Credits = x.CourseTerm.Course.Credits,
                        CareerId = x.CourseTerm.Course.CareerId,
                        Career = new Career
                        {
                            Name = x.CourseTerm.Course.Career.Name
                        }
                    }
                },
                StudentSectionId = x.StudentSections.Where(y => y.SectionId == x.Id).FirstOrDefault().Id,
                Code = x.Code,
                StudentsCount = x.StudentSections.Count()
            }, orderByPredicate, termId, studentId, searchValue);

        }

        public async Task<Section> GetWithIncludes(Guid sectionId)
        {
            var query = _context.Sections
                .Include(x => x.CourseTerm)
                    .ThenInclude(x => x.Course)
                        .ThenInclude(x => x.Career)
                            .ThenInclude(x => x.Faculty)
                .Include(x => x.CourseTerm)
                    .ThenInclude(x => x.Term)
                 .Include(x => x.StudentSections)
                 .Include(x => x.CourseTerm.Course.AcademicProgram)
                .Where(x => x.Id == sectionId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Section>> GetAllByCourseAndTerm(Guid courseId, Guid termId, string teacherId = null)
        {

            var query = _context.Sections.Where(x => x.CourseTerm.CourseId == courseId && x.CourseTerm.TermId == termId).AsNoTracking();

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.TeacherSections.Any(y => y.TeacherId == teacherId));

            var result = await query
                   .Select(x => new Section
                   {
                       Id = x.Id,
                       Code = x.Code,
                       StudentsCount = x.StudentSections.Count,
                       IsDirectedCourse = x.IsDirectedCourse
                   })
                    .ToListAsync();

            return result;
        }

        public async Task<Tuple<string, int, string[]>> GetSpecificSectionDate(Guid sectionId)
        {
            var query = _context.Sections
                .Where(x => x.Id == sectionId)
                .AsQueryable();

            var result = await query.Select(x => new Tuple<string, int, string[]>(x.Code, x.StudentSections.Count, x.TeacherSections.Select(ts => ts.Teacher.User.FullName).ToArray())).FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByCourseAndTermAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId)
        {
            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == termId && x.CourseTerm.CourseId == courseId && !x.IsDirectedCourse)
                .AsQueryable();

            query = query.Select(x => new Section
            {
                Id = x.Id,
                Code = x.Code,
                StudentsCount = x.StudentSections.Count,
                TeacherNames = x.TeacherSections.Select(ts => ts.Teacher.User.FullName),
                TeacherSections = x.TeacherSections,
                CourseTerm = new CourseTerm
                {
                    CourseId = x.CourseTerm.CourseId,
                    TermId = x.CourseTerm.TermId
                }
            });

            return await query.ToDataTables<object>(sentParameters);
        }

        public async Task<int> CountByCourseAndTerm(Guid courseId, Guid termId)
        {
            return await _context.Sections
.Where(x => x.CourseTerm.CourseId == courseId && x.CourseTerm.TermId == termId).CountAsync();
        }

        public async Task<object> GetSectionsByCourseTermId(Guid courseTermId)
        {
            var courseTerm = await _context.CourseTerms.Where(x => x.Id == courseTermId).Select(x => new { x.CourseId, x.TermId }).FirstOrDefaultAsync();

            var activities = await _context.UnitActivities
                .Where(ua => ua.CourseUnit.CourseSyllabus.CourseId == courseTerm.CourseId && ua.CourseUnit.CourseSyllabus.TermId == courseTerm.TermId)
                .ToListAsync();

            var query = _context.Sections
            .Where(x => x.CourseTermId == courseTermId)
            .Select(x => new
            {
                x.Id,
                x.Code,
                x.CourseTermId,
                classes = _context.Classes.Where(y => y.SectionId == x.Id).Select(y => new
                {
                    y.UnitActivityId,
                    y.IsDictated,
                    y.DictatedDate
                }).ToList(),
                teachers = x.TeacherSections.Count() > 0 ? String.Join(";", x.TeacherSections.Select(y => y.Teacher.User.FullName).ToList()) : "--"
            }).ToList();

            var result = query
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.CourseTermId,
                    x.teachers,
                    progress = $"{((activities.Count(y => x.classes.Any(z => z.UnitActivityId == y.Id)) * 100f) / (activities.Any() ? activities.Count() : 1)).ToString()}%"
                }).ToList();

            return result;
        }

        public async Task<IEnumerable<Section>> GetAvailableSectionsByCourseTermId(Guid? termId, Guid? currentSectionId = null)
        {
            var query = _context.Sections.Where(x => x.CourseTermId == termId).AsQueryable();

            if (currentSectionId.HasValue)
                query = query.Where(x => x.Id != currentSectionId);

            var result = await query.ToArrayAsync();
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByCourseComponentTermFacultyAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid? component = null, Guid? termId = null, Guid? faculty = null, string search = null)
        {
            Expression<Func<Section, dynamic>> orderByPredicate = null;


            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CourseTerm.Course.FullName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.CourseTerm.Term.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.CourseTerm.Course.FullName;
                    break;
            }


            var query = _context.Sections
                .AsNoTracking();

            if (termId != Guid.Empty) query = query.Where(x => x.CourseTerm.TermId == termId);


            query = query
                .Where(x => x.CourseTerm.Course.CourseComponentId == component)
                .AsQueryable();

            var recordsFiltered = await query.CountAsync();

            var client = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(section => new
                {
                    section.Id,
                    section.CourseTermId,
                    section.CourseTerm.CourseId,
                    Term = section.CourseTerm.Term.Name,

                    Course = section.CourseTerm.Course.FullName,
                    section.Code,
                    GradeRegistrations = section.GradeRegistrations
                            .Select(x => new
                            {
                                x.EvaluationId,
                                x.WasLate
                            }),

                    Evaluations = section.CourseTerm
                            .Evaluations
                            //.Where(e => e.CourseUnitId.HasValue)
                            .Select(e => new
                            {
                                Unit = e.CourseUnit.Number,
                                EvaluationId = e.Id,
                                e.CourseUnitId
                            })
                })
                .ToListAsync();

            var sections = client
                        .Select(section => new
                        {
                            section.Id,
                            section.CourseTermId,
                            section.CourseId,
                            Term = section.Term,
                            Course = section.Course,
                            section.Code,
                            GradeRegistrations = section.GradeRegistrations
                            .Select(x => new
                            {
                                x.EvaluationId,
                                x.WasLate
                            }),

                            Evaluations = section.Evaluations
                                .Where(e => e.CourseUnitId.HasValue)
                                .Select(e => new
                                {
                                    Unit = e.Unit,
                                    EvaluationId = e.EvaluationId
                                })
                        })
                        .ToList();


            var processedSections = sections
                .Select(section => new
                {
                    term = section.Term,
                    course = section.Course,
                    section = section.Code,
                    units = section.Evaluations
                   .GroupBy(evaluation => evaluation.Unit)
                       .Select(evaluation => new
                       {
                           complete = evaluation.All(e => section.GradeRegistrations.Any(g => g.EvaluationId == e.EvaluationId)),
                           waslate = evaluation.Any(e => section.GradeRegistrations.Any(g => g.EvaluationId == e.EvaluationId && g.WasLate))
                       }
                       )
                   .ToList()
                }).ToList();

            var recordsTotal = processedSections.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = processedSections,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<List<ProcessedSectionsTemplate>> GetGradeRegistrationReport(Guid component, Guid termId, Guid faculty)
        {

            var query = _context.Sections
                .AsNoTracking();

            if (termId != Guid.Empty) query = query.Where(x => x.CourseTerm.TermId == termId);

            var recordsFiltered = await query.CountAsync();

            query = query
                .Where(x => x.CourseTerm.Course.CourseComponentId == component)
                .AsQueryable();

            //if (!string.IsNullOrEmpty(searchValue))
            //    query = query.Where(q =>
            //        q.Section.CourseTerm.Course.FullName.Contains(searchValue) ||
            //        q.Section.Code.Contains(searchValue) ||
            //        q.Section.TeacherSections.Select(t => t.Teacher.User.FullName).Contains(searchValue) ||
            //        q.LastReportGeneratedDate.ToString().Contains(searchValue) ||
            //        q.Status.ToString().Contains(searchValue));

            var sections = await query
             .Select(section => new
             {
                 section.Id,
                 section.CourseTermId,
                 section.CourseTerm.CourseId,
                 Term = section.CourseTerm.Term.Name,


                 Course = section.CourseTerm.Course.FullName,
                 section.Code,
                 GradeRegistrations = section.GradeRegistrations
                 .Select(x => new
                 {
                     x.EvaluationId,
                     x.WasLate
                 }).ToList(),

                 Evaluations = section.CourseTerm
                 .Evaluations
                 .Where(e => e.CourseUnitId.HasValue)
                 .Select(e => new
                 {
                     Unit = e.CourseUnit.Number,
                     EvaluationId = e.Id
                 }).ToList()
             })
             //.OrderByDescending(section => section.Course)
             .ToListAsync();


            var processedSections = sections
                .Select(section => new ProcessedSectionsTemplate
                {
                    Term = section.Term,
                    Course = section.Course,
                    Section = section.Code,
                    Units = section.Evaluations
                   .GroupBy(evaluation => evaluation.Unit)
                       .Select(evaluation => new EvalutaionReportTemplate
                       {
                           Waslate = evaluation.Any(e => section.GradeRegistrations.Any(g => g.EvaluationId == e.EvaluationId && g.WasLate))
                       }
                       )
                   .ToList()
                }).OrderByDescending(section => section.Course).ToList();

            var recordsTotal = processedSections.Count;

            return processedSections;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByCourseTermLatFilterAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid term, int lateFilter, string search)
        {
            Expression<Func<Section, dynamic>> orderByPredicate = null;


            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CourseTerm.Course.FullName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.CourseTerm.Term.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.CourseTerm.Course.FullName;
                    break;
            }


            var query = _context.Sections
                .AsNoTracking();

            var courseComponent = await _context.CourseComponents.FirstOrDefaultAsync();

            var recordsFiltered = await query.CountAsync();

            var courseUnits = await _context.CourseUnits
                .IgnoreQueryFilters()
                .Select(x => new
                {
                    x.Id,
                    x.Number
                })
                .ToArrayAsync();

            var sections = await query
                .IgnoreQueryFilters()
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
               .Select(section => new
               {
                   section.Id,
                   section.CourseTermId,
                   section.CourseTerm.CourseId,
                   Term = section.CourseTerm.Term.Name,
                   Course = section.CourseTerm.Course.FullName,
                   section.Code,
                   Grades = section.GradeRegistrations.ToList(),

                   GradeRegistrations = section.GradeRegistrations
                   .Select(x => x.EvaluationId).ToHashSet(),

                   CourseUnits = section.CourseTerm.Evaluations
                   .Where(evaluation => evaluation.CourseUnitId.HasValue)
                   .GroupBy(evaluation => evaluation.CourseUnit.Number)
                   .Select(evaluation => new
                   {
                       CourseUnitNumber = evaluation.Key,
                       IsCompleted = false,
                       Evaluations = evaluation.Select(e => e.Id).ToArray()
                   })
                   .ToList()
               }).ToListAsync();

            var processedSections = sections
                .Select(section => new
                {
                    term = section.Term,
                    course = section.Course,
                    section = section.Code,
                    units = section.CourseUnits
                    .Select(c => new
                    {
                        number = c.CourseUnitNumber,

                        isCompleted = c.Evaluations.All(e => section.GradeRegistrations.Contains(e)),

                        totalEvaluations = c.Evaluations.Count(),
                        registeredEvaluations = c.Evaluations.Where(e => section.GradeRegistrations.Contains(e)).Count(),

                        wasLate = c.Evaluations.Any(e => section.Grades.Any(g => g.EvaluationId == e && g.WasLate))
                    }).OrderBy(c => c.number).ToList()
                }).ToList();


            for (int i = 0; i < courseComponent.QuantityOfUnits; i++)
            {
                var column = new
                {
                    title = $"U{i + 1}",
                    data = $"units[{i + 1}].isCompleted"
                };
            }

            var recordsTotal = processedSections.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = processedSections,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IEnumerable<Section>> GetAll(string teacherId = null, Guid? studentId = null, Guid? termId = null, Guid? courseId = null, Guid? courseTermId = null, bool showDirectedCourses = false)
        {
            var query = _context.Sections
                .AsNoTracking();

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.TeacherSections.Any(ts => ts.TeacherId == teacherId));

            if (studentId.HasValue)
                query = query.Where(x => x.StudentSections.Any(ss => ss.StudentId == studentId.Value));

            if (termId.HasValue)
                query = query.Where(x => x.CourseTerm.TermId == termId.Value);

            if (courseId.HasValue)
                query = query.Where(x => x.CourseTerm.CourseId == courseId.Value);

            if (courseTermId.HasValue)
                query = query.Where(x => x.CourseTermId == courseTermId.Value);

            if (!showDirectedCourses) query = query.Where(x => !x.IsDirectedCourse);

            var sections = await query
                .Include(x => x.CourseTerm.Term)
                .Include(x => x.CourseTerm.Course)
                .ThenInclude(x => x.Career)
                //.Include(x => x.ClassSchedules)
                .Include(x => x.TeacherSections)
                //.Include(x => x.StudentSections)
                //.Include(x => x.TmpEnrollments)
                .ToListAsync();

            return sections;
        }

        public async Task<Section> GetWithTeacherSections(Guid id)
        {
            return await _context.Sections
                       .Include(x => x.CourseTerm.Course.Career.Faculty)
                       .Include(x => x.CourseTerm.Term)
                       .Include(x => x.ClassSchedules)
                            .ThenInclude(x => x.TeacherSchedules)
                       .Include(x => x.TeacherSections)
                            .ThenInclude(x => x.Teacher.User)
                       .Where(x => x.Id == id)
                       .FirstOrDefaultAsync();
        }

        public async Task<Section> GetWithEvaluations(Guid id)
        {
            return await _context.Sections
                       .Include(x => x.CourseTerm.Course)
                       .Include(x => x.CourseTerm.Term)
                       .Include(x => x.CourseTerm.Evaluations)
                       .Where(x => x.Id == id)
                       .FirstOrDefaultAsync();
        }

        public async Task<Section> GetWithCourseTermCareer(Guid id)
        {
            return await _context.Sections
                               .Where(x => x.Id == id)
                               .Include(x => x.CourseTerm.Term)
                               .Include(x => x.CourseTerm.Course.Career)
                               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SectionTemplateA>> GetAllByTermIdAsModelA(Guid termId, Guid? careerId = null, string coordinatorId = null, Guid? academicDepartmentId = null, ClaimsPrincipal user = null, Guid? curriculumId = null)
        {
            var academicYearCourses = await _context.AcademicYearCourses
                .Select(x => new Tuple<Guid, string, byte, bool>(x.CourseId, x.AcademicYear != 0 && x.AcademicYear < 20 ? ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.AcademicYear] : "", x.AcademicYear, x.IsElective))
                .ToListAsync();

            var teacherSections = await _context.TeacherSections
                .Where(X => X.Teacher != null && X.Teacher.User != null)
                                                .Include(x => x.Teacher.User)
                                                .Include(x => x.Teacher.AcademicDepartment.Career)
                                                .Where(x => x.Section.CourseTerm.TermId == termId)
                                                .ToListAsync();

            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == termId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.CareerId == careerId));

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.TeacherSections.Any(y => y.Teacher.AcademicDepartmentId == academicDepartmentId));

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (!string.IsNullOrEmpty(coordinatorId))
            {
                query = query.Where(x =>
                x.CourseTerm.Course.Career.AcademicCoordinatorId == coordinatorId ||
                x.CourseTerm.Course.Career.CareerDirectorId == coordinatorId ||
                x.CourseTerm.Course.Career.AcademicDepartmentDirectorId == coordinatorId ||
                x.CourseTerm.Course.Career.AcademicSecretaryId == coordinatorId
                );
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    query = query.Where(x =>
                        x.CourseTerm.Course.Career.AcademicCoordinatorId == userId ||
                        x.CourseTerm.Course.Career.CareerDirectorId == userId ||
                        x.CourseTerm.Course.Career.AcademicSecretaryId == userId
                        );
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    var academicDepartments = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId).Select(x => x.Id).ToListAsync();
                    query = query.Where(x =>
                        x.CourseTerm.Sections.Any(y => y.TeacherSections.Any(z => z.Teacher.AcademicDepartmentId.HasValue && academicDepartments.Contains(z.Teacher.AcademicDepartmentId.Value)))
                    ); ;
                }
            }

            var sectionsDB = await query
                .OrderBy(x => x.CourseTerm.Course.Code)
                .ThenBy(x => x.Code)
                .Select(x => new
                {
                    sectionId = x.Id,
                    careerId = x.CourseTerm.Course.CareerId,
                    AcademicDepartment = x.CourseTerm.Course.Career != null ? x.CourseTerm.Course.Career.Name : "-",
                    Faculty = x.CourseTerm.Course.Career != null ? x.CourseTerm.Course.Career.Faculty.Name : "-",
                    courseId = x.CourseTerm.CourseId,
                    CourseName = x.CourseTerm.Course.Name,
                    CourseCode = x.CourseTerm.Course.Code,
                    TheoricalHours = x.CourseTerm.Course.TheoreticalHours,
                    PracticalHours = x.CourseTerm.Course.PracticalHours,
                    VirutalHours = x.CourseTerm.Course.VirtualHours,
                    SeminarHours = x.CourseTerm.Course.SeminarHours,
                    TotalHours = x.CourseTerm.Course.TotalHours,
                    IsDirectedCourse = x.IsDirectedCourse,
                    career = x.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Career.Name).FirstOrDefault(),
                    Group = x.Code,
                    NumberStudents = x.StudentSections.Where(y => y.SectionId == x.Id && y.Section.CourseTerm.TermId == termId).Count(),
                    teachers = x.TeacherSections.Select(y=>y.Teacher.User.FullName).ToList(),
                    academicDeparments = x.TeacherSections.Select(y => y.Teacher.AcademicDepartment.Name ?? "-").ToList(),
                })
                .ToListAsync();

            var sections = sectionsDB
                .Select(x => new SectionTemplateA
                {
                    sectionId = x.sectionId,
                    careerId = x.careerId ?? Guid.Empty,
                    AcademicDepartment = x.AcademicDepartment,
                    Faculty = x.Faculty,
                    Course = $"{x.CourseCode}-{x.CourseName}",
                    Cycle = academicYearCourses.FirstOrDefault(z => z.Item1 == x.courseId) == null ? "-" : academicYearCourses.FirstOrDefault(z => z.Item1 == x.courseId).Item2,
                    AcademicYear = academicYearCourses.FirstOrDefault(z => z.Item1 == x.courseId) == null ? Convert.ToByte(0) : academicYearCourses.FirstOrDefault(z => z.Item1 == x.courseId).Item3,
                    IsElective = academicYearCourses.Where(z => z.Item1 == x.courseId).Select(x=>x.Item4).FirstOrDefault(),
                    //HT = x.HT,
                    //HP = x.HP,
                    //HL = x.HL,
                    TheoricalHours = x.TheoricalHours,
                    SeminarHours = x.SeminarHours,
                    VirtualHours = x.VirutalHours,
                    PracticalHours = x.PracticalHours,
                    TotalHours = x.TotalHours,
                    Teacher = x.teachers,
                    AcademicDepartmentTeacher = x.academicDeparments,
                    IsDirectedCourse = x.IsDirectedCourse,
                    Group = x.Group,
                    NumberStudents = x.NumberStudents,
                    Career = x.career
                })
                .OrderBy(x => x.AcademicYear)
                .ToList();


            foreach (var item in sections)
            {
                if (item.IsDirectedCourse)
                {
                    item.TotalHours = Math.Round((item.TotalHours / 2.0), 2);
                    item.PracticalHours = Math.Round((item.PracticalHours / 2.0), 2);
                    item.VirtualHours = Math.Round((item.VirtualHours / 2.0), 2);
                    item.SeminarHours = Math.Round((item.SeminarHours / 2.0), 2);
                    item.TheoricalHours = Math.Round((item.TheoricalHours / 2.0), 2);
                    item.Group = "DIRIGIDO";
                }

                var fromAnotherSchool = teacherSections.Where(x => x.SectionId == item.sectionId && x.Teacher?.AcademicDepartment?.CareerId != item.careerId).Select(x => x.Teacher.User.FullName).ToList();
                var planTeacher = teacherSections.Where(x => x.SectionId == item.sectionId && x.Teacher?.AcademicDepartment?.CareerId == item.careerId).Select(x => x.Teacher.User.FullName).ToList();
                var schoolOrigin = teacherSections.Where(x => x.SectionId == item.sectionId).Select(x => x.Teacher?.AcademicDepartment?.Name).ToList();

                item.TeacherFromAnotherSchool = fromAnotherSchool.Any() ? string.Join(", ", fromAnotherSchool) : "Sin Asignar";
                item.PlantTeacher = planTeacher.Any() ? string.Join(", ", planTeacher) : "Sin Asignar";
                item.SchoolOfOrigin = schoolOrigin.Any() ? string.Join(", ", schoolOrigin) : "Sin Asignar";
            }

            return sections;
        }

        public async Task<List<TeacherSectionTemplate>> GetTecachersByTerm(Guid termId)
        {
            var result = await _context.Sections
                 .Include(x => x.TeacherSections)
                 .Include(x => x.CourseTerm)
                 .Include(x => x.CourseTerm.Course)
                 .Include(x => x.CourseTerm.Term)
                 .Where(x => x.CourseTerm.TermId == termId)
                 .Select(x => new TeacherSectionTemplate
                 {
                     FullName = x.TeacherSections != null ? string.Join(", ", x.TeacherSections.Select(ts => ts.Teacher.User.FullName)) : "No Asignado",
                     CourseName = x.CourseTerm.Course.FullName,
                     Section = x.Code
                 }).ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? academicDepartmentId = null, string search = null, ClaimsPrincipal user = null)
        {

            Expression<Func<TeacherSection, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Teacher.User.FullName); break;
                case "1":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Code); break;
                case "2":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.Section.Code); break;
            }

            var query = _context.TeacherSections
                .Where(x => x.Section.CourseTerm.TermId == termId)
                .AsNoTracking();

            //var recordsTotal = await query.CountAsync();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryDepartments = _context.AcademicDepartments
                        .Where(x => x.AcademicDepartmentDirectorId == userId)
                        .AsNoTracking();

                    if (academicDepartmentId != null)
                    {
                        query = query.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);
                    }
                    else
                    {
                        var departments = qryDepartments.Select(x => x.Id).ToHashSet();
                        query = query.Where(x => x.Teacher.AcademicDepartmentId.HasValue && departments.Contains(x.Teacher.AcademicDepartmentId.Value));
                    }
                }
            }
            else if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (academicDepartmentId != null)
                    {
                        query = query.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);
                    }
                    else
                    {
                        var careers = qryCareers.Select(x => x.Id).ToHashSet();

                        query = query.Where(x => careers.Contains(x.Teacher.AcademicDepartment.CareerId.Value));
                    }
                }
            }
            else
            {
                if (academicDepartmentId != null) query = query.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);
            }


            var recordsFiltered = await query
                .Select(x => new
                {
                    x.Teacher.User.FullName,
                    x.Teacher.User.Name,
                    x.Teacher.User.PaternalSurname,
                    x.Teacher.User.MaternalSurname,
                    x.Section.CourseTerm.Course.Code,
                    course = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code
                }, search)
                .CountAsync();

            var data = await query
                    .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                    .Select(x => new
                    {
                        id = x.SectionId,
                        name = x.Teacher.User.Name,
                        paternalSurName = x.Teacher.User.PaternalSurname,
                        maternalSurName = x.Teacher.User.MaternalSurname,
                        teacher = x.Teacher.User.FullName,
                        code = x.Section.CourseTerm.Course.Code,
                        course = x.Section.CourseTerm.Course.Name,
                        section = x.Section.Code
                    }, search)
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public class TeacherReportTemp
        {
            public Guid Id { get; set; }
            public string Teacher { get; set; }
            public string Code { get; set; }
            public string Course { get; set; }
            public string Section { get; set; }
            public ICollection<Curriculum> Curriculums { get; set; }
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty, Guid? termId, string teacher, string teacherCode = null, ClaimsPrincipal user = null, Guid? careerId = null, Guid? departmentId = null, string courseName = null, Guid? curriculumId = null, string evaluationCode = null, Guid? academicProgramId = null)
        {
            Expression<Func<Section, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    //orderByPredicate = (x) => x.CourseTerm.Course.Name;
                    break;
            }

            var query = _context.Sections.Include(x => x.EvaluationReports)
                .Where(x => x.CourseTerm.TermId == termId)
                .AsQueryable();

            var sustituteExams = await _context.SubstituteExams.Where(x => x.Section.CourseTerm.TermId == termId && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED).ToArrayAsync();

            //var courseTermHashSet = evaluations.Select(x => x.CourseTermId).ToHashSet();

            //query = query.Where(x => courseTermHashSet.Contains(x.CourseTermId));


            if (faculty != null && faculty != Guid.Empty) query = query.Where(x => x.TeacherSections.Any(t => t.Teacher.Career.FacultyId == faculty));

            if (curriculumId != null && curriculumId != Guid.Empty) query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(z => z.CurriculumId == curriculumId));

            if (departmentId != null && departmentId != Guid.Empty) query = query.Where(x => x.TeacherSections.Any(t => t.Teacher.AcademicDepartmentId == departmentId));

            if (!string.IsNullOrEmpty(teacher) && teacher != Guid.Empty.ToString()) query = query.Where(x => x.TeacherSections.Any(t => t.IsPrincipal && t.TeacherId == teacher));

            if (!string.IsNullOrEmpty(teacherCode)) query = query.Where(x => x.TeacherSections.Any(t => t.IsPrincipal && t.Teacher.User.UserName.ToUpper().Contains(teacherCode.ToUpper())));

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var academicDepartments = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartmentId).ToArrayAsync();
                    query = query.Where(x => x.TeacherSections.Any(y => y.Teacher.AcademicDepartmentId.HasValue && academicDepartments.Contains(y.Teacher.AcademicDepartmentId.Value)));
                }
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    if (!careerId.HasValue || careerId == Guid.Empty)
                    {
                        var careers = await _context.Careers.Where(x => x.AcademicCoordinatorId == userId).Select(x => x.Id).ToArrayAsync();
                        query = query.Where(x => careers.Contains(x.CourseTerm.Course.CareerId.Value));
                    }

                }
            }

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
            {
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    query = query.Where(x => x.CourseTerm.Course.AcademicProgramId.Value == academicProgramId);
                }
                else
                {
                    query = query.Where(x => x.TeacherSections.Any(t => t.IsPrincipal && t.Teacher.CareerId == careerId));
                }

            }
            if (careerId.HasValue && careerId != Guid.Empty)
            {
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    query = query.Where(x => x.CourseTerm.Course.CareerId.Value == careerId);
                }
                else
                {
                    query = query.Where(x => x.TeacherSections.Any(t => t.IsPrincipal && t.Teacher.CareerId == careerId));
                }

            }
            //if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);

            if (!string.IsNullOrEmpty(courseName)) query = query.Where(x => x.CourseTerm.Course.Name.ToUpper().Contains(courseName.ToUpper()));


            if (!string.IsNullOrEmpty(evaluationCode)) query = query.Where(x => x.EvaluationReports.Any(z => z.Code == evaluationCode.Trim()));

            var reports = await _context.EvaluationReports
                .Where(x => x.Section.CourseTerm.TermId == termId
                && x.Type == ConstantHelpers.Intranet.EvaluationReportType.REGULAR || x.Type == ConstantHelpers.Intranet.EvaluationReportType.DIRECTED_COURSE)
                .Select(x => new
                {
                    x.SectionId,
                    x.Status,
                    x.CreatedAt,
                    x.PrintQuantity,
                    x.Code
                })
                .ToArrayAsync();

            var recordsFiltered = await query.CountAsync();

            var dbData = await query
                //.OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                //.Skip(sentParameters.PagingFirstRecord)
                //.Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    career = x.CourseTerm.Course.Career.Name,
                    course = x.CourseTerm.Course.FullName,
                    section = x.Code,
                    x.IsDirectedCourse,
                    //jalo la informacion requerida por los campos faltantes
                    id = x.Id,
                    teacherSections = string.Join(", ", x.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).ToList()),
                    courseTermId = x.CourseTermId,
                    hasEvaluations = x.CourseTerm.Evaluations.Any(),
                    allEvaluationsHasGrades = x.AcademicHistories.Any(),
                    gradeRegistrations = x.GradeRegistrations.ToList(),
                    academicYear = x.CourseTerm.Course.AcademicYearCourses.FirstOrDefault().AcademicYear

                }).ToListAsync();

            var dbData2 = dbData.OrderBy(x => x.academicYear).ThenBy(x => x.course).Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw).ToList();

            var data = dbData2
                .Select(x => new
                {
                    x.career,
                    x.course,
                    x.section,
                    x.id,
                    x.IsDirectedCourse,
                    //trabajo los campos faltantes en el lado del cliente
                    wasGenerated = reports.Any(r => r.SectionId == x.id),
                    status = reports.Any(r => r.SectionId == x.id)
                        ? ConstantHelpers.Intranet.EvaluationReport.NAMES[reports.FirstOrDefault(r => r.SectionId == x.id).Status]
                        : ConstantHelpers.Intranet.EvaluationReport.NAMES[ConstantHelpers.Intranet.EvaluationReport.PENDING],
                    teachers = x.teacherSections,
                    printQuantity = (reports.Where(r => r.SectionId == x.id).Select(y => y.PrintQuantity).FirstOrDefault()) ?? 0,
                    complete = x.hasEvaluations && x.allEvaluationsHasGrades,
                    hasSustituteExams = sustituteExams.Any(y => y.Section?.CourseTermId == x.courseTermId),
                    isUNAMAD = ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD,
                    academicYearName = $"Semestre {ConstantHelpers.ACADEMIC_YEAR.TEXT[x.academicYear]}",
                    academicYear = x.academicYear,
                    evaluationReportCode = reports.Where(r => r.SectionId == x.id).Select(y => y.Code).FirstOrDefault() ?? "Pendiente",
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetEvaluationReportDatatableV2(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? academicDepartmentId, string teacherId, string teacherCode, Guid? careerId, Guid? curriculumId, string courseSearch, ClaimsPrincipal user, int? academicYear)
        {
            Expression<Func<Section, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CourseTerm.Course.Career.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.CourseTerm.Course.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "3":
                    orderByPredicate = (x) => x.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault();
                    break;
                default:
                    orderByPredicate = (x) => x.CourseTerm.Course.Career.Name;
                    break;
            }

            var query = _context.Sections.Include(x => x.EvaluationReports)
                .Where(x => x.CourseTerm.TermId == termId)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var departments = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartmentId).ToArrayAsync();
                    var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();

                    query = query.Where(x => (x.CourseTerm.Course.CareerId.HasValue && careers.Contains(x.CourseTerm.Course.CareerId.Value)) || (x.TeacherSections.Any(y => y.Teacher.AcademicDepartment.CareerId.HasValue && departments.Contains(y.Teacher.AcademicDepartmentId.Value))));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    query = query.Where(x => x.CourseTerm.Course.Career.AcademicCoordinatorId == userId);
                }
            }

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.TeacherSections.Any(y => y.IsPrincipal && y.Teacher.AcademicDepartmentId == academicDepartmentId));

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.TeacherSections.Any(y => y.IsPrincipal && y.TeacherId == teacherId));

            if (!string.IsNullOrEmpty(teacherCode))
            {
                teacherCode = teacherCode.Trim().ToLower();
                query = query.Where(x => x.TeacherSections.Any(y => y.IsPrincipal && y.Teacher.User.UserName.ToLower() == teacherCode));
            }

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.CareerId == careerId) /*|| x.CourseTerm.Course.CareerId == careerId*/);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (academicYear.HasValue)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.AcademicYear == academicYear));

            if (!string.IsNullOrEmpty(courseSearch))
            {
                courseSearch = courseSearch.Trim().ToLower();
                query = query.Where(x => x.CourseTerm.Course.Name.ToLower().Contains(courseSearch) || x.CourseTerm.Course.Code.ToLower().Contains(courseSearch));
            }

            var recordsFiltered = await query.CountAsync();

            var dataDB = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    career = x.CourseTerm.Course.Career.Name,
                    course = x.CourseTerm.Course.FullName,
                    section = x.Code,
                    x.IsDirectedCourse,
                    id = x.Id,
                    teacherSections = string.Join(", ", x.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).ToList()),
                    courseTermId = x.CourseTermId,
                    hasEvaluations = x.CourseTerm.Evaluations.Any(),
                    allEvaluationsHasGrades = x.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE ? x.CourseTerm.Evaluations.Count() == x.GradeRegistrations.Count() : x.AcademicHistories.Any(),
                    wasGenerated = x.EvaluationReports.Any(),
                    status = x.EvaluationReports.Select(x => x.Status).FirstOrDefault(),
                    printQuantity = x.EvaluationReports.Select(x => x.PrintQuantity).FirstOrDefault(),
                    hasSustituteExams = x.SubstituteExams.Any(),
                    evaluationReportCode = x.EvaluationReports.Select(x => x.Code).FirstOrDefault(),
                    academicYear = x.CourseTerm.Course.AcademicYearCourses.Select(x => x.AcademicYear).FirstOrDefault()
                })
                .ToListAsync();

            var result = dataDB
             .Select(x => new
             {
                 x.career,
                 x.course,
                 x.section,
                 x.id,
                 x.IsDirectedCourse,
                 wasGenerated = x.wasGenerated,
                 status = x.status != 0
                     ? ConstantHelpers.Intranet.EvaluationReport.NAMES[x.status]
                    : ConstantHelpers.Intranet.EvaluationReport.NAMES[ConstantHelpers.Intranet.EvaluationReport.PENDING],
                 teachers = x.teacherSections,
                 printQuantity = x.printQuantity ?? 0,
                 complete = x.hasEvaluations && x.allEvaluationsHasGrades,
                 hasSustituteExams = x.hasSustituteExams,
                 isUNAMAD = ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD,
                 academicYearName = $"Semestre {ConstantHelpers.ACADEMIC_YEAR.TEXT[x.academicYear]}",
                 academicYear = x.academicYear,
                 evaluationReportCode = string.IsNullOrEmpty(x.evaluationReportCode) ? "Pendiente" : x.evaluationReportCode,
             })
             .ToList();

            var recordsTotal = result.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = result,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }
        public async Task<IEnumerable<Guid>> GetListSectionIdToGeneratedEvaluationReportInBlock(Guid termId, string teacherId = null, Guid? careerId = null)
        {
            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == termId)
                .AsNoTracking();

            var evaluations = await _context.Evaluations
                .Where(x => x.CourseTerm.TermId == termId)
                .Select(x => new
                {
                    EvaluationId = x.Id,
                    x.CourseTermId
                })
                .ToListAsync();

            var sustituteExams = await _context.SubstituteExams.Where(x => x.Section.CourseTerm.TermId == termId && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED).ToArrayAsync();

            //var courseTermHashSet = evaluations.Select(x => x.CourseTermId).ToHashSet();
            if (evaluations != null && evaluations.Count > 0)
                query = query.Where(x => evaluations.Any(e => e.CourseTermId == x.CourseTermId));
            //query = query.Where(x => courseTermHashSet.Contains(x.CourseTermId));

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.TeacherSections.Any(t => t.TeacherId == teacherId));

            if (Guid.Empty == careerId)
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);

            var reports = await _context.EvaluationReports
                .Where(x => x.Section.CourseTerm.TermId == termId
                && x.Type == ConstantHelpers.Intranet.EvaluationReportType.REGULAR)
                .Select(x => new
                {
                    x.SectionId
                })
                .ToArrayAsync();
            var data = new List<TmpDataStudentSections>();
            var result = new List<Guid>();

            if (ConstantHelpers.Institution.UNAMAD != ConstantHelpers.GENERAL.Institution.Value)
            {
                data = await query
                  .Select(x => new TmpDataStudentSections
                  {
                      wasGenerated = reports.Any(r => r.SectionId == x.Id),
                      id = x.Id,
                      //complete = evaluations.Where(e => e.CourseTermId == x.CourseTermId).All(e => x.GradeRegistrations.Any(gr => gr.EvaluationId == e.EvaluationId)),
                      hasSustituteExams = sustituteExams.Any(y => y.Section.CourseTermId == x.CourseTermId)
                  }).ToListAsync();
                result = data.Where(x => /*x.complete && */!x.hasSustituteExams && !x.wasGenerated).Select(x => x.id).ToList();
            }
            else
            {
                result = await query
                  .Select(x => x.Id).ToListAsync();
            }
            return result;
        }
        public class TmpDataStudentSections
        {
            public bool wasGenerated { get; set; }
            public Guid id { get; set; }
            public bool hasSustituteExams { get; set; }
        }

        public async Task<Section> GetSectionWithTermAndCareer(Guid sectionId)
        {
            var section = await _context.Sections
            .Where(x => x.Id == sectionId)
                .Include(x => x.ClassSchedules)
                .Include(x => x.CourseTerm.Term)
                .Include(x => x.CourseTerm.Course)
                .ThenInclude(x => x.Career)
                .ThenInclude(x => x.Faculty)
                .Include(x => x.CourseTerm.Course.AcademicYearCourses)
            .FirstOrDefaultAsync();

            return section;
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetSectionsByTermIdAndTeacherIdSelect2ClientSide(Guid termId, string teacherId)
        {
            var result = await _context.Sections
                .Where(x => !x.IsDirectedCourse && x.CourseTerm.TermId == termId && x.ClassSchedules.Any(y => y.TeacherSchedules.Any(z => z.TeacherId == teacherId)))
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.CourseTerm.Course.Name + " - Sección " + x.Code
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetSectionsByStudentIdAndTermIdSelect2ClientSide(Guid studentId, Guid termId)
        {
            var result = await _context.StudentSections
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.SectionId,
                    Text = $"{x.Section.CourseTerm.Course.Name} - {x.Section.Code}"
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetSectionsByCourseTermIdSelect2ClientSide(Guid courseTermId)
        {
            var result = await _context.Sections
                .Where(x => x.CourseTermId == courseTermId)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Code
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetSectionsByTermIdSelect2ClientSide(Guid termId, Guid courseId)
        {
            var result = await _context.Sections
                .Where(x => x.CourseTerm.TermId == termId && x.CourseTerm.CourseId == courseId)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Code
                })
                .OrderBy(x => x.Text)
                .ToArrayAsync();

            return result;
        }

        public async Task<object> GetAllAsModelB(Guid? termId = null, Guid? courseId = null)
        {
            var query = _context.Sections.Where(x => !x.IsDirectedCourse).AsNoTracking();
            var teacherQuery = _context.TeacherSections.AsNoTracking();

            if (termId.HasValue && termId != Guid.Empty)
            {
                query = query.Where(x => x.CourseTerm.TermId == termId.Value);
                teacherQuery = teacherQuery.Where(x => x.Section.CourseTerm.TermId == termId.Value);
            }

            if (courseId.HasValue && courseId != Guid.Empty)
            {
                query = query.Where(x => x.CourseTerm.CourseId == courseId.Value);
                teacherQuery = teacherQuery.Where(x => x.Section.CourseTerm.CourseId == courseId.Value);
            }

            var teacherSections = await teacherQuery
                .IgnoreQueryFilters()
                .Include(x => x.Teacher)
                .ThenInclude(x => x.User)
                .ToListAsync();

            var sections = await query
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    vacancies = x.Vacancies,
                    careerId = x.CourseTerm.Course.CareerId
                }).ToListAsync();

            var result = sections
                .Select(x => new
                {
                    x.id,
                    x.code,
                    teacher = teacherSections.Any(y => y.SectionId == x.id)
                        ? string.Join("<br/> ", teacherSections.Where(y => y.SectionId == x.id).Select(ts => ts.Teacher?.User?.FullName).OrderBy(ts => ts).ToList())
                        : "No Asignado",
                    x.vacancies,
                    x.careerId
                }).ToList();

            return result;
        }

        public async Task<object> GetAsModelC(Guid? id = null)
        {
            var result = await _context.Sections
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    teachers = (x.TeacherSections != null) ? x.TeacherSections.Select(ts => ts.TeacherId) : null,
                    vacancies = x.Vacancies
                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<Section> GetWithTeacherSectionsAndClassSchedules(Guid id)
        {
            var section = await _context.Sections.Include(x => x.TeacherSections).ThenInclude(x => x.Teacher.User)
                    .Include(x => x.ClassSchedules)
                    .ThenInclude(x => x.TeacherSchedules).FirstOrDefaultAsync(x => x.Id == id);

            return section;
        }

        public async Task<Section> GetWithCourseTermAndClassSchedules(Guid id)
        {
            var section = await _context.Sections
                .Include(s => s.ClassSchedules)
                .Include(s => s.StudentSections)
                .Include(x => x.CourseTerm.Term)
                .Include(x => x.CourseTerm.Course)
                .Include(x => x.ClassSchedules)
                .FirstOrDefaultAsync(x => x.Id == id);

            return section;
        }

        public async Task<object> GetStudentAvailableSectionsDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid courseId, Guid termId)
        {
            var studentGroupSelection = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENABLE_STUDENT_GROUP_SELECTION));

            var sectionGroups = await _context.SectionGroups
                .Select(x => new
                {
                    x.Id,
                    x.Code
                }).ToListAsync();

            var tmpEnrollments = await _context.TmpEnrollments
                .Where(x => x.Section.CourseTerm.TermId == termId && x.StudentId == studentId)
                .Select(x => new
                {
                    x.Section.CourseTerm.CourseId,
                    x.SectionId,
                    x.SectionGroupId,
                    ClassSchedules = x.Section.ClassSchedules.Where(y => !y.SectionGroupId.HasValue || y.SectionGroupId == x.SectionGroupId).ToList()
                })
                .ToListAsync();

            var studentSchedules = tmpEnrollments
                .Where(x => x.CourseId != courseId)
                .SelectMany(x => x.ClassSchedules).ToList();

            var sections = await _context.Sections
                .Where(x => x.CourseTerm.TermId == termId && x.CourseTerm.CourseId == courseId && !x.IsDirectedCourse)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    StudentSections = x.StudentSections
                        .Select(y => new
                        {
                            y.Id,
                            y.SectionGroupId
                        })
                        .ToList(),
                    x.Vacancies,
                    ClassSchedules = x.ClassSchedules
                        .Select(y => new
                        {
                            y.SectionGroupId,
                            y.WeekDay,
                            StartTime = y.StartTime,
                            EndTime = y.EndTime,
                            Teachers = y.TeacherSchedules.Select(z => z.Teacher.User.FullName).ToList(),
                            y.Classroom.Capacity,
                            y.Classroom.Building.CampusId
                        }).ToList(),
                    Active = x.TmpEnrollments.Any(te => te.StudentId == studentId)
                }).ToListAsync();

            var limitEnrollmentToCampus = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.LIMIT_ENROLLMENT_TO_STUDENT_CAMPUS));

            if (limitEnrollmentToCampus)
            {
                var studentCampus = (await _context.Students.FirstOrDefaultAsync(x => x.Id == studentId)).CampusId;
                sections = sections.Where(x => x.ClassSchedules.Any(y => y.CampusId == studentCampus)).ToList();
            }

            var enrollmentSections = new List<EnrollmentSectionTemplate>();

            foreach (var item in sections)
            {
                if (item.ClassSchedules.Any(x => x.SectionGroupId.HasValue) && studentGroupSelection)
                {
                    var groups = item.ClassSchedules
                        .Where(x => x.SectionGroupId.HasValue)
                        .Select(x => x.SectionGroupId)
                        .Distinct().ToList();


                    foreach (var group in groups)
                    {
                        var groupCode = sectionGroups.FirstOrDefault(x => x.Id == group);
                        var vacancies = item.ClassSchedules.Where(x => !x.SectionGroupId.HasValue || x.SectionGroupId == group)
                            .OrderBy(x => x.Capacity)
                            .Select(x => x.Capacity)
                            .FirstOrDefault();

                        if (vacancies > item.Vacancies) vacancies = item.Vacancies;

                        enrollmentSections.Add(new EnrollmentSectionTemplate
                        {
                            SectionId = item.Id,
                            Section = $"{item.Code} ({groupCode.Code})",
                            SectionGroupId = group.HasValue ? group.Value : Guid.Empty,
                            Vacancies = vacancies - item.StudentSections.Where(x => x.SectionGroupId == group).Count(),
                            IsActive = tmpEnrollments.Any(x => x.SectionId == item.Id && x.SectionGroupId == group),

                            Teachers = string.Join(", ", item.ClassSchedules
                                .Where(x => !x.SectionGroupId.HasValue || x.SectionGroupId == group)
                                .SelectMany(x => x.Teachers)
                                .ToList()
                                .OrderBy(x => x)
                                .Distinct()
                                .ToList()),

                            Schedules = string.Join(", ", item.ClassSchedules
                                .Where(x => !x.SectionGroupId.HasValue || x.SectionGroupId == group).OrderBy(x => x.WeekDay)
                                .Select(cs => ConstantHelpers.WEEKDAY.VALUES[cs.WeekDay] + " (" + cs.StartTime.ToLocalDateTimeFormatUtc() + " - " + cs.EndTime.ToLocalDateTimeFormatUtc() + ") ")
                                .ToList()),

                            Intersection = ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD ? false : item.ClassSchedules
                                .Where(x => !x.SectionGroupId.HasValue || x.SectionGroupId == group)
                                .Any(cs => studentSchedules.Any(ss =>
                                    ss.WeekDay == cs.WeekDay && ((ss.StartTime.ToLocalTimeSpanUtc() <= cs.StartTime.ToLocalTimeSpanUtc() && cs.StartTime.ToLocalTimeSpanUtc() < ss.EndTime.ToLocalTimeSpanUtc())
                                    || (ss.StartTime.ToLocalTimeSpanUtc() < cs.EndTime.ToLocalTimeSpanUtc() && cs.EndTime.ToLocalTimeSpanUtc() <= ss.EndTime.ToLocalTimeSpanUtc())
                                    || (cs.StartTime.ToLocalTimeSpanUtc() <= ss.StartTime.ToLocalTimeSpanUtc() && ss.StartTime.ToLocalTimeSpanUtc() < cs.EndTime.ToLocalTimeSpanUtc())
                                    || (cs.StartTime.ToLocalTimeSpanUtc() < ss.EndTime.ToLocalTimeSpanUtc() && ss.EndTime.ToLocalTimeSpanUtc() <= cs.EndTime.ToLocalTimeSpanUtc()))
                          ))
                        });
                    }
                }
                else
                {
                    enrollmentSections.Add(new EnrollmentSectionTemplate
                    {
                        SectionId = item.Id,
                        Section = item.Code,
                        Vacancies = item.Vacancies - item.StudentSections.Count,
                        IsActive = tmpEnrollments.Any(x => x.SectionId == item.Id),
                        SectionGroupId = Guid.Empty,

                        Schedules = string.Join(", ", item.ClassSchedules
                            .OrderBy(x => x.WeekDay)
                            .Select(cs => ConstantHelpers.WEEKDAY.VALUES[cs.WeekDay] + " (" + cs.StartTime.ToLocalDateTimeFormatUtc() + " - " + cs.EndTime.ToLocalDateTimeFormatUtc() + ") ")
                            .ToList()),

                        Teachers = string.Join(", ", item.ClassSchedules
                                .SelectMany(x => x.Teachers)
                                .ToList()
                                .OrderBy(x => x)
                                .Distinct()
                                .ToList()),

                        Intersection = ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD ? false : item.ClassSchedules
                          .Any(cs => studentSchedules.Any(ss =>
                              ss.WeekDay == cs.WeekDay && ((ss.StartTime.ToLocalTimeSpanUtc() <= cs.StartTime.ToLocalTimeSpanUtc() && cs.StartTime.ToLocalTimeSpanUtc() < ss.EndTime.ToLocalTimeSpanUtc())
                              || (ss.StartTime.ToLocalTimeSpanUtc() < cs.EndTime.ToLocalTimeSpanUtc() && cs.EndTime.ToLocalTimeSpanUtc() <= ss.EndTime.ToLocalTimeSpanUtc())
                              || (cs.StartTime.ToLocalTimeSpanUtc() <= ss.StartTime.ToLocalTimeSpanUtc() && ss.StartTime.ToLocalTimeSpanUtc() < cs.EndTime.ToLocalTimeSpanUtc())
                              || (cs.StartTime.ToLocalTimeSpanUtc() < ss.EndTime.ToLocalTimeSpanUtc() && ss.EndTime.ToLocalTimeSpanUtc() <= cs.EndTime.ToLocalTimeSpanUtc()))
                          ))
                    });
                }
            }

            var data = enrollmentSections
                .Select(x => new
                {
                    id = x.SectionId,
                    section = x.Section,
                    teachers = x.Teachers,
                    schedules = x.Schedules,
                    vacancies = x.Vacancies <= 0 ? 0 : x.Vacancies,
                    intersection = x.Intersection,
                    active = x.IsActive,
                    groupId = x.SectionGroupId,
                    value = x.SectionGroupId == Guid.Empty ? $"{x.SectionId}" : $"{x.SectionId}|{x.SectionGroupId}",
                }).ToList();

            var recordsFiltered = data.Count;
            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<Section>> GetSectionWithStudentSection(Guid courseId, Guid? termId = null)
        {

            var sections = await _context.Sections
                .Where(s => s.CourseTerm.TermId == termId &&
                            s.CourseTerm.CourseId == courseId &&
                            s.StudentSections.Count < s.Vacancies)
                .Include(x => x.StudentSections)
                .ToListAsync();

            return sections;
        }

        public async Task<Section> GetStudentSectionWithClassCourse(Guid id)
        {
            var section = await _context.Sections
                            .Include(s => s.ClassSchedules)
                            .Include(s => s.StudentSections)
                            .Include(s => s.CourseTerm)
                                .ThenInclude(ct => ct.Course)
                            .FirstOrDefaultAsync(s => s.Id == id);

            return section;
        }

        public async Task<Section> GetSectionIncludeClassStudentAndCourse(Guid id)
        {
            var section = await _context.Sections.Include(s => s.ClassSchedules).Include(s => s.StudentSections).Include(s => s.CourseTerm).ThenInclude(ct => ct.Course).FirstOrDefaultAsync(s => s.Id == id);

            return section;
        }

        public async Task<object> GetCoruseSections(List<ClassSchedule> studentSchedules, Guid studentId, Guid courseId, Guid? termId = null)
        {

            var result = await _context.Sections
                .Where(s => s.CourseTerm.CourseId == courseId && s.CourseTerm.TermId == termId)
                .Select(s => new
                {
                    id = s.Id,
                    section = s.Code,
                    teachers = string.Join(',',
                        _context.TeacherSchedules.Where(ts => ts.ClassSchedule.SectionId == s.Id).Select(ts => ts.Teacher.User.FullName).Distinct().ToList()),
                    schedules = string.Join(string.Empty,
                        s.ClassSchedules
                        .Select(cs =>
                        ConstantHelpers.WEEKDAY.VALUES[cs.WeekDay] + " (" + cs.StartTime.ToLocalDateTimeFormatUtc() + " - " + cs.EndTime.ToLocalDateTimeFormatUtc() + ") "
                        ).ToList()),
                    vacancies = s.Vacancies - s.StudentSections.Count,
                    active = s.StudentSections.Any(te => te.StudentId == studentId),
                    intersection = s.ClassSchedules
                        .Any(cs => studentSchedules.Any(ss =>
                            ss.WeekDay == cs.WeekDay && ((ss.StartTime <= cs.StartTime && cs.StartTime < ss.EndTime) || (ss.StartTime < cs.EndTime && cs.EndTime <= ss.EndTime) || (cs.StartTime <= ss.StartTime && ss.StartTime < cs.EndTime) || (cs.StartTime < ss.EndTime && ss.EndTime <= cs.EndTime))
                        ))
                })
                .ToListAsync();


            return result;
        }

        public async Task<object> GetSectionsByCareerSelect2ClientSide(Guid? careerId)
        {
            var query = _context.Sections.AsQueryable();
            if (careerId.HasValue)
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId.Value);

            var data = await query
                .Select(x =>
                new
                {
                    text = x.Code,
                    id = x.Id
                })
                .ToListAsync();

            return data;
        }
        public async Task<object> GetSectionByCourseId(Guid courseId)
        {
            var query = _context.Sections.AsQueryable();

            query = query.Where(x => x.CourseTerm.CourseId == courseId);

            var data = await query
                .Select(x =>
                new
                {
                    text = x.Code,
                    id = x.Id
                })
                .ToListAsync();

            return data;
        }
        public async Task<List<SubstituteExamSectionsTemplate>> GetSectionsByTermCareerCurriculumCycleCourseData(Guid termid, Guid? careerId, Guid? curriculumId, int cycleId, Guid? courseId, ClaimsPrincipal user = null, string search = null)
        {
            var query = _context.Sections
                .Where(x => x.SubstituteExams.Any() && x.CourseTerm.TermId == termid && x.Code != "EVALUACIÓN EXTRAORDINARIA")
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    query = query.Where(x =>
                        x.CourseTerm.Course.Career.AcademicCoordinatorId == userId ||
                        x.CourseTerm.Course.Career.CareerDirectorId == userId ||
                        x.CourseTerm.Course.Career.AcademicSecretaryId == userId
                        );
                }
            }

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId.Value);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId.Value));

            if (cycleId != 0)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.AcademicYear == cycleId));

            if (courseId.HasValue && courseId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.CourseId == courseId.Value);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.CourseTerm.Course.Name.ToUpper().Contains(search.ToUpper()));
            }

            var data = await query
                .Select(s => new SubstituteExamSectionsTemplate
                {
                    id = s.Id,
                    courseTermId = s.CourseTermId,
                    code = s.CourseTerm.Course.Code,
                    career = s.CourseTerm.Course.Career.Name,
                    name = s.CourseTerm.Course.Name,
                    academicYear = string.Join(",", s.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear)),
                    modality = "Regular",
                    teachers = string.Join(",", s.TeacherSections.Select(r => r.Teacher.User.FullName)),
                    group = s.Code,
                    groupcycle = "SEMESTRE: " + string.Join(",", s.CourseTerm.Course.AcademicYearCourses.Select(y => ConstantHelpers.ACADEMIC_YEAR.TEXT[y.AcademicYear])) + " - GRUPO(S): " + s.Code,
                    StudentsThatFit = s.SubstituteExams.Count(),
                    EnrolledStudents = s.SubstituteExams.Count(x => x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED)
                })
                .OrderBy(x => x.career)
                .ThenBy(x => x.code)
                .ToListAsync();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsByTermCareerCurriculumCycleCourseDataTable(DataTablesStructs.SentParameters sentParameters, Guid termid, Guid? careerId, Guid? curriculumId, int cycleId, Guid? courseId, ClaimsPrincipal user = null, string search = null)
        {
            //Expression<Func<Section, dynamic>> orderByPredicate = null;

            //switch (sentParameters.OrderColumn)
            //{
            //    default:
            //        orderByPredicate = (x) => x.CourseTerm.Course.Code;
            //        break;
            //}

            //var query = _context.Sections
            //    .Where(x => x.CourseTerm.SubstituteExams.Count() > 0)
            //    .AsNoTracking();

            var query = _context.Sections
                .Where(x => x.SubstituteExams.Any() && x.CourseTerm.TermId == termid && x.Code != "EVALUACIÓN EXTRAORDINARIA")
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    query = query.Where(x =>
                        x.CourseTerm.Course.Career.AcademicCoordinatorId == userId ||
                        x.CourseTerm.Course.Career.CareerDirectorId == userId ||
                        x.CourseTerm.Course.Career.AcademicSecretaryId == userId
                        );
                }
            }

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId.Value);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId.Value));

            if (cycleId != 0)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.AcademicYear == cycleId));

            if (courseId.HasValue && courseId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.CourseId == courseId.Value);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.CourseTerm.Course.Name.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                //.OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(s => new SubstituteExamSectionsTemplate
                {
                    id = s.Id,
                    courseTermId = s.CourseTermId,
                    code = s.CourseTerm.Course.Code,
                    career = s.CourseTerm.Course.Career.Name,
                    name = s.CourseTerm.Course.Name,
                    academicYear = string.Join(",", s.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear)),
                    modality = "Regular",
                    teachers = string.Join(",", s.TeacherSections.Select(r => r.Teacher.User.FullName)),
                    group = s.Code,
                    groupcycle = "SEMESTRE: " + string.Join(",", s.CourseTerm.Course.AcademicYearCourses.Select(y => ConstantHelpers.ACADEMIC_YEAR.TEXT[y.AcademicYear])) + " - GRUPO(S): " + s.Code
                })
                .OrderBy(x => x.career)
                .ThenBy(x => x.code)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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
        public async Task<SectionSubstituteExamViewModel> GetSubstituteSectionById(Guid sectionId)
        {

            var sections = await _context.Sections
              .Where(x => x.Id == sectionId && x.Code != "EVALUACIÓN EXTRAORDINARIA")
               .Select(s => new SectionSubstituteExamViewModel
               {
                   id = s.Id,
                   courseTermId = s.CourseTermId,
                   code = s.CourseTerm.Course.Code,
                   career = s.CourseTerm.Course.Career.Name,
                   name = s.CourseTerm.Course.Name,
                   academicYear = string.Join(",", s.CourseTerm.Course.AcademicYearCourses.Select(y => ConstantHelpers.ACADEMIC_YEAR.TEXT[y.AcademicYear])),
                   modality = "Regular",
                   teachers = string.Join(",", s.TeacherSections.Select(r => r.Teacher.User.FullName)),
                   group = s.Code,
                   groupcycle = "SEMESTRE: " + string.Join(",", s.CourseTerm.Course.AcademicYearCourses.Select(y => ConstantHelpers.ACADEMIC_YEAR.TEXT[y.AcademicYear])) + " - GRUPO: " + s.Code
               }).ToListAsync();

            var result = sections
                .Select(x => new SectionSubstituteExamViewModel
                {
                    id = x.id,
                    courseTermId = x.courseTermId,
                    code = x.code,
                    career = x.career,
                    name = x.name,
                    academicYear = x.academicYear,
                    modality = x.modality
                })
                .FirstOrDefault();

            result.teachers = string.Join(", ", sections.Select(x => x.teachers).ToList());
            result.group = string.Join(", ", sections.Select(x => x.group).ToList());
            return result;
        }
        public async Task<object> GetSectionByCourseTermIdSelect2(Guid coursetermId)
        {
            var query = _context.Sections.AsQueryable();

            query = query.Where(x => x.CourseTermId == coursetermId);

            var data = await query
                .Select(x =>
                new
                {
                    text = x.Code,
                    id = x.Id
                })
                .ToListAsync();

            return data;
        }
        public async Task<int> CountByGroupId(Guid id)
        {
            return await _context.Sections
                .Where(s => s.GroupId == id)
                .Select(s => s.Vacancies - s.StudentSections.Count)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> AvalableBySectionId(Guid id)
        {
            return await _context.Sections
                .Where(s => s.Id == id)
                .Select(s => (s.Vacancies - s.StudentSections.Count) > 0)
                .FirstOrDefaultAsync();
        }

        public async Task<Section> GetFirstBySectionCodeCourseAndTermId(string sectionCode, Guid courseId, Guid termId)
        {
            return await _context.Sections.FirstOrDefaultAsync(x => x.Code == sectionCode && x.CourseTerm.CourseId == courseId && x.CourseTerm.TermId == termId);
        }

        public async Task<object> GetAllCourseSectionsDataTableClientSide(Guid courseId, Guid termId, string search = null)
        {
            var query = _context.Sections
                  .Where(x => x.CourseTerm.CourseId == courseId && x.CourseTerm.TermId == termId);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Code.Contains(search));
            }

            var sections = await query
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    teacher = x.TeacherSections.Any()
                                            ? string.Join("<br/> ", x.TeacherSections.Select(ts => ts.Teacher.User.FullName))
                                            : "No Asignado",
                    vacancies = x.Vacancies
                }).ToListAsync();

            //return sections;
            return new DataTablesStructs.ReturnedData<object>
            {
                Data = sections,
                DrawCounter = 1,
                RecordsFiltered = sections.Count(),
                RecordsTotal = sections.Count()
            };
        }


        public async Task<object> GetJsonByCourseAndTermId(Guid courseId, Guid termId, bool withDirectedCourses = false)
        {
            var query = _context.Sections.Where(x => x.CourseTerm.CourseId == courseId && x.CourseTerm.TermId == termId).AsNoTracking();

            if (!withDirectedCourses)
                query = query.Where(x => !x.IsDirectedCourse);

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Code
                })
                .ToArrayAsync();

            return result;
        }

        #endregion

        public override async Task Insert(Section section)
        {
            await base.Insert(section);
            await MoodleCreateSection(section.Id);
        }

        public override async Task Delete(Section section)
        {
            await base.Delete(section);
            await MoodleDeleteSection(section.Id);
        }

        public override async Task Update(Section section)
        {
            await base.Update(section);
            await MoodleEditSection(section.Id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSummerEnrollmentDataTable(DataTablesStructs.SentParameters sentParameters, Guid termid, Guid? careerId = null, Guid? curriculumId = null, Guid? programId = null, int? cycle = null, Guid? courseId = null, string search = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Section, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CourseTerm.Course.AcademicYearCourses.Select(x => x.AcademicYear).FirstOrDefault();
                    break;
                case "1":
                    orderByPredicate = (x) => x.CourseTerm.Course.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.CourseTerm.Course.Name;
                    break;
                default:
                    //orderByPredicate = (x) => x.Code;
                    break;
            }

            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == termid)
                .AsNoTracking();

            if (programId.HasValue && programId != Guid.Empty) query = query.Where(x => x.CourseTerm.Course.AcademicProgramId == programId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty) query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId.Value));

            if (cycle.HasValue && cycle != 0) query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.AcademicYear == cycle));

            if (courseId.HasValue && courseId != Guid.Empty) query = query.Where(x => x.CourseTerm.CourseId == courseId.Value);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Code.ToUpper().Contains(search.ToUpper()) || x.CourseTerm.Course.Name.ToUpper().Contains(search.ToUpper()) || x.CourseTerm.Course.Code.ToUpper().Contains(search.ToUpper()));

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers.Where(x => x.CareerDirectorId == userId || x.AcademicCoordinatorId == userId);

                    if (careerId.HasValue && careerId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => x.CourseTerm.Course.CareerId.HasValue && careers.Contains(x.CourseTerm.Course.CareerId.Value));
                }
            }
            else if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CourseTerm.Course.CareerId == careerId.Value);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(s => new
                {
                    id = s.Id,
                    cycle = s.CourseTerm.Course.AcademicYearCourses.Select(x => x.AcademicYear).FirstOrDefault(),
                    code = s.CourseTerm.Course.Code,
                    name = s.CourseTerm.Course.Name,
                    group = s.Code,
                    career = s.CourseTerm.Course.Career.Name,
                    program = s.CourseTerm.Course.AcademicProgramId.HasValue ? s.CourseTerm.Course.AcademicProgram.Name : "---"
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsToAssignSectionGroup(DataTablesStructs.SentParameters sentParameters, Guid? careerId, Guid? academicProgramId, string searchValue = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Section, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.Code;
                    break;
            }

            var termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();

            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == termId && x.ClassSchedules.Any(y => y.SectionGroupId.HasValue))
                .AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    query = query.Where(x => x.CourseTerm.Course.Career.CareerDirectorId == userId || x.CourseTerm.Course.Career.AcademicCoordinatorId == userId || x.CourseTerm.Course.Career.AcademicSecretaryId == userId);
                }
            }

            if (careerId.HasValue)
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);

            if (academicProgramId.HasValue)
                query = query.Where(x => x.CourseTerm.Course.Career.AcademicPrograms.Any(y => y.Id == academicProgramId));

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => x.CourseTerm.Course.Code.ToLower().Contains(searchValue) ||
                x.CourseTerm.Course.Name.ToLower().Contains(searchValue) ||
                x.Code.ToLower().Contains(searchValue));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      id = x.Id,
                      section = x.Code,
                      course = x.CourseTerm.Course.Name,
                      teachersCount = x.TeacherSections.Count()
                  })
                  .ToListAsync();


            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };


        }

        public async Task<IEnumerable<Select2Structs.Result>> GetAvailableSectionsByCourseTermIdSelectClientSide(Guid courseTermId, Guid? selectedId = null)
        {
            var sections = await _context.Sections.Where(x =>
                x.CourseTermId == courseTermId &&
                x.StudentSections.Count() < x.Vacancies
                )
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Code,
                    Selected = (x.Id == selectedId) ? true : false
                })
                .ToListAsync();

            return sections;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsByCourseTermIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseTermId)
        {
            var query = _context.Sections
                .Where(x => x.CourseTermId == courseTermId)
                .AsQueryable();

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderBy(x => x.Code)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      x.Code,
                      enrolledStudents = x.StudentSections.Count(),
                      x.Vacancies,
                      vacanciesAvailable = (Convert.ToInt32(x.Vacancies) - x.StudentSections.Count())
                  })
                  .ToListAsync();


            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithTimeCrossingDataTable(DataTablesStructs.SentParameters sentParameters, Guid? termId, Guid? careerId, Guid? facultyId, Guid? courseId, string search, ClaimsPrincipal user = null)
        {
            var query = _context.Students.AsNoTracking();

            if (!termId.HasValue || termId == Guid.Empty)
            {
                var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
                if (term == null)
                    term = new Term();

                termId = term.Id;
            }

            query = query.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId.Value));

            if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId || x.Career.Faculty.AdministrativeAssistantId == userId);
                }
            }

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId.Value);
            if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId.Value);
            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper()) || x.User.FullName.ToUpper().Contains(search.ToUpper()));

            if (courseId.HasValue && courseId != Guid.Empty)
            {
                query = query.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId && y.Section.CourseTerm.CourseId == courseId));

                var dbData = await query
                    .Select(x => new
                    {
                        careercode = x.Career.Code,
                        careername = x.Career.Name,
                        studentname = x.User.FullName,
                        studentcode = x.User.UserName,
                        studentphone = x.User.PhoneNumber,
                        studentemail = x.User.Email,
                        studentid = x.Id,
                        studentSections = x.StudentSections
                        .Where(ss => ss.Section.CourseTerm.TermId == termId)
                        .Select(ss => new
                        {
                            ss.Section.CourseTerm.CourseId,
                            ss.SectionId,
                            classSchedules = ss.Section.ClassSchedules
                                .Where(y => !y.SectionGroupId.HasValue || y.SectionGroupId == ss.SectionGroupId)
                                .ToList()
                        }).ToList()
                    }).ToListAsync();

                var data = dbData
                    .Where(x => x.studentSections.Where(ss1 => ss1.CourseId == courseId)
                    .Any(ss1 => ss1.classSchedules
                        .Any(cs1 => x.studentSections.Where(ss2 => ss2.SectionId != ss1.SectionId)
                            .Any(ss2 => ss2.classSchedules.Any(cs2 => cs2.WeekDay == cs1.WeekDay
                                && ((cs2.StartTime.ToLocalTimeSpanUtc() <= cs1.StartTime.ToLocalTimeSpanUtc() && cs1.StartTime.ToLocalTimeSpanUtc() < cs2.EndTime.ToLocalTimeSpanUtc())
                                || (cs2.StartTime.ToLocalTimeSpanUtc() < cs1.EndTime.ToLocalTimeSpanUtc() && cs1.EndTime.ToLocalTimeSpanUtc() <= cs2.EndTime.ToLocalTimeSpanUtc())
                                || (cs1.StartTime.ToLocalTimeSpanUtc() <= cs2.StartTime.ToLocalTimeSpanUtc() && cs2.StartTime.ToLocalTimeSpanUtc() < cs1.EndTime.ToLocalTimeSpanUtc())
                                || (cs1.StartTime.ToLocalTimeSpanUtc() < cs2.EndTime.ToLocalTimeSpanUtc() && cs2.EndTime.ToLocalTimeSpanUtc() <= cs1.EndTime.ToLocalTimeSpanUtc())))))))
                    .Select(x => new
                    {
                        x.studentid,
                        x.studentname,
                        x.studentcode,
                        x.studentphone,
                        x.careercode,
                        x.careername,
                        x.studentemail
                    }).ToList();

                //var dbData = await query
                //    .Select(x => new
                //    {
                //        careercode = x.Career.Code,
                //        careername = x.Career.Name,
                //        studentname = x.User.FullName,
                //        studentcode = x.User.UserName,
                //        studentphone = x.User.PhoneNumber,
                //        studentemail = x.User.Email,
                //        studentid = x.Id,
                //        intersection = x.StudentSections.Where(ss1 => ss1.Section.CourseTerm.CourseId == courseId && ss1.Section.CourseTerm.TermId == termId)
                //        .Any(ss1 => ss1.Section.ClassSchedules.Where(cs1 => !cs1.SectionGroupId.HasValue || cs1.SectionGroupId == ss1.SectionGroupId)
                //            .Any(cs1 => x.StudentSections.Where(ss2 => ss2.Section.CourseTerm.TermId == termId && ss2.SectionId != ss1.SectionId)
                //                .Any(ss2 => ss2.Section.ClassSchedules.Where(cs2 => !cs2.SectionGroupId.HasValue || cs2.SectionGroupId == ss2.SectionGroupId)
                //                    .Any(cs2 => cs2.WeekDay == cs1.WeekDay && ((cs2.StartTime <= cs1.StartTime && cs1.StartTime < cs2.EndTime) || (cs2.StartTime < cs1.EndTime && cs1.EndTime <= cs2.EndTime) || (cs1.StartTime <= cs2.StartTime && cs2.StartTime < cs1.EndTime) || (cs1.StartTime < cs2.EndTime && cs2.EndTime <= cs1.EndTime))))))
                //    }).ToListAsync();

                //var data = dbData
                //    .Where(x => x.intersection)
                //    .Select(x => new
                //    {
                //        x.studentid,
                //        x.studentname,
                //        x.studentcode,
                //        x.studentphone,
                //        x.careercode,
                //        x.careername,
                //        x.studentemail
                //    }).ToList();

                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = data,
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = dbData.Count,
                    RecordsTotal = data.Count
                };
            }
            else
            {
                var dbData = await query
                    .Select(x => new
                    {
                        careercode = x.Career.Code,
                        careername = x.Career.Name,
                        studentname = x.User.FullName,
                        studentcode = x.User.UserName,
                        studentphone = x.User.PhoneNumber,
                        studentemail = x.User.Email,
                        studentid = x.Id,

                        studentSections = x.StudentSections
                        .Where(ss => ss.Section.CourseTerm.TermId == termId)
                        .Select(ss => new
                        {
                            ss.SectionId,
                            classSchedules = ss.Section.ClassSchedules
                                .Where(y => !y.SectionGroupId.HasValue || y.SectionGroupId == ss.SectionGroupId)
                                .ToList()
                        }).ToList(),

                        //  intersection = x.StudentSections.Where(ss1 => ss1.Section.CourseTerm.TermId == termId)
                        //.Any(ss1 => ss1.Section.ClassSchedules.Where(cs1 => !cs1.SectionGroupId.HasValue || cs1.SectionGroupId == ss1.SectionGroupId)
                        //    .Any(cs1 => x.StudentSections.Where(ss2 => ss2.Section.CourseTerm.TermId == termId && ss2.SectionId != ss1.SectionId)
                        //        .Any(ss2 => ss2.Section.ClassSchedules.Where(cs2 => !cs2.SectionGroupId.HasValue || cs2.SectionGroupId == ss2.SectionGroupId)
                        //            .Any(cs2 => cs2.WeekDay == cs1.WeekDay && ((cs2.StartTime <= cs1.StartTime && cs1.StartTime < cs2.EndTime) || (cs2.StartTime < cs1.EndTime && cs1.EndTime <= cs2.EndTime) || (cs1.StartTime <= cs2.StartTime && cs2.StartTime < cs1.EndTime) || (cs1.StartTime < cs2.EndTime && cs2.EndTime <= cs1.EndTime))))))
                    }).ToListAsync();

                var data = dbData
                    //.Where(x => x.intersection)
                    .Where(x => x.studentSections
                    .Any(ss1 => ss1.classSchedules
                        .Any(cs1 => x.studentSections.Where(ss2 => ss2.SectionId != ss1.SectionId)
                            .Any(ss2 => ss2.classSchedules.Any(cs2 => cs2.WeekDay == cs1.WeekDay
                                && ((cs2.StartTime.ToLocalTimeSpanUtc() <= cs1.StartTime.ToLocalTimeSpanUtc() && cs1.StartTime.ToLocalTimeSpanUtc() < cs2.EndTime.ToLocalTimeSpanUtc())
                                || (cs2.StartTime.ToLocalTimeSpanUtc() < cs1.EndTime.ToLocalTimeSpanUtc() && cs1.EndTime.ToLocalTimeSpanUtc() <= cs2.EndTime.ToLocalTimeSpanUtc())
                                || (cs1.StartTime.ToLocalTimeSpanUtc() <= cs2.StartTime.ToLocalTimeSpanUtc() && cs2.StartTime.ToLocalTimeSpanUtc() < cs1.EndTime.ToLocalTimeSpanUtc())
                                || (cs1.StartTime.ToLocalTimeSpanUtc() < cs2.EndTime.ToLocalTimeSpanUtc() && cs2.EndTime.ToLocalTimeSpanUtc() <= cs1.EndTime.ToLocalTimeSpanUtc())))))))

                    .Select(x => new
                    {
                        x.studentid,
                        x.studentname,
                        x.studentcode,
                        x.studentphone,
                        x.careercode,
                        x.careername,
                        x.studentemail
                    }).ToList();

                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = data,
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = dbData.Count,
                    RecordsTotal = data.Count
                };
            }

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTimeCrossingCoursesByStudent(Guid studentId, Guid termId)
        {
            var studentSections = await _context.StudentSections
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId)
                .Select(x => new
                {
                    code = x.Section.CourseTerm.Course.Code,
                    course = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    x.SectionId,
                    x.SectionGroupId,
                    classSchedules = x.Section.ClassSchedules
                    .Where(y => !y.SectionGroupId.HasValue || y.SectionGroupId == x.SectionGroupId)
                    .Select(y => new
                    {
                        y.WeekDay,
                        y.StartTime,
                        y.EndTime,
                        y.SectionGroupId
                    }).ToList()
                }).ToListAsync();

            var data = studentSections
                .Where(x => x.classSchedules.Any(cs => studentSections.Where(ss => ss.SectionId != x.SectionId)
                .Any(ss => ss.classSchedules.Any(cs2 => cs2.WeekDay == cs.WeekDay
                && ((cs2.StartTime.ToLocalTimeSpanUtc() <= cs.StartTime.ToLocalTimeSpanUtc() && cs.StartTime.ToLocalTimeSpanUtc() < cs2.EndTime.ToLocalTimeSpanUtc())
                || (cs2.StartTime.ToLocalTimeSpanUtc() < cs.EndTime.ToLocalTimeSpanUtc() && cs.EndTime.ToLocalTimeSpanUtc() <= cs2.EndTime.ToLocalTimeSpanUtc())
                || (cs.StartTime.ToLocalTimeSpanUtc() <= cs2.StartTime.ToLocalTimeSpanUtc() && cs2.StartTime.ToLocalTimeSpanUtc() < cs.EndTime.ToLocalTimeSpanUtc())
                || (cs.StartTime.ToLocalTimeSpanUtc() < cs2.EndTime.ToLocalTimeSpanUtc() && cs2.EndTime.ToLocalTimeSpanUtc() <= cs.EndTime.ToLocalTimeSpanUtc()))))))
                .Select(x => new
                {
                    x.code,
                    x.course,
                    x.section,
                    intersection = string.Join(", ", studentSections.Where(ss => ss.SectionId != x.SectionId && x.classSchedules.Any(cs => ss.classSchedules.Any(cs2 => cs2.WeekDay == cs.WeekDay
                    && ((cs2.StartTime.ToLocalTimeSpanUtc() <= cs.StartTime.ToLocalTimeSpanUtc() && cs.StartTime.ToLocalTimeSpanUtc() < cs2.EndTime.ToLocalTimeSpanUtc())
                    || (cs2.StartTime.ToLocalTimeSpanUtc() < cs.EndTime.ToLocalTimeSpanUtc() && cs.EndTime.ToLocalTimeSpanUtc() <= cs2.EndTime.ToLocalTimeSpanUtc())
                    || (cs.StartTime.ToLocalTimeSpanUtc() <= cs2.StartTime.ToLocalTimeSpanUtc() && cs2.StartTime.ToLocalTimeSpanUtc() < cs.EndTime.ToLocalTimeSpanUtc())
                    || (cs.StartTime.ToLocalTimeSpanUtc() < cs2.EndTime.ToLocalTimeSpanUtc() && cs2.EndTime.ToLocalTimeSpanUtc() <= cs.EndTime.ToLocalTimeSpanUtc())))))
                    .Select(ss => ss.code).ToList())
                }).ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = 0,
                RecordsFiltered = data.Count,
                RecordsTotal = data.Count
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportClassDetailDataTable(DataTablesStructs.SentParameters parameters, Guid sectionId)
        {
            var query = _context.StudentSections.Where(x => x.SectionId == sectionId).AsNoTracking();

            var sectioonsclasses = await _context.ClassStudents.Where(x => x.Class.SectionId == sectionId).Include(x => x.Class).ToListAsync();

            var totalClasses = await _context.Classes.Where(x => x.ClassSchedule.SectionId == sectionId).CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.StudentId,
                    Fullname = x.Student.User.FullName,
                    x.Id,
                    x.Status
                })
                .ToListAsync();

            var result = data
                .Select(x => new
                {
                    x.Id,
                    x.Fullname,
                    Disabled = x.Status == ConstantHelpers.STUDENT_SECTION_STATES.DPI,
                    Absences = sectioonsclasses.Where(y => y.StudentId == x.StudentId).Count(cs => cs.IsAbsent && cs.Class.StartTime.ToLocalTime() < DateTime.Now),
                    Assisted = sectioonsclasses.Where(y => y.StudentId == x.StudentId).Count(cs => !cs.IsAbsent && cs.Class.StartTime.ToLocalTime() < DateTime.Now),
                    MaxAbsences = (int)Math.Round(totalClasses * (1 - ((decimal)ConstantHelpers.COURSES.ATTENDANCE_MIN_PERCENTAGE / 100M)), 0, MidpointRounding.AwayFromZero),
                    Dictated = sectioonsclasses.Where(y => y.StudentId == x.StudentId).Count(c => c.Class.StartTime.ToLocalTime() < DateTime.Now)
                })
                .ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = result,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = result.Count,
                //RecordsFiltered = recordsFiltered,
                RecordsTotal = result.Count,
            };
        }

        public async Task<int> GetTotalClassesBySection(Guid sectionId)
            => await _context.Classes.Where(x => x.ClassSchedule.SectionId == sectionId).CountAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsWithOutEvaluationReport(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? careerId, Guid? curriculumId)
        {
            var query = _context.Sections
                .Where(x => !x.EvaluationReports.Any())
                .AsNoTracking();

            if (termId.HasValue && termId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.TermId == termId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault())
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    //academicYear = x.CourseTerm.Course.AcademicYearCourses.Select(y => $"{ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[y.AcademicYear].ToUpper()} CICLO").FirstOrDefault(),
                    academicYear = x.CourseTerm.Term.Name,
                    x.Id,
                    section = x.Code,
                    courseCode = x.CourseTerm.Course.Code,
                    courseName = x.CourseTerm.Course.Name,
                    teachers = string.Join(", ", x.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).ToList())
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsEnabledForSubstituteExam(DataTablesStructs.SentParameters parameters, Guid termId, Guid? careerId, Guid? curriculmId, string searchValue)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();

            var query = _context.Sections.Where(x => x.CourseTerm.TermId == termId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.CareerId == careerId));

            if (curriculmId.HasValue && curriculmId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculmId));

            query = query.Where(x => x.GradeRegistrations.Count() == x.CourseTerm.Evaluations.Count() && x.StudentSections.Any(y => y.FinalGrade < term.MinGrade)).AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.CourseTerm.Course.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.CourseTerm.Course.Code.ToLower().Contains(searchValue.ToLower()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
              .OrderBy(x => x.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault())
              .Skip(parameters.PagingFirstRecord)
              .Take(parameters.RecordsPerDraw)
              .Select(x => new
              {
                  section = x.Code,
                  courseCode = x.CourseTerm.Course.Code,
                  course = x.CourseTerm.Course.Name,
                  failedStudents = x.StudentSections.Where(y => y.FinalGrade < term.MinGrade).Count()
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetDirectedSectionsDatatable(DataTablesStructs.SentParameters parameters, string searchValue, ClaimsPrincipal user)
        {
            var termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();
            var query = _context.Sections.Where(x => x.CourseTerm.TermId == termId && x.IsDirectedCourse).AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y=>y.Curriculum.Career.CareerDirectorId == userId) || x.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.Career.AcademicSecretaryId == userId) || x.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.Career.AcademicCoordinatorId == userId));

                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    var careers = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId).Select(x => x.CareerId).ToListAsync();
                    query = query.Where(x => careers.Contains(x.CourseTerm.Course.CareerId));
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.CourseTerm.Course.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.CourseTerm.Course.Code.ToLower().Contains(searchValue.ToLower()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
              .Skip(parameters.PagingFirstRecord)
              .Take(parameters.RecordsPerDraw)
              .Select(x => new
              {
                  x.Id,
                  x.Code,
                  courseCode = x.CourseTerm.Course.Code,
                  courseName = x.CourseTerm.Course.Name,
                  career = x.CourseTerm.Course.Career.Name,
                  teacher = string.Join(", ", x.TeacherSections.Select(y => y.Teacher.User.FullName).ToList()),

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

        public async Task<DataTablesStructs.ReturnedData<object>> GetCapacityFullfilmentReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, int capacity = 0, ClaimsPrincipal user = null)
        {
            Expression<Func<Section, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CourseTerm.Course.Career.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.CourseTerm.Course.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "3":
                    orderByPredicate = (x) => x.StudentSections.Count;
                    break;
                default:
                    break;
            }

            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == termId && x.StudentSections.Count <= capacity)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {

                    var qryCareers = _context.Careers
                         .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                         .AsNoTracking();

                    if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => x.CourseTerm.Course.CareerId.HasValue
                    && careers.Contains(x.CourseTerm.Course.CareerId.Value));
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    var qryCareers = _context.Careers.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId).AsNoTracking();

                    if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => x.CourseTerm.Course.CareerId.HasValue
                    && careers.Contains(x.CourseTerm.Course.CareerId.Value));
                }
                else if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);
            }
            else
            {
                if (careerId != Guid.Empty) query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    course = x.CourseTerm.Course.Name,
                    career = x.CourseTerm.Course.Career.Name,
                    students = x.StudentSections.Count
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<SectionCapacityTemplate>> GetCapacityFullfilmentReportData(Guid termId, Guid? careerId = null, int capacity = 0, ClaimsPrincipal user = null)
        {
            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == termId && x.StudentSections.Count <= capacity)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {

                    var qryCareers = _context.Careers
                         .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                         .AsNoTracking();

                    if (careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => x.CourseTerm.Course.CareerId.HasValue
                    && careers.Contains(x.CourseTerm.Course.CareerId.Value));
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    var qryCareers = _context.Careers.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId).AsNoTracking();

                    if (careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => x.CourseTerm.Course.CareerId.HasValue
                    && careers.Contains(x.CourseTerm.Course.CareerId.Value));
                }
                else if (careerId != Guid.Empty) query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);
            }
            else
            {
                if (careerId != Guid.Empty) query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);
            }

            var data = await query
              .Select(x => new SectionCapacityTemplate
              {
                  Code = x.Code,
                  Course = x.CourseTerm.Course.Name,
                  Career = x.CourseTerm.Course.Career.Name,
                  Students = x.StudentSections.Count
              }).ToListAsync();

            return data;
        }

        public async Task<SectionIncompleteScheduleReportTemplate> GetSectionIncompleteSchedulesTemplate(Guid termId, Guid? careerId, Guid? curriculumId, ClaimsPrincipal user)
        {
            var confi_pedagogical_hour_time = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME).FirstOrDefaultAsync();

            if (confi_pedagogical_hour_time is null)
            {
                confi_pedagogical_hour_time = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME,
                    Value = ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES[ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME]
                };
            }

            var termName = await _context.Terms.Where(x => x.Id == termId).Select(x => x.Name).FirstOrDefaultAsync();

            var model = new SectionIncompleteScheduleReportTemplate
            {
                Term = termName,
                Sections = new List<SectionIncompleteScheduleDetailReportTemplate>()
            };

            var pedagogical_hour_time = Convert.ToInt32(confi_pedagogical_hour_time.Value);

            var sectionGroups = await _context.SectionGroups.ToListAsync();

            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == termId)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y =>
                      y.Curriculum.Career.AcademicCoordinatorId == userId ||
                      y.Curriculum.Career.CareerDirectorId == userId ||
                      y.Curriculum.Career.AcademicSecretaryId == userId
                    ));
                }
            }

            if (careerId.HasValue && careerId != Guid.Empty)
            {
                var career = await _context.Careers.Where(x => x.Id == careerId).Select(x => x.Name).FirstOrDefaultAsync();
                model.Career = career;
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId || x.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.CareerId == careerId));

            }

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
            {
                var curriculum = await _context.Curriculums.Where(x => x.Id == curriculumId).Select(x => x.Code).FirstOrDefaultAsync();
                model.Curriculum = curriculum;
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));
            }

            var sections = await query
                .Select(x => new SectionIncompleteScheduleDetailReportTemplate
                {
                    CourseCode = x.CourseTerm.Course.Code,
                    Course = x.CourseTerm.Course.Name,
                    Section = x.Code,
                    Enrolled = x.StudentSections.Count(),
                    TheoricalHours = x.CourseTerm.Course.TheoreticalHours,
                    PracticalHours = x.CourseTerm.Course.PracticalHours,
                    Credits = x.CourseTerm.Course.Credits,
                    Schedules = x.ClassSchedules.Select(y => new ScheduleReportTemplate
                    {
                        PedagogicalHourTime = pedagogical_hour_time,
                        StartTime = y.StartTime,
                        EndTime = y.EndTime,
                        SessionType = y.SessionType,
                        WeekDay = y.WeekDay,
                        SectionGroupId = y.SectionGroupId,
                        SectionGroup = y.SectionGroup.Code
                    }).ToList(),
                    Teachers = x.TeacherSections.Select(y => y.Teacher.User.FullName).ToList()
                })
                .ToListAsync();

            var sectionWithOutSchedules = sections.Where(x => !x.Schedules.Any()).ToList();
            var sectionsTheory = sections.Where(x => x.Schedules.Where(y => y.SessionType == ConstantHelpers.SESSION_TYPE.THEORY).Sum(y => y.Duration) < x.TheoricalHours).ToList();
            var sectionsPractice = sections.Where(y => y.Schedules.Any(y => y.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE && !y.SectionGroupId.HasValue)).ToList()
                .Where(x => x.Schedules.Where(y => y.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE && !y.SectionGroupId.HasValue).Sum(y => y.Duration) < x.PracticalHours).ToList();

            var sectionsGroupsIncomplete = new List<SectionIncompleteScheduleDetailReportTemplate>();

            foreach (var sectionGroup in sectionGroups)
            {
                var sectionsGroupsIncompleteToAdd = sections
                    .Where(y => y.Schedules.Any(y => y.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE && y.SectionGroupId == sectionGroup.Id)).ToList()
                    .Where(x => x.Schedules.Where(y => y.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE && y.SectionGroupId.HasValue && y.SectionGroupId == sectionGroup.Id).Sum(y => y.Duration) < x.PracticalHours).ToList();
                sectionsGroupsIncomplete.AddRange(sectionsGroupsIncompleteToAdd);
            }

            model.Sections.AddRange(sectionWithOutSchedules);
            model.Sections.AddRange(sectionsTheory);
            model.Sections.AddRange(sectionsPractice);
            model.Sections.AddRange(sectionsGroupsIncomplete);

            model.Sections = model.Sections.Distinct().OrderBy(x => x.CourseCode).ThenBy(x => x.Course).ToList();

            model.Sections.ForEach(x => x.Schedules.ForEach(x => x.PedagogicalHourTime = pedagogical_hour_time));

            return model;
        }
 
    
        public async Task<List<Section>> GetSectionByClassRange(string teacherId, DateTime start, DateTime end)
        {
            var sections = await _context.Classes
                .Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.TeacherId == teacherId) && c.StartTime < end && start < c.EndTime)
                .OrderBy(x=>x.StartTime)
                .Select(x=> new Section
                {
                    Id = x.Section.Id,
                    Code = x.Section.Code,
                    CourseTerm = new CourseTerm
                    {
                        Course = new Course
                        {
                            Code = x.Section.CourseTerm.Course.Code,
                            Name = x.Section.CourseTerm.Course.Name,
                        }
                    }
                })
                .ToListAsync();

            return sections;
        }

        public async Task<SectionProgressReportTemplate> GetSectionProgressReportTemplate(Guid sectionId, string teacherId = null)
        {
            var model = await _context.Sections.Where(x => x.Id == sectionId)
                .Select(x => new SectionProgressReportTemplate
                {
                    Course = $"{x.CourseTerm.Course.Code}-{x.CourseTerm.Course.Name}",
                    Term = x.CourseTerm.Term.Name,
                    Curriculum = x.CourseTerm.Course.AcademicYearCourses.Select(y=>y.Curriculum.Code).FirstOrDefault(),
                    Career = x.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Career.Name).FirstOrDefault(),
                    AcademicYear = x.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                    Section = x.Code,
                    Enrolled = x.StudentSections.Count(),
                    HT = x.CourseTerm.Course.TheoreticalHours,
                    HP = x.CourseTerm.Course.PracticalHours,
                    Teacher = string.Join("; ", x.TeacherSections.Select(y=>y.Teacher.User.FullName).ToList()),
                    AcademicDepartment = string.Join("; ", x.TeacherSections.Select(y => y.Teacher.AcademicDepartment.Name).ToList()),

                    TermId = x.CourseTerm.TermId,
                    CourseId = x.CourseTerm.CourseId,

                    Details = new List<SectionProgressReportDetailTemplate>()
                })
                .FirstOrDefaultAsync();

            var queryClasses = _context.Classes.Where(x => x.SectionId == sectionId && x.UnitActivityId.HasValue).AsNoTracking();

            if (!string.IsNullOrEmpty(teacherId))
            {
                queryClasses = queryClasses.Where(x => x.TeacherId == teacherId);
                model.Teacher = await _context.Teachers.Where(x => x.UserId == teacherId).Select(x => x.User.FullName).FirstOrDefaultAsync();
            }

            var activities = await _context.UnitActivities.Where(x => x.CourseUnit.CourseSyllabus.CourseId == model.CourseId && x.CourseUnit.CourseSyllabus.TermId == model.TermId).ToListAsync();

            var classes = await queryClasses
                .OrderBy(x=>x.StartTime)
                .Select(x=> new SectionProgressReportDetailTemplate
                {
                    Date = x.StartTime,
                    Observation = x.Commentary,
                    Subject = x.UnitActivity.Name,
                    Students = x.ClassStudents.Count(y=>!y.IsAbsent),
                    SessionType = x.ClassSchedule.SessionType,
                    ActivityId = x.UnitActivityId.Value
                })
                .ToListAsync();

            model.Details = classes;
            model.Progress = activities.Count() < 1 ? 0 : (decimal)(activities.Count(x => classes.Any(y => y.ActivityId == x.Id)) * 100F) / activities.Count();
            model.Progress = Math.Round(model.Progress, 2, MidpointRounding.AwayFromZero);

            return model;
        }
    }
}