using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.TeacherSection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class TeacherSectionRepository : Repository<TeacherSection>, ITeacherSectionRepository
    {
        public TeacherSectionRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<TeacherSection>> GetAllBySection(Guid sectionId) => await _context.TeacherSections
            .Where(x => x.SectionId == sectionId).Select(x => new TeacherSection
            {
                Id = x.Id,
                SectionId = x.SectionId,
                TeacherId = x.TeacherId,
                IsPrincipal = x.IsPrincipal,
                Teacher = new Teacher
                {
                    User = new ApplicationUser
                    {
                        Id = x.Teacher.User.Id,
                        Name = x.Teacher.User.Name,
                        MaternalSurname = x.Teacher.User.MaternalSurname,
                        PaternalSurname = x.Teacher.User.PaternalSurname,
                        FullName = x.Teacher.User.FullName
                    }
                }
            }).ToListAsync();

        public async Task UpdateMainTeacher(Guid teacherSectionId, Guid sectionId)
        {
            await _context.TeacherSections.Where(x => x.SectionId == sectionId)
                .ForEachAsync(x => x.IsPrincipal = x.Id == teacherSectionId);
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteById(Guid teacherSectionId)
        {
            var teacherSection = await Get(teacherSectionId);
            var teacherSchedules = await _context.TeacherSchedules.Where(x =>
                    x.TeacherId == teacherSection.TeacherId && x.ClassSchedule.SectionId == teacherSection.SectionId)
                .ToListAsync();
            _context.TeacherSchedules.RemoveRange(teacherSchedules);
            _context.TeacherSections.Remove(teacherSection);
            await _context.SaveChangesAsync();
        }

        public async Task<object> GetAllAsModelAByTeacherId(string teacherId, Guid? termId = null)
        {
            var subq = _context.TeacherSections.AsNoTracking();

            if (termId.HasValue && termId != Guid.Empty)
                subq = subq.Where(x => x.Section.CourseTerm.TermId == termId);

            var query = await subq
            .Include(x => x.Section.CourseTerm.Course)
            .Where(x => x.TeacherId == teacherId)
            .Select(x => x.Section)
            .Select(x => new
            {
                x.Id,
                x.Code,
                Course = x.CourseTerm.Course.FullName,
                FullName = $"{x.CourseTerm.Course.FullName} - Sección: {x.Code}",
                x.CourseTermId,
                activities = _context.UnitActivities.Include(ua => ua.CourseUnit.CourseSyllabus).Where(ua => ua.CourseUnit.CourseSyllabus.CourseId == x.CourseTerm.CourseId && ua.CourseUnit.CourseSyllabus.TermId == termId).ToList(),
                classes = _context.Classes.Where(y => y.SectionId == x.Id).Select(y => new
                {
                    y.UnitActivityId,
                    y.IsDictated,
                    y.DictatedDate
                }).ToList(),
                teachers = x.TeacherSections.Count() > 0 ? String.Join(";", x.TeacherSections.Select(y => y.Teacher.User.FullName).ToList()) : "--"
            }).ToListAsync();

            var result = query
                .Select(x => new
                {
                    x.Id,
                    x.Course,
                    x.FullName,
                    cid = x.CourseTermId,
                    x.Code,
                    x.CourseTermId,
                    x.teachers,
                    progress = x.activities.Count() < 1 ? "0%" : $"{Math.Round((x.activities.Count(y => x.classes.Any(z => z.UnitActivityId == y.Id)) * 100f) / x.activities.Count(),2,MidpointRounding.AwayFromZero).ToString()}%"
                }).ToList();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var surveys = _context.SurveyUsers.AsQueryable();
            var query = _context.TeacherSections
                                .Where(x => surveys.Any(y => y.SectionId == x.SectionId))
                                .AsQueryable();

            if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(x => x.Section.CourseTerm.Course.Career.Faculty.DeanId == userId || x.Section.CourseTerm.Course.Career.Faculty.SecretaryId == userId);
                }
            }
            if (facultyId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);

            if (careerId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);

            var pagedList = query
                        .Select(x => new
                        {
                            id = x.SectionId,
                            code = x.Section.Code,
                            course = x.Section.CourseTerm.Course.Name,
                            term = x.Section.CourseTerm.Term.Name
                        });

            return await pagedList.ToDataTables<object>(sentParameters);
        }

        public async Task<IEnumerable<TeacherSectionTemplateZ>> GetReportDatatable(Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var surveys = _context.SurveyUsers.AsQueryable();
            var query = _context.TeacherSections
                                .Where(x => surveys.Any(y => y.SectionId == x.SectionId))
                                .AsQueryable();

            if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(x => x.Section.CourseTerm.Course.Career.Faculty.DeanId == userId || x.Section.CourseTerm.Course.Career.Faculty.SecretaryId == userId);
                }
            }

            if (facultyId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);

            if (careerId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);

            var pagedList = await query
                        .Select(x => new TeacherSectionTemplateZ
                        {
                            Id = x.SectionId,
                            Code = x.Section.Code,
                            Course = x.Section.CourseTerm.Course.Name,
                            Term = x.Section.CourseTerm.Term.Name
                        }).ToListAsync();

            return pagedList;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsWithTermActiveDatatable(DataTablesStructs.SentParameters sentParameters, Guid facultyId, Guid careerId, Guid courseId)
        {
            Expression<Func<TeacherSection, dynamic>> orderByPredicate = null;

            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Section.Code;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Section.StudentSections.Count;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Teacher.User.FullName;
                    break;
            }

            var studentSections = _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == term.Id)
                .Select(x => x.SectionId)
                .ToHashSet();

            var query = _context.TeacherSections
                .Where(x => x.Section.CourseTerm.TermId == term.Id
                && studentSections.Contains(x.SectionId))
                .AsNoTracking();

            if (facultyId != Guid.Empty) query = query.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);

            if (careerId != Guid.Empty) query = query.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);

            if (courseId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.CourseId == courseId);

            var recordsFiltered = await query.CountAsync();


            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    courseCode = x.Section.CourseTerm.Course.Code,
                    course = x.Section.CourseTerm.Course.Name,
                    code = x.Section.Code,
                    teacherName = x.Teacher.User.FullName,
                    studentsCount = studentSections.Count(y => y == x.SectionId)
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

        public async Task<IEnumerable<TeacherSectionTemplateC>> GetAllAsModelCByTermId(Guid termId, Guid? careerId = null, string coordinatorId = null, string teacherId = null, ClaimsPrincipal user = null, Guid? academicDepartmentId = null, Guid? curriculumId = null)
        {
            var ttt = await _context.TeacherSchedules
                .Select(
                    x => new
                    {
                        x.TeacherId,
                        x.ClassSchedule.SectionId,
                        x.ClassSchedule.EndTime,
                        x.ClassSchedule.StartTime,
                        x.ClassSchedule.SessionType,
                        x.ClassSchedule.WeekDay,
                        x.ClassSchedule.Classroom.Number,//1 teorico - 2 practico - 3 virtual - 4 seminario
                        x.ClassSchedule.Classroom.Description
                    }
                ).ToListAsync();

            var query = _context.TeacherSections
                .Where(x => x.Section.CourseTerm.TermId == termId && !x.Section.IsDirectedCourse);

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.TeacherId == teacherId && !x.Section.IsDirectedCourse);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.CareerId == careerId));

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId));

            if (!string.IsNullOrEmpty(coordinatorId))
            {
                query = query.Where(x =>
                    x.Section.CourseTerm.Course.Career.AcademicCoordinatorId == coordinatorId ||
                    x.Section.CourseTerm.Course.Career.CareerDirectorId == coordinatorId ||
                    x.Section.CourseTerm.Course.Career.AcademicDepartmentDirectorId == coordinatorId ||
                    x.Section.CourseTerm.Course.Career.AcademicSecretaryId == coordinatorId
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
                   x.Section.CourseTerm.Course.Career.AcademicCoordinatorId == userId ||
                   x.Section.CourseTerm.Course.Career.CareerDirectorId == userId ||
                   x.Section.CourseTerm.Course.Career.AcademicSecretaryId == userId
                   );
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    query = query.Where(x => x.Teacher.AcademicDepartment.AcademicDepartmentDirectorId == userId || x.Teacher.AcademicDepartment.AcademicDepartmentSecretaryId == userId);
                }
            }

            var confi = _context.Configurations.FirstOrDefault(x => x.Key == ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME);
            var pedagogicalhour = confi != null ? Convert.ToDouble(confi.Value) : 0;

            var dataDB = await query.Where(x => x.Teacher != null && x.Teacher.User != null).Include(x => x.Section.CourseTerm.Course.Area).Include(x => x.Teacher.User).Include(x => x.Teacher.TeacherDedication).Include(x => x.Teacher.AcademicDepartment).ToArrayAsync();

            var sections = dataDB
                .Select(x => new TeacherSectionTemplateC
                {
                    Teacher = x.Teacher.User.FullName,
                    Category = _context.WorkerLaborInformation.Include(y => y.WorkerLaborCategory).FirstOrDefault(y => y.UserId == x.Teacher.UserId)?.WorkerLaborCategory?.Name ?? "-",
                    Grade = x.Teacher?.TeacherDedication?.Name ?? "-",
                    Condition = _context.WorkerLaborInformation.Include(y => y.WorkerLaborCondition).FirstOrDefault(y => y.UserId == x.Teacher.UserId)?.WorkerLaborCondition?.Name ?? "-",
                    Course = x.Section.CourseTerm.Course.Code + " - " + x.Section.CourseTerm.Course.Name,
                    AcademicDepartment = x.Teacher?.AcademicDepartment?.Name,
                    CourseArea = x.Section.CourseTerm.Course.Area?.Name,
                    CareerCode = x.Section.CourseTerm.Course.Career?.Code,
                    CourseCode = x.Section.CourseTerm.Course.Code,
                    CourseName = x.Section.CourseTerm.Course.Name,
                    Credits = x.Section.CourseTerm.Course.Credits,
                    Cycle = ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS.ContainsKey(_context.AcademicYearCourses.Where(y => y.CourseId == x.Section.CourseTerm.CourseId).Select(y => y.AcademicYear).FirstOrDefault()) ? ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[_context.AcademicYearCourses.Where(y => y.CourseId == x.Section.CourseTerm.CourseId).Select(y => y.AcademicYear).FirstOrDefault()] : "0",
                    AcademicYear = _context.AcademicYearCourses.Where(y => y.CourseId == x.Section.CourseTerm.CourseId).Select(y => y.AcademicYear).FirstOrDefault(),
                    Group = x.Section.Code,
                    ClassRoomName = ttt.Where(y => y.TeacherId == x.TeacherId && y.SectionId == x.SectionId).Select(cs => cs.Description).FirstOrDefault(),
                    ClassRoom = ttt.Where(y => y.TeacherId == x.TeacherId && y.SectionId == x.SectionId).Select(cs => cs.Number).FirstOrDefault(),
                    HT = GetNumberFormat(ttt.Where(y => y.TeacherId == x.TeacherId && y.SectionId == x.SectionId && y.SessionType == ConstantHelpers.SESSION_TYPE.THEORY).Select(cs => cs.EndTime.ToLocalTimeSpanUtc().Subtract(cs.StartTime.ToLocalTimeSpanUtc()).TotalHours * 60 / pedagogicalhour).Sum()),
                    HP = GetNumberFormat(ttt.Where(y => y.TeacherId == x.TeacherId && y.SectionId == x.SectionId && y.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE).Select(cs => cs.EndTime.ToLocalTimeSpanUtc().Subtract(cs.StartTime.ToLocalTimeSpanUtc()).TotalHours * 60 / pedagogicalhour).Sum()),
                    HV = GetNumberFormat(ttt.Where(y => y.TeacherId == x.TeacherId && y.SectionId == x.SectionId && y.SessionType == ConstantHelpers.SESSION_TYPE.VIRTUAL).Select(cs => cs.EndTime.ToLocalTimeSpanUtc().Subtract(cs.StartTime.ToLocalTimeSpanUtc()).TotalHours * 60 / pedagogicalhour).Sum()),
                    HS = GetNumberFormat(ttt.Where(y => y.TeacherId == x.TeacherId && y.SectionId == x.SectionId && y.SessionType == ConstantHelpers.SESSION_TYPE.SEMINAR).Select(cs => cs.EndTime.ToLocalTimeSpanUtc().Subtract(cs.StartTime.ToLocalTimeSpanUtc()).TotalHours * 60 / pedagogicalhour).Sum()),
                    NumberStudents = _context.StudentSections.Where(y => y.SectionId == x.SectionId).Count(),
                    Section = x.Section.Code,
                    SessionTypes = ttt.Where(y => y.TeacherId == x.TeacherId && y.SectionId == x.SectionId)
                                     .Select(cs => new SessionTypesTemplate
                                     {
                                         WeekDay = cs.WeekDay,
                                         StartTime = cs.StartTime,
                                         EndTime = cs.EndTime,
                                         SessionType = cs.SessionType,
                                         Duration = GetNumberFormat(cs.EndTime.ToLocalTimeSpanUtc().Subtract(cs.StartTime.ToLocalTimeSpanUtc()).TotalHours * 60 / pedagogicalhour)
                                     }).ToList()
                })
                .OrderBy(x => x.AcademicYear)
                .ThenBy(x => x.CourseCode)
                .ThenBy(x => x.CourseName).ToList();


            sections.ForEach(x => x.TotalHours = x.HT + x.HP + x.HV + x.HS);

            return sections;
        }

        private double GetNumberFormat(double number)
        {
            if (number % 1 != 0)
                number = Math.Round(number, 1, MidpointRounding.AwayFromZero);

            return number;
        }

        public async Task<bool> AnyBySectionAndTeacher(Guid sectionId, string teacherId)
            => await _context.TeacherSections.AnyAsync(x => x.SectionId == sectionId && x.TeacherId == teacherId);

        public async Task<int> CountStudentsInSectionsWithTermActive(Guid facultyId, Guid careerId, Guid courseId)
        {
            var query = _context.TeacherSections
                             .Where(x => x.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE
                             && x.Section.StudentSections.Count > 0)
                             .AsQueryable();

            if (facultyId != Guid.Empty)
            {
                query = query.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);
            }
            if (careerId != Guid.Empty)
            {
                query = query.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);
            }
            if (courseId != Guid.Empty)
            {
                query = query.Where(x => x.Section.CourseTerm.CourseId == courseId);
            }
            var secciones = query.Include(x => x.Teacher).AsQueryable();

            var StudentsInSection = _context.StudentSections
                    .Where(x => secciones.Any(y => y.SectionId == x.SectionId)).AsQueryable();

            return await StudentsInSection.CountAsync();
        }

        public async Task<List<TeacherSection>> GetAllSectionsWithTermActiveWithIncludes(Guid facultyId, Guid careerId, Guid courseId)
        {
            var query = _context.TeacherSections
                                        .Where(x => x.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE
                                        && x.Section.StudentSections.Count > 0)
                                        .AsQueryable();

            if (facultyId != Guid.Empty)
            {
                query = query.Where(x => x.Section.CourseTerm.Course.Career.FacultyId == facultyId);
            }
            if (careerId != Guid.Empty)
            {
                query = query.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);
            }
            if (courseId != Guid.Empty)
            {
                query = query.Where(x => x.Section.CourseTerm.CourseId == courseId);
            }

            return await query.Include(x => x.Teacher).ToListAsync();
        }

        public async Task<object> GetSectionsByUser(string userId, string term)
        {
            var query = _context.TeacherSections
                .Include(x => x.Section.CourseTerm.Course)
                .Where(x => x.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                .Where(x => x.TeacherId == userId)
                .AsQueryable();

            foreach (var filtro in term.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Where(x => x.Section.Code.ToLower().StartsWith(filtro.ToLower()) || x.Section.CourseTerm.Course.Code.StartsWith(filtro.ToLower()) || x.Section.CourseTerm.Course.Name.StartsWith(filtro.ToLower()));

            var result = await query.Select(x => new
            {
                id = x.SectionId,
                text = $"{x.Section.CourseTerm.Course.FullName} / {x.Section.Code}",
            }).ToListAsync();

            return result;
        }

        public async Task<Section> GetTeacherSectionsWithTermAndCareer(Guid sectionId)
        {
            var query = await _context.TeacherSections
             .Include(x => x.Teacher.User)
             .Include(x => x.Section.CourseTerm.Term)
             .Include(x => x.Section.CourseTerm.Course.Career)
             .Include(x => x.Section.CourseTerm.Course.AcademicYearCourses)
             .FirstOrDefaultAsync(x => x.SectionId == sectionId);
            var section = query?.Section;
            return section;
        }

        public async Task<TeacherSection> GetTeacherSectionBySection(Guid sectionId)
        {
            var teacherSection = await _context.TeacherSections.Include(x => x.Teacher).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.SectionId == sectionId);

            return teacherSection;
        }

        public async Task<string> GetTeacherBySection(Guid sectionId)
        {
            var teacherSection = await _context.TeacherSections.Include(x => x.Teacher).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.SectionId == sectionId);

            return teacherSection.Teacher.User.FullName;
        }

        public async Task<object> GetAllAsModelDByTermIdAndTeacherId(Guid termId, string teacherId)
        {
            var tschedules = await _context.TeacherSchedules.Include(y => y.ClassSchedule.Section.CourseTerm.Course).Where(y => y.TeacherId == teacherId).ToListAsync();

            var _temp = await _context.TeacherSections
                                       .Include(x => x.Section.CourseTerm.Course.Career)
                                       .Include(x => x.Section.ClassSchedules)
                                       .ThenInclude(x => x.Classroom)
                                       .Where(x => x.Section.CourseTerm.Term.Id == termId && x.TeacherId == teacherId)
                                       .ToListAsync();

            var output = _temp
                        .Select(x => new
                        {
                            Course = $"{x.Section.CourseTerm.Course.Name} - {x.Section.CourseTerm.Course.Code}",
                            Career = x.Section.CourseTerm.Course.Career.Name,
                            ClassRoom = String.Join('/', x.Section.ClassSchedules.Select(y => y.Classroom.Description)),
                            HT = tschedules.Where(y => y.ClassSchedule.SessionType == 1 && y.ClassSchedule.SectionId == x.SectionId).Sum(y => DateTime.ParseExact(y.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(y.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalHours),
                            HP = tschedules.Where(y => y.ClassSchedule.SessionType == 2 && y.ClassSchedule.SectionId == x.SectionId).Sum(y => DateTime.ParseExact(y.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(y.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalHours),
                        })
                        .ToList();

            return output;
        }

        public async Task<object> GetAllAsSelect2ClientSide(Guid? sectionId = null)
        {
            var result = await _context
                   .TeacherSections
                   .Where(ts => ts.SectionId == sectionId)
                   .Select(ts => new
                   {
                       id = ts.TeacherId,
                       text = ts.Teacher.User.FullName
                   }).ToListAsync();

            return result;
        }

        public async Task<object> GetAllAsSelect2ClientSide2(Guid? courseId = null, Guid? termId = null)
        {
            var query = _context
                   .TeacherSections
                   .Where(x => x.Section.CourseTerm.CourseId == courseId)
                   .AsQueryable();

            if (termId.HasValue)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            var result = await query
                   .Select(t => new
                   {
                       id = t.Teacher.UserId,
                       text = t.Teacher.User.FullName
                   })
                   .Distinct()
                   .ToListAsync();



            return result;
        }
        public async Task<object> GetSectionTeachersJson(Guid sid)
        {
            var result = await _context
                .TeacherSections
                .Where(ts => ts.SectionId == sid)
                .Select(ts => new
                {
                    id = ts.TeacherId,
                    text = ts.Teacher.User.FullName
                }).ToListAsync();

            return result;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<double> GetTotalHoursSectionsByTeacherAndTermId(string teacherId, Guid termId)
        {
            var confi = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME).Select(x => x.Value).FirstOrDefaultAsync();
            Int32.TryParse(confi, out var pedagogical_hour_time);
            var teacherSections = await _context.TeacherSections.Where(x => x.TeacherId == teacherId && x.Section.CourseTerm.TermId == termId).Select(x => x.SectionId).ToListAsync();
            var classSchedules = await _context.TeacherSchedules.Where(x => x.TeacherId == teacherId && teacherSections.Contains(x.ClassSchedule.SectionId))
                .Select(x => new
                {
                    x.ClassSchedule.EndTime,
                    x.ClassSchedule.StartTime
                })
                .ToListAsync();

            var durations = classSchedules.Select(cs => (cs.EndTime.ToLocalTimeSpanUtc().Subtract(cs.StartTime.ToLocalTimeSpanUtc()).TotalMinutes) / pedagogical_hour_time).ToList();
            var directedCourseHours = await _context.DirectedCourses.Where(x => x.TeacherId == teacherId && x.TermId == termId).Select(x => (double)x.Course.TheoreticalHours).ToListAsync();

            var total = durations.Sum() + (directedCourseHours.Sum() / 2);

            var result = Math.Round(total, 1, MidpointRounding.AwayFromZero);
            return result;
        }
        //=> await _context.TeacherSections.Where(x => x.TeacherId == teacherId && x.Section.CourseTerm.TermId == termId).Select(x=>x.Section.CourseTerm.Course.TotalHours).SumAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherSectionByTeacherId(DataTablesStructs.SentParameters sentParameters, string teacherId, Guid termId, bool withDirectedCourse = false)
        {
            Expression<Func<TeacherSection, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Career.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.Section.Code); break;
                default:
                    orderByPredicate = ((x) => x.Id); break;
            }

            int.TryParse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME), out var pedagogical_hour_time);

            var query = _context.TeacherSections.Where(x => x.TeacherId == teacherId && x.Section.CourseTerm.TermId == termId)
                .AsQueryable();

            if (!withDirectedCourse)
                query = query.Where(x => !x.Section.IsDirectedCourse);

            var coursesId = await query.Select(x => x.Section.CourseTerm.CourseId).ToListAsync();

            var academicYearCourses = await _context.AcademicYearCourses.Where(x => coursesId.Contains(x.CourseId)).ToListAsync();

            int recordsFiltered = await query.CountAsync();
            var dataDB = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      courseId = x.Section.CourseTerm.CourseId,
                      course = $"{x.Section.CourseTerm.Course.Code}-{x.Section.CourseTerm.Course.Name}",
                      section = x.Section.Code,
                      x.IsPrincipal,
                      schedules = x.Section.ClassSchedules.Where(y => y.TeacherSchedules.Any(z => z.TeacherId == x.TeacherId)).ToList(),
                      //totalHours = x.Section.ClassSchedules.Where(y=>y.TeacherSchedules.Any(z=>z.TeacherId == x.TeacherId)).Sum(y=>y.EndTime.ToLocalTimeSpanUtc().Subtract(y.StartTime.ToLocalTimeSpanUtc()).TotalMinutes),
                      sectionId = x.Section.Id,
                      x.Section.IsDirectedCourse,
                      x.Section.CourseTermId,
                      isCoordinator = x.Section.CourseTerm.CoordinatorId == teacherId,
                      career = x.Section.CourseTerm.Course.Career.Name,
                      academicProgram = x.Section.CourseTerm.Course.AcademicProgramId.HasValue ? x.Section.CourseTerm.Course.AcademicProgram.Name : string.Empty,
                      vacancies = x.Section.IsDirectedCourse ? "-" : x.Section.Vacancies.ToString()
                  })
                  .ToListAsync();

            var data = dataDB
                  .Select(x => new
                  {
                      x.Id,
                      x.course,
                      x.section,
                      TotalHours = x.schedules.Sum(y => Math.Round((y.EndTime.ToLocalTimeSpanUtc().Subtract(y.StartTime.ToLocalTimeSpanUtc()).TotalMinutes / pedagogical_hour_time), 2, MidpointRounding.AwayFromZero)),
                      x.sectionId,
                      x.IsPrincipal,
                      x.vacancies,
                      x.CourseTermId,
                      x.career,
                      x.academicProgram,
                      x.IsDirectedCourse,
                      x.isCoordinator,
                      cycles = string.Join(", ", academicYearCourses.Where(y => y.CourseId == x.courseId)
                      .Select(x => ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS.ContainsKey(x.AcademicYear) ? ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.AcademicYear] : "-").ToList().Distinct())
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

        public async Task<TeacherSection> GetByTeacherAndSectionId(string teacherId, Guid sectionId)
            => await _context.TeacherSections.Where(x => x.TeacherId == teacherId && x.SectionId == sectionId).FirstOrDefaultAsync();

        public async Task<List<TeacherSection>> GetListBySectionId(Guid sectionId)
            => await _context.TeacherSections
            .Where(x => x.SectionId == sectionId)
            .OrderByDescending(x => x.CreatedAt)
            .Include(x => x.Teacher)
            .ThenInclude(x => x.User)
            .ToListAsync();

        public async Task<IEnumerable<TeacherSection>> GetTeacherSectionsByTermIdAndCourseId(Guid termId, Guid courseId)
            => await _context.TeacherSections.Include(x => x.Teacher.User.WorkerLaborInformation.WorkerLaborCondition).Include(x => x.Teacher.User.WorkerProfessionalTitles).Where(x => x.Section.CourseTerm.TermId == termId && x.Section.CourseTerm.CourseId == courseId).ToArrayAsync();

        public async Task<Select2Structs.ResponseParameters> GetTeachersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? termId = null, Guid? careerId = null)
        {
            var query = _context.Teachers.AsNoTracking();

            var teacherSections = _context.TeacherSections.AsNoTracking();

            if (termId != null)
                teacherSections = teacherSections.Where(x => x.Section.CourseTerm.TermId == termId);

            if (careerId.HasValue)
                teacherSections = teacherSections.Where(x => x.Section.CourseTerm.Course.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    teacherSections = teacherSections.Where(x => EF.Functions.Contains(x.Teacher.User.FullName, searchValue));
                }
                else
                    teacherSections = teacherSections.Where(x => x.Teacher.User.FullName.Contains(searchValue));
            }

            query = query.Where(x => teacherSections.Any(y => y.TeacherId == x.UserId));

            return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.UserId,
                Text = x.User.FullName
            }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);

        }

        public async Task<object> GetTeacherCoursesSelect2(string teacherId, bool showDirectedCourses = false)
        {
            var termId = await _context.Terms
                .Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                .Select(x => x.Id).FirstOrDefaultAsync();

            var query = _context.Sections
                .Where(x => x.CourseTerm.TermId == termId && x.TeacherSections.Any(ts => ts.TeacherId == teacherId))
                .AsNoTracking();

            if (!showDirectedCourses) query = query.Where(x => !x.IsDirectedCourse);

            var sections = await query
                .OrderBy(x => x.CourseTerm.Course.Code)
                .ThenBy(x => x.CourseTerm.Course.Name)
                .Select(x => new
                {
                    id = x.CourseTerm.Id,
                    text = $"{x.CourseTerm.Course.Code}-{x.CourseTerm.Course.Name}"
                })
                .ToListAsync();

            return sections.Distinct().ToList();
        }

        public async Task<object> GetTeacherCourseSectionsSelect2(string teacherId, Guid courseTermId, bool showDirectedCourses = false)
        {
            var query = _context.Sections
                .Where(x => x.CourseTermId == courseTermId && x.TeacherSections.Any(ts => ts.TeacherId == teacherId))
                .AsNoTracking();

            if (!showDirectedCourses) query = query.Where(x => !x.IsDirectedCourse);

            var sections = await query
                .OrderBy(x => x.Code)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.Code}"
                })
                .ToListAsync();

            return sections;
        }

        public async Task<List<ConsolidatedAcademicLoadReport>> GetConsolidatedAcademicLoadReport(Guid termId, Guid? academicDepartmentId, bool viewAll)
        {
            var teacher = _context.Teachers.AsNoTracking();

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                teacher = teacher.Where(x => x.AcademicDepartmentId == academicDepartmentId);

            teacher = teacher.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == termId));

            if (!viewAll)
                teacher = teacher.Where(x => x.TeacherSchedules.Any(y => y.ClassSchedule.Section.CourseTerm.TermId == termId) || x.TeacherSections.Any(y => y.Section.CourseTerm.TermId == termId && y.Section.IsDirectedCourse));

            var model = await teacher
                .Select(x => new ConsolidatedAcademicLoadReport
                {
                    TeacherId = x.UserId,
                    Teacher = x.User.FullName,
                    AcademicDepartment = x.AcademicDepartment.Name ?? "Sin Asignar",
                    Details = x.TeacherSections.Where(y => y.Section.CourseTerm.TermId == termId)
                    .Select(y => new ConsolidatedAcademicLoadDetailsReport
                    {
                        Course = $"{y.Section.CourseTerm.Course.Code}-{y.Section.CourseTerm.Course.Name}",
                        Section = y.Section.Code,
                        Monday = string.Join(", ", y.Section.ClassSchedules.Where(y => y.TeacherSchedules.Any(z => z.TeacherId == x.UserId) && y.WeekDay == ConstantHelpers.WEEKDAY.MONDAY).Select(z => $"{z.StartTime.ToLocalDateTimeFormatUtc()}-{z.EndTime.ToLocalDateTimeFormatUtc()}").ToList()),
                        Tuesday = string.Join(", ", y.Section.ClassSchedules.Where(y => y.TeacherSchedules.Any(z => z.TeacherId == x.UserId) && y.WeekDay == ConstantHelpers.WEEKDAY.TUESDAY).Select(z => $"{z.StartTime.ToLocalDateTimeFormatUtc()}-{z.EndTime.ToLocalDateTimeFormatUtc()}").ToList()),
                        Wednesday = string.Join(", ", y.Section.ClassSchedules.Where(y => y.TeacherSchedules.Any(z => z.TeacherId == x.UserId) && y.WeekDay == ConstantHelpers.WEEKDAY.WEDNESDAY).Select(z => $"{z.StartTime.ToLocalDateTimeFormatUtc()}-{z.EndTime.ToLocalDateTimeFormatUtc()}").ToList()),
                        Thursday = string.Join(", ", y.Section.ClassSchedules.Where(y => y.TeacherSchedules.Any(z => z.TeacherId == x.UserId) && y.WeekDay == ConstantHelpers.WEEKDAY.THURSDAY).Select(z => $"{z.StartTime.ToLocalDateTimeFormatUtc()}-{z.EndTime.ToLocalDateTimeFormatUtc()}").ToList()),
                        Friday = string.Join(", ", y.Section.ClassSchedules.Where(y => y.TeacherSchedules.Any(z => z.TeacherId == x.UserId) && y.WeekDay == ConstantHelpers.WEEKDAY.FRIDAY).Select(z => $"{z.StartTime.ToLocalDateTimeFormatUtc()}-{z.EndTime.ToLocalDateTimeFormatUtc()}").ToList()),
                        Staurday = string.Join(", ", y.Section.ClassSchedules.Where(y => y.TeacherSchedules.Any(z => z.TeacherId == x.UserId) && y.WeekDay == ConstantHelpers.WEEKDAY.SATURDAY).Select(z => $"{z.StartTime.ToLocalDateTimeFormatUtc()}-{z.EndTime.ToLocalDateTimeFormatUtc()}").ToList())
                    })
                    .ToList()
                })
                .ToListAsync();

            return model;

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersByAcademicDepartmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? academicDepartmentId = null)
        {

            var query = _context.TeacherSections.AsQueryable();

            if (termId != null) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (academicDepartmentId != null) query = query.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);

            int recordsFiltered = await query
                .Select(x => new
                {
                    x.Section.CourseTerm.TermId,
                    x.Teacher.AcademicDepartmentId
                })
                .Distinct()
                .CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.TeacherId,
                    x.Section.CourseTerm.TermId,
                    TermName = x.Section.CourseTerm.Term.Name,
                    TermYear = x.Section.CourseTerm.Term.Year,
                    TermNumber = x.Section.CourseTerm.Term.Number,
                    x.Teacher.AcademicDepartmentId,
                    AcademicDepartment = x.Teacher.AcademicDepartment.Name,
                })
                .Distinct()
                .GroupBy(x => new { x.TermId, x.TermName, x.TermYear, x.TermNumber, x.AcademicDepartmentId, x.AcademicDepartment })
                .OrderByDescending(x => x.Key.TermYear)
                .ThenByDescending(x => x.Key.TermNumber)
                .Select(x => new
                {
                    x.Key.TermName,
                    Teachers = x.Count(),
                    x.Key.AcademicDepartment
                })
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

        public async Task<object> GetTeachersByAcademicDepartmentChart(Guid? termId = null, Guid? academicDepartmentId = null)
        {
            var query = _context.TeacherSections.AsQueryable();

            if (termId != null) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (academicDepartmentId != null) query = query.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);

            var data = await _context.AcademicDepartments
                .Select(x => new
                {
                    x.Name,
                    Accepted = query.Where(y => y.Teacher.AcademicDepartmentId == x.Id).Select(y => y.TeacherId).Distinct().Count()
                }).ToListAsync();


            var result = new
            {
                categories = data.Select(x => x.Name).ToList(),
                data = data.Select(x => x.Accepted).ToList()
            };

            return result;
        }

        public async Task<int> GetMagisterByTermId(Guid? termId = null, ClaimsPrincipal user = null)
        {
            var query = _context.TeacherSections.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Teacher.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId != null) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            var data = await query.Select(x => x.Teacher.UserId).Distinct().ToListAsync();

            //Magisters
            var magisters = await _context.WorkerMasterDegrees
                .Select(x => x.UserId)
                .Distinct()
                .ToListAsync();

            //cuantos son magisters
            var count = data.Where(x => magisters.Any(y => y == x)).Count();

            return count;
        }

        public async Task<int> GetDoctorByTermId(Guid? termId = null, ClaimsPrincipal user = null)
        {
            var query = _context.TeacherSections.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Teacher.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId != null) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            var data = await query.Select(x => x.Teacher.UserId).Distinct().ToListAsync();

            //Doctors
            var doctors = await _context.WorkerDoctoralDegrees
                .Select(x => x.UserId)
                .Distinct()
                .ToListAsync();

            //cuantos son doctores
            var count = data.Where(x => doctors.Any(y => y == x)).Count();

            return count;
        }
    }
}