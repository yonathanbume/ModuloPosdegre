using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Course;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(AkdemicContext context) : base(context) { }

        public override async Task<Course> Get(Guid id)
        {
            return await _context.Courses
                .Include(x => x.Career.Faculty)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        async Task<DataTablesStructs.ReturnedData<object>> ICourseRepository.GetAllWithSyllabusByTermAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid termId)
        {
            var query = _context.Courses.AsQueryable();

            //switch (paginationParameter.SortField)
            //{
            //    case "0":
            //        query = paginationParameter.SortOrder == paginationParameter.BaseOrder
            //            ? query.OrderByDescending(q => q.Code)
            //            : query.OrderBy(q => q.Code);
            //        break;
            //    case "1":
            //        query = paginationParameter.SortOrder == paginationParameter.BaseOrder
            //            ? query.OrderByDescending(q => q.Name)
            //            : query.OrderBy(q => q.Name);
            //        break;
            //    case "2":
            //        query = paginationParameter.SortOrder == paginationParameter.BaseOrder
            //            ? query.OrderByDescending(q => q.CourseType.Name)
            //            : query.OrderBy(q => q.CourseType.Name);
            //        break;
            //    case "3":
            //        query = paginationParameter.SortOrder == paginationParameter.BaseOrder
            //            ? query.OrderByDescending(q => q.AreaId.HasValue ? q.Area.Name : q.Career.Name)
            //            : query.OrderBy(q => q.AreaId.HasValue ? q.Area.Name : q.Career.Name);
            //        break;
            //}

            //if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
            //    query = query.Where(q =>
            //        q.Code.Contains(paginationParameter.SearchValue) ||
            //        q.Name.Contains(paginationParameter.SearchValue) ||
            //        q.CourseType.Name.Contains(paginationParameter.SearchValue) || (q.AreaId.HasValue
            //            ? q.Area.Name.Contains(paginationParameter.SearchValue)
            //            : q.Career.Name.Contains(paginationParameter.SearchValue)));

            query = query
                .Select(x => new Course
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    CourseType = new CourseType
                    {
                        Name = x.CourseType.Name
                    },
                    Credits = x.Credits,
                    Area = new Area
                    {
                        Name = x.Area.Name
                    },
                    Career = new Career
                    {
                        Name = x.Career.Name
                    },
                    SyllabusId = x.Sylabus.FirstOrDefault(s => s.TermId == termId).Id
                });

            return await query.ToDataTables<object>(sentParameters);
        }

        async Task<IEnumerable<Course>> ICourseRepository.GetAllByTeacherId(string teacherId, Guid termId)
        {
            var teachers = await _context.TeacherSchedules.Where(x =>
                    x.TeacherId == teacherId && x.ClassSchedule.Section.CourseTerm.TermId == termId)
                .Select(x =>
                    x.ClassSchedule.Section.CourseTerm.Course)
                .Select(x => new Course
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    CourseType = new CourseType
                    {
                        Name = x.CourseType.Name
                    },
                    Area = new Area
                    {
                        Name = x.Area.Name
                    },
                    Career = new Career
                    {
                        Name = x.Career.Name
                    }
                })
                .Distinct()
                .ToListAsync();

            return teachers;
        }

        public async Task<int> GetQuantityCoursesAssigned(Guid termId, string teacherId)
            => await _context.TeacherSections.Where(x => x.TeacherId == teacherId && x.Section.CourseTerm.TermId == termId).CountAsync();

        async Task<DataTablesStructs.ReturnedData<object>> ICourseRepository.GetAllByTermAndAreaCareerAndAcademicYearAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid areaCareerId, Guid academicProgramId, byte? academicYear, string searchValue, bool? onlyWithSections = null, ClaimsPrincipal user = null, bool? onlyWithOutCoordinators = null, Guid? curriculumId = null)
        {
            var query = _context.Courses.AsQueryable();

            if (user != null)
            {
                if (!curriculumId.HasValue || curriculumId == Guid.Empty)
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                        user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                        user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY)
                        )
                    {
                        var careers = await _context.Careers
                            .Where(x => x.AcademicCoordinatorId == userId ||
                                       x.CareerDirectorId == userId ||
                                       x.AcademicSecretaryId == userId
                            )
                            .Select(x => x.Id).ToListAsync();

                        query = query.Where(x => x.CareerId.HasValue && careers.Contains(x.CareerId.Value));
                    }

                    if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                    {
                        var careers = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId || x.AcademicDepartmentCoordinatorId == userId).Select(x => x.CareerId).ToListAsync();
                        query = query.Where(x => x.CareerId.HasValue && careers.Contains(x.CareerId.Value));
                    }
                }
            }

            if (areaCareerId != Guid.Empty)
                query = query.Where(q => q.AreaId.Value == areaCareerId || q.CareerId.Value == areaCareerId || q.AcademicYearCourses.Any(y => y.Curriculum.CareerId == areaCareerId));
            if (academicProgramId != Guid.Empty)
                query = query.Where(q => q.AcademicProgramId == academicProgramId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (academicYear.HasValue)
                query = query.Where(q =>
                    _context.AcademicYearCourses.Any(ay =>
                        ay.AcademicYear == academicYear && ay.CourseId == q.Id));

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(q =>
                    q.Code.Contains(searchValue) ||
                    q.Name.Contains(searchValue));

            //var sections = await _context.Sections
            //    .Where(s => s.CourseTerm.TermId == termId && !s.IsDirectedCourse)
            //    .Select(s => s.CourseTerm.CourseId)
            //    .ToArrayAsync();

            var courseTerm = await _context.CourseTerms
                .Where(x => x.TermId == termId && !string.IsNullOrEmpty(x.CoordinatorId))
                .Select(x => new { x.CourseId, x.Coordinator.FullName, x.CoordinatorId })
                .ToArrayAsync();

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    query = sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION
                        ? query.OrderByDescending(q => q.Code)
                        : query.OrderBy(q => q.Code);
                    break;
                case "2":
                    query = sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION
                        ? query.OrderByDescending(q => q.Name)
                        : query.OrderBy(q => q.Name);
                    break;
                case "3":
                    query = sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION
                        ? query.OrderByDescending(q => q.CourseType.Name)
                        : query.OrderBy(q => q.CourseType.Name);
                    break;
                case "4":
                    query = sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION
                        ? query.OrderByDescending(q => q.AreaId.HasValue ? q.Area.Name : q.Career.Name)
                        : query.OrderBy(q => q.AreaId.HasValue ? q.Area.Name : q.Career.Name);
                    break;
                case "5":
                    query = sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION
                        ? query.OrderByDescending(q => q.Credits)
                        : query.OrderBy(q => q.Credits);
                    break;
                case "6":
                    query = sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION
                        //? query.OrderByDescending(q => q.SectionCount)
                        //: query.OrderBy(q => q.SectionCount);
                        ? query.OrderByDescending(q => q.CourseTerms.Select(x => x.Sections).Count())
                        : query.OrderBy(q => q.CourseTerms.Select(x => x.Sections).Count());
                    break;
            }

            if (onlyWithSections.HasValue && onlyWithSections.Value)
            {
                query = query.Where(x => x.CourseTerms.Any(y => y.TermId == termId && y.Sections.Any(z => !z.IsDirectedCourse)));
                query = query.OrderByDescending(x => x.CourseTerms.Where(y => y.TermId == termId).Select(y => y.Sections).Count());
            }

            if (onlyWithOutCoordinators.HasValue && onlyWithOutCoordinators.Value)
            {
                query = query.Where(x => x.CourseTerms.Any(y => y.TermId == termId && string.IsNullOrEmpty(y.CoordinatorId)));
            }

            int recordsFiltered = await query.CountAsync();
            var dataDB = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new Course
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    AreaId = x.AreaId,
                    CareerId = x.CareerId,
                    CourseType = new CourseType
                    {
                        Name = x.CourseType.Name
                    },
                    Credits = x.Credits,
                    Area = new Area
                    {
                        Name = x.Area.Name
                    },
                    Career = new Career
                    {
                        Name = x.Career.Name
                    }
                })
                .ToListAsync();

            var sections = await _context.Sections.Where(x => dataDB.Select(y => y.Id).ToList().Contains(x.CourseTerm.CourseId) && !x.IsDirectedCourse && x.CourseTerm.TermId == termId)
                .Select(x=> new
                {
                    x.Id,
                    x.CourseTerm.CourseId
                })
                .OrderBy(X=>X.CourseId)
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    x.AreaId,
                    x.CareerId,
                    x.CourseType,
                    x.Credits,
                    x.Area,
                    x.Career,
                    sectionCount = sections.Where(y=>x.Id == y.CourseId).Count(),
                    coordinator = courseTerm.Where(y => y.CourseId == x.Id).Select(y => y.FullName).FirstOrDefault(),
                    coordinatorId = courseTerm.Where(y => y.CourseId == x.Id).Select(y => y.CoordinatorId).FirstOrDefault()
                })
                .ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<CourseTeacherReportExcel>> GetCourseTeacherExcel(ClaimsPrincipal user, Guid termId, Guid? careerId, Guid? academicProgramId, Guid? curriculumId, byte? academicYear, bool? onlyWithSections, bool? onlyWithOutCoordinators)
        {
            var query = _context.Courses.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)
                    )
                {
                    var careers = await _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId ||
                                   x.CareerDirectorId == userId ||
                                   x.AcademicDepartmentDirectorId == userId
                        )
                        .Select(x => x.Id).ToListAsync();

                    query = query.Where(x => x.CareerId.HasValue && careers.Contains(x.CareerId.Value));
                }
            }

            if (careerId != Guid.Empty && careerId.HasValue)
                query = query.Where(q => q.CareerId.HasValue && q.CareerId.Value == careerId);

            if (academicProgramId != Guid.Empty && academicProgramId.HasValue)
                query = query.Where(q => q.AcademicProgramId == academicProgramId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (academicYear.HasValue)
                query = query.Where(q =>
                    _context.AcademicYearCourses.Any(ay =>
                        ay.AcademicYear == academicYear && ay.CourseId == q.Id));


            if (onlyWithSections.HasValue && onlyWithSections.Value)
            {
                query = query.Where(x => x.CourseTerms.Any(y => y.TermId == termId && y.Sections.Any(z => !z.IsDirectedCourse)));
                query = query.OrderByDescending(x => x.CourseTerms.Where(y => y.TermId == termId).Select(y => y.Sections).Count());
            }

            if (onlyWithOutCoordinators.HasValue && onlyWithOutCoordinators.Value)
            {
                query = query.Where(x => x.CourseTerms.Any(y => y.TermId == termId && string.IsNullOrEmpty(y.CoordinatorId)));
            }

            var sections = await _context.Sections
               .Where(s => s.CourseTerm.TermId == termId && !s.IsDirectedCourse)
               .Select(s => s.CourseTerm.CourseId)
               .ToArrayAsync();

            var courseTerm = await _context.CourseTerms
                .Where(x => x.TermId == termId && !string.IsNullOrEmpty(x.CoordinatorId))
                .Select(x => new { x.CourseId, x.Coordinator.FullName, x.CoordinatorId })
                .ToArrayAsync();

            var dataDB = await query
                .Select(x => new Course
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    AreaId = x.AreaId,
                    CareerId = x.CareerId,
                    CourseType = new CourseType
                    {
                        Name = x.CourseType.Name
                    },
                    Credits = x.Credits,
                    Area = new Area
                    {
                        Name = x.Area.Name
                    },
                    Career = new Career
                    {
                        Name = x.Career.Name
                    }
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new CourseTeacherReportExcel
                {
                    Career = x.Career.Name,
                    Coordinator = courseTerm.Where(y => y.CourseId == x.Id).Select(y => y.FullName).FirstOrDefault(),
                    Course = x.Name,
                    CourseCode = x.Code,
                    Credits = x.Credits,
                    Sections = sections.Count(s => s == x.Id),
                    Type = x.CourseType.Name
                })
                .ToList();

            return data;
        }

        async Task<IEnumerable<CourseATemplate>> ICourseRepository.GetCoursesATemplate(Guid termId, Guid? careerId, Guid? courseTypeId, ClaimsPrincipal user = null)
        {
            var coursesQuery = _context.Courses
                .Where(x => x.CourseTerms.Any(y => y.TermId == termId));

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    coursesQuery = coursesQuery.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }
            }

            if (careerId.HasValue && careerId.Value != Guid.Empty)
                coursesQuery = coursesQuery.Where(x => x.CareerId == careerId.Value);

            if (courseTypeId.HasValue && courseTypeId.Value != Guid.Empty)
                coursesQuery = coursesQuery.Where(x => x.CourseTypeId == courseTypeId.Value);

            var cterms = await _context.CourseTerms.Where(x => x.TermId == termId).Select(x => new { x.CourseId, x.Id }).ToListAsync();

            var coursesData = await coursesQuery
                .Select(x => new CourseATemplate
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    CourseTypeId = x.CourseTypeId,
                    AreaCareer = x.CareerId.HasValue ? x.Career.Name : "---",
                    Type = x.CourseType.Name,
                    CareerId = x.CareerId
                }).ToListAsync();

            var courses = coursesData
                .Select(x => new CourseATemplate
                {
                    Ctid = cterms.First(y => y.CourseId == x.Id).Id,
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    CourseTypeId = x.CourseTypeId,
                    AreaCareer = x.AreaCareer,
                    Type = x.Type,
                    CareerId = x.CareerId
                }).ToList();

            return courses;
        }

        async Task<CourseBTemplate> ICourseRepository.GetCourseBTemplate(Guid courseTermId)
        {
            var course = await _context.Courses
                .Where(x => x.CourseTerms.Any(y => y.Id == courseTermId))
                .Select(
                    x => new CourseBTemplate
                    {
                        FullName = x.FullName,
                        CourseTermId = courseTermId
                    }
                ).FirstOrDefaultAsync();

            return course;
        }

        async Task<DataTablesStructs.ReturnedData<object>> ICourseRepository.GetAllWithSyllabusComplianceDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, int state, Guid careerOrAreaId, ClaimsPrincipal user = null)
        {
            var request = await _context.SyllabusRequests.FirstOrDefaultAsync(x => x.TermId == termId);
            var existRequest = request != null;
            var isInPeriod = existRequest && DateTime.UtcNow >= request.Start && DateTime.UtcNow <= request.End;

            //var query = _context.Courses.AsQueryable();
            var query = _context.AcademicYearCourses.Where(x => x.Course.CourseTerms.Any(y => y.TermId == termId)).AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR)
                    )
                {
                    var careers = await _context.Careers.Where(x =>
                    x.AcademicCoordinatorId == userId ||
                    x.AcademicDepartmentDirectorId == userId ||
                    x.CareerDirectorId == userId)
                        .Select(x => x.Id).ToListAsync();
                    query = query.Where(x => x.Course.CareerId.HasValue && careers.Contains(x.Course.CareerId.Value));
                }
            }

            if (careerOrAreaId != Guid.Empty)
                query = query.Where(x => x.Course.AreaId == careerOrAreaId || x.Course.CareerId == careerOrAreaId);

            var syllabusTeachers = _context.SyllabusTeachers.Include(x => x.CourseTerm.Course).Where(x => x.CourseTerm.TermId == termId && x.SyllabusRequestId == (request == null ? Guid.Empty : request.Id));

            var result = query.Select(x => new
            {
                code = x.Course.Code,
                name = x.Course.Name,
                curriculumId = x.CurriculumId,
                courseId = x.CourseId,
                curriculum = x.Curriculum.Name,
                termId,
                existRequest,
                hasSyllabus = x.Course.CourseTerms.Any(y => y.TermId == termId && y.SyllabusTeachers.Any()),
                syllabus = new
                {
                    isDigital = syllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id).Select(y => y.IsDigital).FirstOrDefault(),
                    id = syllabusTeachers.Where(y => y.CourseTerm.CourseId == x.Course.Id).Select(y => y.Id).FirstOrDefault()
                }
            });

            if (state != 0)
            {
                if (state == 1)
                {
                    result = result.Where(x => x.hasSyllabus);
                }
                else if (state == 2)
                {
                    result = result.Where(x => !x.hasSyllabus);
                }
            }

            return await result.ToDataTables<object>(sentParameters);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByTermIdAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid termId, string name = null)
        {
            var query = _context.Sections
                .Include(x => x.TeacherSections)
                .Include(x => x.CourseTerm)
                .Include(x => x.CourseTerm.Course)
                .Include(x => x.CourseTerm.Term)
                .Where(x => x.CourseTerm.TermId == termId).AsQueryable();

            var result = query.Select(x => new
            {
                FullName = x.TeacherSections != null ? string.Join(", ", x.TeacherSections.Select(ts => ts.Teacher.User.FullName)) : "No Asignado",
                CourseName = x.CourseTerm.Course.FullName,
                Section = x.Code
            });

            //if (!string.IsNullOrEmpty(name))
            //{
            //    result = result.Where(x => name.SplitContains(x.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)));
            //}

            return await result.ToDataTables<object>(sentParameters);

        }
        public async Task<IEnumerable<Select2Structs.Result>> GetCoursesSelect2ClientSide(Guid? careerId = null, string search = null, Guid? curriculumId = null, bool onlyWithSections = false)
        {
            var query = _context.Courses.AsQueryable();

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToLower().Contains(search.Trim().ToLower()));

            if (curriculumId.HasValue)
            {
                var academicYearCourses = await _context.AcademicYearCourses.Where(x => x.CurriculumId == curriculumId).Select(x => x.CourseId).ToArrayAsync();
                query = query.Where(x => academicYearCourses.Any(y => y == x.Id));
            }

            if (onlyWithSections)
            {
                var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
                query = query.Where(x => x.CourseTerms.Any(y => y.TermId == term.Id && y.Sections.Any()));
            }

            var result = await query
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = $"{x.Code}-{x.Name}"
                })
                .ToArrayAsync();

            return result;
        }
        public async Task<IEnumerable<Select2Structs.Result>> GetCoursesSelect2ClientSide(Guid termId, Guid facultyId, Guid academicProgramId, Guid? planId = null, string search = null)
        {
            var query = _context.CourseTerms
                .Where(x => x.Course.Career.FacultyId == facultyId && x.TermId == termId)
                .AsQueryable();

            if (planId.HasValue)
            {
                var academicProgramCurriculums = _context.AcademicProgramCurriculums.Where(x => x.CurriculumId == planId && x.AcademicProgramId == academicProgramId);
                query = query.Where(x => academicProgramCurriculums.Any(c => c.AcademicProgram.Courses.Any(y => x.Id == y.Id)));
            }

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Course.Name.ToLower().Contains(search.Trim().ToLower()));

            var result = await query
                .Select(x => new Select2Structs.Result
                {
                    Id = x.CourseId,
                    Text = $"{x.Course.Code} - {x.Course.Name}"
                })
                .OrderBy(x => x.Text)
                .ToArrayAsync();

            return result;
        }


        public Task<bool> AnyByCodeAndName(string code, string name)
        {
            return _context.Courses.AnyAsync(x => x.Code == code && x.Name == name);
        }

        public async Task<object> GetAllAsSelectClientSide(string name = null, Guid? careerId = null, Guid? academicProgramId = null)
        {
            var query = _context.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Name.ToUpper().Contains(name.ToUpper()) || x.Code.ToUpper().Contains(name.ToUpper()));

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId.Value);

            if (academicProgramId.HasValue)
                query = query.Where(x => x.AcademicProgramId == academicProgramId.Value);

            var courses = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.FullName
                })
                .Take(10)
                .ToListAsync();

            return courses;
        }

        public async Task<bool> AnyByCode(string code, Guid? id = null)
        {
            if (id.HasValue)
            {
                return await _context.Courses.AnyAsync(x => x.Code.Equals(code) && !x.Id.Equals(id));
            }
            else
            {
                return await _context.Courses.AnyAsync(x => x.Code.Equals(code));
            }
        }

        public async Task<Course> GetByCode(string code)
        {
            return await _context.Courses.Where(x => x.Code == code).FirstOrDefaultAsync();
        }


        public async Task<bool> AnyInArea(Guid areaId)
        {
            return await _context.Courses.AnyAsync(x => x.AreaId == areaId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllAsModelB(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, string teacherId = null, bool forCareerDirector = false, Guid? areaCareerId = null, Guid? planId = null, string coordinatorId = null, Guid? programId = null, int? cycle = null, string search = null, ClaimsPrincipal user = null,
            bool? withSections = null)
        {
            var query = _context.Courses.AsQueryable();
            var term = await _context.Terms.FindAsync(termId);

            if (withSections.HasValue && withSections.Value)
                query = query.Where(x => x.CourseTerms.Any(y => y.TermId == term.Id && y.Sections.Any()));

            if (forCareerDirector)
            {
                //var careers = GetCoordinatorCareers(coordinatorId);
            }

            if (!string.IsNullOrEmpty(teacherId))
            {
                var teacherCourses = await _context.TeacherSections.Where(x => x.TeacherId == teacherId && x.Section.CourseTerm.TermId == termId).Select(x => x.Section.CourseTerm.CourseId).Distinct().ToListAsync();
                query = query.Where(x => teacherCourses.Contains(x.Id));
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if ((user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY)))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var qryCareers = _context.Careers
                            .Where(x => x.AcademicCoordinatorId == userId ||
                            x.CareerDirectorId == userId ||
                            x.AcademicDepartmentDirectorId == userId ||
                            x.AcademicSecretaryId == userId)
                            .AsNoTracking();

                        var careers = qryCareers.Select(x => x.Id).ToHashSet();
                        query = query.Where(x => careers.Contains(x.CareerId.Value));
                    }
                }
            }

            if (areaCareerId.HasValue && areaCareerId.Value != Guid.Empty)
                query = query.Where(x => x.CareerId == areaCareerId || x.AreaId == areaCareerId || x.AcademicYearCourses.Any(y => y.Curriculum.CareerId == areaCareerId));

            if (planId.HasValue && planId.Value != Guid.Empty)
                query = query.Where(x => x.AcademicYearCourses.Any(y => y.CurriculumId == planId));

            if (programId.HasValue && programId.Value != Guid.Empty)
            {
                query = query.Where(x => x.AcademicProgramId == programId);
            }

            if (cycle.HasValue && cycle.Value != 0)
            {
                query = query.Where(x => x.AcademicYearCourses.Any(y => y.AcademicYear == cycle.Value));
            }
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x =>
                x.Name.Trim().ToLower().Contains(search.Trim().ToLower()) ||
                x.Code.Trim().ToLower().Contains(search.Trim().ToLower()));
            }

            var rrr = query.Select(x => new
            {
                id = x.Id,
                code = x.Code,
                name = x.Name,
                areaCareer = x.CareerId.HasValue ? x.Career.Name : "---",
                cycle = x.AcademicYearCourses.Count > 0 ? ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.AcademicYearCourses.FirstOrDefault().AcademicYear] : "-",
                program = x.AcademicProgramId.HasValue ? x.AcademicProgram.Name : "---",
                type = x.CourseType.Name,
                hasSylabus = x.Sylabus.Any(s => s.TermId == termId),
                canEdit = term.Status != ConstantHelpers.TERM_STATES.FINISHED,
                careerId = x.CareerId,
                area = x.AreaId.HasValue ? x.Area.Name : "---",
                academicYear = x.AcademicYearCourses.Count > 0 ? x.AcademicYearCourses.FirstOrDefault().AcademicYear : 0
            })
                .OrderBy(x => x.academicYear)
            .ThenBy(x => x.code);

            return await rrr.ToDataTables<object>(sentParameters);
        }
        public async Task<Select2Structs.ResponseParameters> GetCoursesServerSideSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? selectedId = null, ClaimsPrincipal user = null)
        {
            return await GetCoursesServerSideSelect2Async(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = $"{x.Code} - {x.Name}",
                Selected = x.Id == selectedId
            }, searchValue, user);
        }

        private async Task<Select2Structs.ResponseParameters> GetCoursesServerSideSelect2Async(Select2Structs.RequestParameters requestParameters, Expression<Func<Course, Select2Structs.Result>> selectPredicate = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            var query = _context.Courses
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) || x.Code.ToUpper().Contains(searchValue.ToUpper()));
            }

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(selectPredicate)
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByParameters(DataTablesStructs.SentParameters sentParameters, Guid pid, Guid aid, Guid cid, byte? ayid, Guid apid, string search)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Id == pid);

            if (term == null)
            {
                term = await _context.Terms.OrderByDescending(e => e.Id).FirstOrDefaultAsync();
            }
            var query = _context.Courses.AsNoTracking();
            //var academicYearCoursesQry = _context.AcademicYearCourses.AsNoTracking();

            if (aid != Guid.Empty)
            {
                query = query.Where(x => x.AreaId == aid || x.CareerId == aid || x.AcademicYearCourses.Any(ay => ay.Curriculum.CareerId == aid));
                //academicYearCoursesQry = academicYearCoursesQry.Where(x => x.Curriculum.CareerId == aid);
            }

            if (cid != Guid.Empty)
            {
                query = query.Where(x => x.AcademicYearCourses.Any(ay => ay.CurriculumId == cid));
                //academicYearCoursesQry = academicYearCoursesQry.Where(x => x.CurriculumId == cid);
            }

            if (ayid.HasValue) query = query.Where(x => x.AcademicYearCourses.Any(ay => ay.AcademicYear == ayid.Value));

            if (apid != Guid.Empty) query = query.Where(x => x.AcademicProgramId == apid);

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()) || x.Code.ToUpper().Contains(search.ToUpper()));

            //var academicYearCourses = await academicYearCoursesQry
            //    .Select(x => new
            //    {
            //        x.CourseId,
            //        x.CurriculumId,
            //        Curriculum = x.Curriculum.Name
            //    }).ToArrayAsync();

            //var courseTerms = _context.CourseTerms
            //    .Where(x => x.TermId == term.Id)
            //    .Select(x =>
            //        x.CourseId
            //    ).ToHashSet();

            var recordsTotal = await query.CountAsync();

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    query = query.OrderByCondition(sentParameters.OrderDirection, q => q.Code);
                    break;
                case "1":
                    query = query.OrderByCondition(sentParameters.OrderDirection, q => q.Name);
                    break;
                case "2":
                    //query = query.OrderByCondition(sentParameters.OrderDirection, q => q.Career.Name);
                    break;
                case "3":
                    query = query.OrderByCondition(sentParameters.OrderDirection, q => q.CourseType.Name);
                    break;
                case "4":
                    query = query.OrderByCondition(sentParameters.OrderDirection, q => q.Credits);
                    break;
                case "5":
                    //query = query.OrderByCondition(sentParameters.OrderDirection, q => q.curriculum);
                    break;
            }

            var courses = await query
                 .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    credits = x.Credits,
                    area = x.Area != null ? x.Area.Name : string.Empty,
                    career = x.Career != null ? x.Career.Name : string.Empty,
                    curriculum = cid != Guid.Empty ? x.AcademicYearCourses.Where(y => y.CurriculumId == cid).Select(y => y.Curriculum.Name).FirstOrDefault() : x.AcademicYearCourses.Where(y => y.CourseId == y.Id).Select(y => y.Curriculum.Name).FirstOrDefault(),
                    type = x.CourseType.Name,
                    //canEdit = true,
                    courseTermId = term != null && x.CourseTerms.Any(y => y.TermId == term.Id) ? term.Id.ToString() : string.Empty
                    //canEditInTerm = term != null ? term.Status != CORE.Helpers.ConstantHelpers.TERM_STATES.FINISHED : false
                }).ToListAsync();

            var data = courses
                .Select(x => new
                {
                    x.id,
                    x.code,
                    x.name,
                    x.credits,
                    x.area,
                    x.career,
                    curriculum = x.curriculum,
                    //curriculum = cid != Guid.Empty ? academicYearCourses.Where(ay => ay.CurriculumId == cid).Select(ay => ay.Curriculum).FirstOrDefault() : academicYearCourses.Where(ay => ay.CourseId == x.id).Select(ay => ay.Curriculum).FirstOrDefault(),
                    x.type,
                    canEdit = true,
                    courseTermId = x.courseTermId,
                    canEditInTerm = term != null ? term.Status != CORE.Helpers.ConstantHelpers.TERM_STATES.FINISHED : false
                }).ToList();

            var recordsFiltered = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                //RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
            };
        }

        public async Task<object> GetWithAcademicHistory(Guid id, Guid studentId, Guid curriculumId)
        {
            var academicYearCourse = await _context.AcademicYearCourses
                 .Where(y => y.CurriculumId == curriculumId && y.CourseId == id)
                 .Select(x => new
                 {
                     code = x.Course.Code,
                     course = x.Course.Name,
                     credits = x.Course.Credits,
                     curriculum = x.Curriculum.Code,
                     academicyear = x.AcademicYear
                 })
                 .FirstOrDefaultAsync();

            var query = _context.AcademicHistories
                .Where(x => x.CourseId == id && x.StudentId == studentId)
                .AsNoTracking();
            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD)
                query = query.Where(x => x.Term.Number != "A" && !x.Term.IsSummer);
            else if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSM)
                query = query
                    .Where(x => !x.Term.IsSummer
                    && x.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.REEVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED);
            else
                query = query
                    .Where(x => x.Type != ConstantHelpers.AcademicHistory.Types.EXTRAORDINARY_EVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.REEVALUATION
                    && x.Type != ConstantHelpers.AcademicHistory.Types.DEFERRED);
            
            var time = await query.CountAsync();

            var result = new
            {
                academicYearCourse.code,
                academicYearCourse.course,
                academicYearCourse.credits,
                time,
                academicYearCourse.curriculum,
                academicYearCourse.academicyear
            };

            return result;
        }

        public async Task UpdateCourseCodeJob()
        {
            var courses = await _context.Courses.ToListAsync();

            foreach (var item in courses)
            {
                var curriculumCode = await _context.AcademicYearCourses.Where(x => x.CourseId == item.Id).Select(x => x.Curriculum.Code).FirstOrDefaultAsync();

                item.Code = item.Code + "-" + curriculumCode;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Course> GetCourseBySectionId(Guid sectionId)
            => await _context.Courses.Where(x => x.CourseTerms.Any(y => y.Sections.Any(z => z.Id == sectionId))).FirstOrDefaultAsync();

        public async Task<Select2Structs.ResponseParameters> GetCoursesSelect2(Select2Structs.RequestParameters requestParameters, ClaimsPrincipal claimsPrincipal = null, string searchValue = null, Guid? academicProgramId = null, bool? generalCourses = null)
        {
            var query = _context.Courses.AsNoTracking();

            if (academicProgramId.HasValue && (!generalCourses.HasValue || !generalCourses.Value))
            {
                query = query.Where(x => x.AcademicProgramId == academicProgramId);
            }

            if (academicProgramId.HasValue && generalCourses.HasValue && generalCourses.Value)
            {
                query = query.Where(x => x.AcademicProgramId == academicProgramId || !x.AcademicProgramId.HasValue);
            }

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Code.Trim().ToLower().Contains(searchValue.ToLower().Trim()) || x.Name.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            if (claimsPrincipal != null)
            {
                var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (claimsPrincipal.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || claimsPrincipal.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT) || claimsPrincipal.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    var careers = await _context.Careers.Where(x => x.AcademicCoordinatorId == userId || x.AcademicDepartmentDirectorId == userId || x.CareerDirectorId == userId).Select(x => x.Id).ToArrayAsync();
                    query = query.Where(x => x.CareerId.HasValue && careers.Contains(x.CareerId.Value));
                }
            }

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.FullName
                })
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };

        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesDatatble(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? areaCareerId, Guid? academicProgramId, Guid? curriculumId, int? cycle, ClaimsPrincipal user, string searchValue)
        {
            var query = _context.Courses.AsQueryable();
            var term = await _context.Terms.FindAsync(termId);

            Expression<Func<Course, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code); break;
                case "1":
                    orderByPredicate = ((x) => x.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.Area.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.Career.Name); break;
                case "4":
                    orderByPredicate = ((x) => x.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault()); break;
                default:
                    orderByPredicate = ((x) => x.Code); break;
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.TEACHERS))
                {
                    var teacherCoures = await _context.TeacherSections.Where(x => x.TeacherId == userId && x.Section.CourseTerm.TermId == termId).Select(x => x.Section.CourseTerm.CourseId).ToListAsync();
                    query = query.Where(x => teacherCoures.Contains(x.Id));
                }
            }

            if (areaCareerId.HasValue && areaCareerId != Guid.Empty)
                query = query.Where(x => x.CareerId == areaCareerId || x.AreaId == areaCareerId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
                query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (cycle.HasValue && cycle.Value != 0)
                query = query.Where(x => x.AcademicYearCourses.Any(y => y.AcademicYear == cycle.Value));

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()) || x.Code.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            var data = query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    career = x.CareerId.HasValue ? x.Career.Name : "---",
                    cycle = x.AcademicYearCourses.Count > 0 ? ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.AcademicYearCourses.FirstOrDefault().AcademicYear] : "-",
                    program = x.AcademicProgramId.HasValue ? x.AcademicProgram.Name : "---",
                    type = x.CourseType.Name,
                    hasSylabus = x.Sylabus.Any(s => s.TermId == termId),
                    canEdit = term.Status != ConstantHelpers.TERM_STATES.FINISHED,
                    careerId = x.CareerId,
                    area = x.AreaId.HasValue ? x.Area.Name : "---",
                    academicYear = x.AcademicYearCourses.Count > 0 ? x.AcademicYearCourses.FirstOrDefault().AcademicYear : 0
                });

            return await data.ToDataTables<object>(parameters);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, Guid? programId = null, Guid? curriculumId = null, int? cycle = null, string search = null, ClaimsPrincipal user = null, Guid? termId = null)
        {
            var query = _context.Courses.AsNoTracking();

            if (termId.HasValue && termId != Guid.Empty)
            {
                query = query.Where(x => x.CourseTerms.Any(y => y.TermId == termId));
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY)
                    )
                {
                    query = query.Where(x => x.Career.CareerDirectorId == userId || x.Career.AcademicCoordinatorId == userId || x.Career.AcademicSecretaryId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    var careers = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId).Select(x => x.CareerId).ToListAsync();
                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }


            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
            if (curriculumId.HasValue && curriculumId != Guid.Empty) query = query.Where(x => x.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));
            if (programId.HasValue && programId != Guid.Empty) query = query.Where(x => x.AcademicProgramId == programId);

            if (cycle.HasValue) query = query.Where(x => x.AcademicYearCourses.Any(y => y.AcademicYear == cycle));

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.Code.ToUpper().Contains(search.ToUpper()) || x.Name.ToUpper().Contains(search.ToUpper()));

            var recordsTotal = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    cycle = x.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                    code = x.Code,
                    name = x.Name,
                    career = x.Career.Name,
                    program = x.AcademicProgramId.HasValue ? x.AcademicProgram.Name : "---"
                })
                .OrderBy(x => x.cycle)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsFiltered = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal,
            };
            return result;
        }
        public async Task<EditTemplate> GetCourseEditTemplate(Guid id)
        {
            return await _context.Courses
              .Where(x => x.Id == id)
              .Select(x => new EditTemplate
              {
                  AcademicProgram = x.AcademicProgramId.HasValue ? x.AcademicProgram.Name : "---",
                  Career = x.Career.Name,
                  Course = $"{x.Code}-{x.Name}",
                  CourseId = x.Id
              }).FirstOrDefaultAsync();
        }

        public async Task<object> GetTeacherCoursesSelect2ClientSide(string teacherId, Guid? termId = null)
        {
            if (!termId.HasValue)
                termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();

            var teacherSections = await _context.TeacherSections.Where(x => x.TeacherId == teacherId && x.Section.CourseTerm.TermId == termId)
                .Select(x => new
                {
                    id = x.Section.CourseTerm.Course.Id,
                    text = x.Section.CourseTerm.Course.Name
                })
                .Distinct()
                .ToListAsync();

            return teacherSections;
        }

        public async Task<List<CourseScheduleLoad>> GetCourseSectionsScheduleLoad(Guid termId, Guid? careerId, Guid? academicProgramId, Guid? curriculumId, int? academicYear)
        {
            var query = _context.Sections.Where(x => x.CourseTerm.TermId == termId).AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.CareerId == careerId);

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicProgramId == academicProgramId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (academicYear.HasValue)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.AcademicYear == academicYear));

            var pedagogical_hour_time_configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME).FirstOrDefaultAsync();

            if (pedagogical_hour_time_configuration is null)
            {
                pedagogical_hour_time_configuration = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME,
                    Value = ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES[ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME]
                };
            }

            var pedagogical_hour_time = Convert.ToDouble(pedagogical_hour_time_configuration.Value);

            var data = await query
                .Select(x => new
                {
                    AcademicYearNumber = curriculumId.HasValue && curriculumId != Guid.Empty ? x.CourseTerm.Course.AcademicYearCourses.Where(x => x.CurriculumId == curriculumId).Select(x => x.AcademicYear).FirstOrDefault() : x.CourseTerm.Course.AcademicYearCourses.Select(x => x.AcademicYear).FirstOrDefault(),
                    AcademicYear = curriculumId.HasValue && curriculumId != Guid.Empty ? x.CourseTerm.Course.AcademicYearCourses.Where(x => x.CurriculumId == curriculumId).Select(x => ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.AcademicYear]).FirstOrDefault() : x.CourseTerm.Course.AcademicYearCourses.Select(x => ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.AcademicYear]).FirstOrDefault(),
                    CurriculumId = curriculumId.HasValue && curriculumId != Guid.Empty ? curriculumId : x.CourseTerm.Course.AcademicYearCourses.Select(x => x.CurriculumId).FirstOrDefault(),
                    Curriculum = curriculumId.HasValue && curriculumId != Guid.Empty ? x.CourseTerm.Course.AcademicYearCourses.Where(x => x.CurriculumId == curriculumId).Select(x => x.Curriculum.Code).FirstOrDefault() : x.CourseTerm.Course.AcademicYearCourses.Select(x => x.Curriculum.Code).FirstOrDefault(),
                    Course = x.CourseTerm.Course.Name,
                    CourseCode = x.CourseTerm.Course.Code,
                    EnrolledStudents = x.StudentSections.Count(),
                    Section = x.Code,

                    x.CourseTerm.Course.TheoreticalHours,
                    x.CourseTerm.Course.PracticalHours,
                    x.CourseTerm.Course.SeminarHours,
                    x.CourseTerm.Course.VirtualHours,

                    ClassSchedules = x.ClassSchedules.ToList()
                })
                .ToListAsync();

            var result = data
                .Select(x => new CourseScheduleLoad
                {
                    AcademicYearNumber = x.AcademicYearNumber,
                    Curriculum = x.Curriculum,
                    CurriculumId = x.CurriculumId,
                    AcademicYear = x.AcademicYear,
                    Course = x.Course,
                    CourseCode = x.CourseCode,
                    EnrolledStudents = x.EnrolledStudents,
                    Section = x.Section,

                    TheoreticalHours = x.TheoreticalHours,
                    PracticalHours = x.PracticalHours,
                    SeminarHours = x.SeminarHours,
                    VirtualHours = x.VirtualHours,

                    AssignedTheoreticalHours = Math.Round((x.ClassSchedules.Where(y => y.SessionType == ConstantHelpers.SESSION_TYPE.THEORY).Sum(x => x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalMinutes) / pedagogical_hour_time), 1, MidpointRounding.AwayFromZero),
                    AssignedPracticalHours = Math.Round((x.ClassSchedules.Where(y => y.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE).Sum(x => x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalMinutes) / pedagogical_hour_time), 1, MidpointRounding.AwayFromZero),
                    AssignedSeminarHours = Math.Round((x.ClassSchedules.Where(y => y.SessionType == ConstantHelpers.SESSION_TYPE.SEMINAR).Sum(x => x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalMinutes) / pedagogical_hour_time), 1, MidpointRounding.AwayFromZero),
                    AssignedVirtualHours = Math.Round((x.ClassSchedules.Where(y => y.SessionType == ConstantHelpers.SESSION_TYPE.VIRTUAL).Sum(x => x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalMinutes) / pedagogical_hour_time), 1, MidpointRounding.AwayFromZero),

                    Details = x.ClassSchedules.Select(y => new CourseScheduleLoadDetail
                    {
                        EndTime = y.EndTime.ToLocalDateTimeFormatUtc(),
                        StartTime = y.StartTime.ToLocalDateTimeFormatUtc(),
                        WeekDay = y.WeekDay
                    })
                    .ToList()
                })
                .ToList();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCurriculumsByCourseDatatable(DataTablesStructs.SentParameters parameters, Guid courseId)
        {
            Expression<Func<Curriculum, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Year + x.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.Code;
                    break;
            }

            var query = _context.Curriculums.Where(x => x.IsActive && x.AcademicYearCourses.Any(y => y.CourseId == courseId)).AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    career = x.Career.Name
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> AnyCourseTerm(Guid courseId)
            => await _context.CourseTerms.Where(x => x.CourseId == courseId).AnyAsync();
    }
}