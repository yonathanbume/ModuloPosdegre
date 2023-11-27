using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassSchedule;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Section = AKDEMIC.ENTITIES.Models.Enrollment.Section;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class ClassScheduleRepository : Repository<ClassSchedule>, IClassScheduleRepository
    {
        public ClassScheduleRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<ClassSchedule>> GetAllBySection(Guid sectionId)
        {
            var result = await _context.ClassSchedules.Where(x => x.SectionId == sectionId)
                           .Select(x => new ClassSchedule
                           {
                               Id = x.Id,
                               WeekDay = x.WeekDay,
                               StartTime = x.StartTime,
                               EndTime = x.EndTime,
                               TeacherSchedules = x.TeacherSchedules.
                               Select(y => new TeacherSchedule
                               {
                                   Teacher = new ENTITIES.Models.Generals.Teacher
                                   {
                                       User = new ENTITIES.Models.Generals.ApplicationUser
                                       {
                                           FullName = y.Teacher.User.FullName
                                       }
                                   }
                               }).ToList(),
                               TeacherNames = x.TeacherSchedules.Select(ts => ts.Teacher.User.FullName),
                               ClassroomId = x.ClassroomId,
                               SectionGroupId = x.SectionGroupId,
                               SectionGroup = x.SectionGroup != null ?  new SectionGroup
                               {
                                   Id = x.SectionGroup.Id,
                                   Code = x.SectionGroup.Code,
                                   Description = x.SectionGroup.Description
                               } : null,
                               SectionId = x.SectionId,
                               Classroom =  x.Classroom != null ? new Classroom
                               {
                                   Description = x.Classroom.Description
                               } : null,
                               SessionType = x.SessionType
                           }).ToListAsync();

            result.ForEach(x => x.TeacherNames = x.TeacherSchedules.Select(ts => ts.Teacher.User.FullName));

            return result;
        }

        public async Task<IEnumerable<ClassSchedule>> GetAllByStudentAndTerm(Guid studentId, Guid termId)
            => await _context.ClassSchedules
                .Include(x => x.Section.CourseTerm.Course)
                .Include(x => x.Classroom)
                .Where(cs => cs.Section.StudentSections.Any(x => x.StudentId.Equals(studentId)) && cs.Section.CourseTerm.TermId.Equals(termId))
                .ToListAsync();

        public async Task<IEnumerable<ClassSchedule>> GetAllByTeacherAndTerm(string teacherId, Guid termId)
            => await _context.ClassSchedules
                .Include(x => x.Section.CourseTerm.Course)
                .Include(x => x.Classroom)
                .Where(cs => cs.TeacherSchedules.Any(ts => ts.TeacherId == teacherId) && cs.Section.CourseTerm.TermId == termId)
                .ToListAsync();

        public async Task<ClassSchedule> GetWithTeacherSchedules(Guid id)
            => await _context.ClassSchedules
                .Include(x => x.Classroom)
                .Include(x => x.Section.CourseTerm.Course)
                .Include(x => x.TeacherSchedules)
                .ThenInclude(x => x.Teacher.User)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<ClassScheduleTemplate>> GetClassSchedulesByStudentIdAndTermId(Guid studentId, Guid termId)
        {
            var today = (int)DateTime.UtcNow.ToDefaultTimeZone().DayOfWeek;

            var result = await _context.ClassSchedules
            .Where(cs => cs.Section.StudentSections.Any(x => x.StudentId == studentId) && cs.Section.CourseTerm.TermId == termId)
            .Select(cs => new ClassScheduleTemplate
            {
                Id = cs.Id,
                Title = string.Format("{0}-{1} ({2})", cs.Section.CourseTerm.Course.Code, cs.Section.CourseTerm.Course.Name, cs.Section.Code),
                Description = cs.Classroom.Description,
                AllDay = false,
                Start = DateTime.UtcNow.ToDefaultTimeZone().Date.AddDays((cs.WeekDay + 1) - today).ToString("yyyy-MM-dd") + "T" + cs.StartTime.ToLocalDateTimeUtc().ToString("HH:mm:ss"),
                End = DateTime.UtcNow.ToDefaultTimeZone().Date.AddDays((cs.WeekDay + 1) - today).ToString("yyyy-MM-dd") + "T" + cs.EndTime.ToLocalDateTimeUtc().ToString("HH:mm:ss")
            }).ToListAsync();

            return result;
        }

        public async Task<object> GetSchedule(Guid studentId, Guid termId)
        {
            var date = DateTime.UtcNow.ToDefaultTimeZone();

            var today = date.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)date.DayOfWeek;

            var result = await _context.ClassSchedules
                .Where(cs => cs.Section.StudentSections.Any(x => x.StudentId == studentId && (!cs.SectionGroupId.HasValue || cs.SectionGroupId == x.SectionGroupId)) && cs.Section.CourseTerm.TermId == termId)
                .Select(cs => new
                {
                    id = cs.Id,
                    title = string.Format("{0}-{1} ({2})", cs.Section.CourseTerm.Course.Code, cs.Section.CourseTerm.Course.Name, cs.Section.Code),
                    description = cs.Classroom.Description,
                    allDay = false,
                    start = date.Date.AddDays((cs.WeekDay + 1) - today).ToString("yyyy-MM-dd") + "T" + cs.StartTime.ToLocalDateTimeUtc().ToString("HH:mm:ss"),
                    end = date.Date.AddDays((cs.WeekDay + 1) - today).ToString("yyyy-MM-dd") + "T" + cs.EndTime.ToLocalDateTimeUtc().ToString("HH:mm:ss")
                }).ToListAsync();

            return result;
        }

        public async Task<ClassScheduleReportTemplate> GetScheduleReport(Guid studentId, Guid termId)
        {
            var student = await _context.Students
                .Where(x => x.Id == studentId)
                .Select(x => new
                {
                    UserName = x.User.UserName,
                    FullName = x.User.FullName,
                    CareerName = x.Career.Name,
                    CurriculumCode = x.Curriculum.Code,
                })
                .FirstOrDefaultAsync();

            var term = await _context.Terms
                .Where(x => x.Id == termId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Year,
                    x.Number
                })
                .FirstOrDefaultAsync();

            var classSchedule = await _context.ClassSchedules
                .Where(x => x.Section.StudentSections.Any(y => y.StudentId == studentId && (!x.SectionGroupId.HasValue || x.SectionGroupId == y.SectionGroupId)) && x.Section.CourseTerm.TermId == termId)
                .Select( x => new SectionScheduleTemplate
                {
                    Code = x.Section.CourseTerm.Course.Code + " - " + x.Section.Code,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    TimeText = $"{x.StartTimeText} - {x.EndTimeText}",
                    WeekDay = x.WeekDay
                })
                .ToListAsync();

            var result = new ClassScheduleReportTemplate
            {
                UserName = student.UserName,
                FullName = student.FullName,
                CareerName = student.CareerName,
                CurriculumCode = student.CurriculumCode,
                TermName = term.Name,
                SectionSchedules = classSchedule
            };

            return result;
        }

        public async Task<List<ClassScheduleTemplate>> GetClassScheduleTemplateBySectionId(Guid sectionId)
        {
            var date = DateTime.UtcNow.ToDefaultTimeZone();
            var today = date.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)date.DayOfWeek;

            var result = await _context.ClassSchedules
                .Where(x => x.SectionId == sectionId)
                .Select(x => new ClassScheduleTemplate
                {
                    Id = x.Id,
                    Title = $"{x.Section.CourseTerm.Course.Code}-{x.Section.CourseTerm.Course.Name} ({x.Section.Code})",
                    Description = x.Classroom.Description,
                    AllDay = false,
                    Start = date.Date.AddDays((x.WeekDay + 1) - today).ToString("yyyy-MM-dd") + "T" + x.StartTime.ToLocalDateTimeUtc().ToString("HH:mm:ss"),
                    End = date.Date.AddDays((x.WeekDay + 1) - today).ToString("yyyy-MM-dd") + "T" + x.EndTime.ToLocalDateTimeUtc().ToString("HH:mm:ss")
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<ClassScheduleTemplateA>> GetAllAsModelA(Guid termId, string teacherId)
        {
            var classSchedule = await _context.ClassSchedules
                .Where(x => x.TeacherSchedules.Any(y => y.TeacherId == teacherId) && x.Section.CourseTerm.TermId == termId)
                .Select(
                    x => new ClassScheduleTemplateA
                    {
                        Code = x.Section.CourseTerm.Course.Code + " - " + x.Section.Code,
                        StartTime = x.StartTime,
                        EndTime = x.EndTime,
                        TimeText = $"{x.StartTimeText} - {x.EndTimeText}",
                        WeekDay = x.WeekDay,
                        SectionId = x.SectionId,
                        SessionType = x.SessionType,
                        SectionGroup = x.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE ? x.SectionGroup.Code : null,
                        SectionGroupStudentsCount = x.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE && x.SectionGroupId.HasValue ?
                        x.Section.StudentSections.Where(y=>y.SectionGroupId == x.SectionGroupId).Count() : 0
                    }
                )
                .ToListAsync();

            return classSchedule;
        }

        public async Task<ClassSchedule> GetFirstForSection(Guid id)
            => await _context.ClassSchedules
                .Include(x => x.Classroom.Building.Campus)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<ClassSchedule>> GetAllByClassroomAndTerm(Guid classroomId, Guid termId)
            => await _context.ClassSchedules
                .Include(x => x.Section.CourseTerm.Course)
                .Include(x => x.Classroom)
                .Include(x => x.TeacherSchedules)
                .ThenInclude(x => x.Teacher.User)
                .Where(cs => cs.ClassroomId == classroomId && cs.Section.CourseTerm.TermId == termId)
                .ToListAsync();

        public async Task<ClassSchedule> GetClassSchedulesBySectionId(Guid sectionId)
        {
            var Class = await _context.ClassSchedules.Include(x => x.Classroom.Building).FirstOrDefaultAsync(x => x.SectionId == sectionId);

            return Class;
        }

        public async Task<object> GetAsModelB(Guid id)
        {
            var result = await _context.ClassSchedules.Where(x => x.Id == id)
                .Select(cs => new
                {
                    id = cs.Id,
                    weekday = cs.WeekDay,
                    startTime = cs.StartTime.ToLocalDateTimeFormatUtc(),
                    endTime = cs.EndTime.ToLocalDateTimeFormatUtc(),
                    classroomid = cs.ClassroomId,
                    classroomDescription = cs.Classroom.Description,
                    sectionid = cs.SectionId,
                    sessionType = cs.SessionType,
                    teacher = cs.TeacherSchedules.Select(x => x.TeacherId),
                    sectionGroup = cs.SectionGroupId,
                    campusId = cs.Classroom.Building.CampusId,
                })
                .SingleOrDefaultAsync();

            return result;
        }

        private async Task<List<SectionClassSchedulesTemplate>> GetSectionClassSchedules(Guid sectionId, string search = null, string teacherId = null)
        {
            var confi = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME).Select(x => x.Value).FirstOrDefaultAsync();
            Int32.TryParse(confi, out var pedagogical_hour_time);

            var query = (from cs in _context.ClassSchedules
                         join s in _context.Sections on cs.SectionId equals s.Id
                         where s.Id.Equals(sectionId)
                         select new SectionClassSchedulesTemplate
                         {
                             Id = cs.Id,
                             Weekday = ConstantHelpers.WEEKDAY.VALUES[cs.WeekDay],
                             StartTime = cs.StartTime.ToLocalDateTimeFormatUtc(),
                             EndTime = cs.EndTime.ToLocalDateTimeFormatUtc(),
                             Classroom = cs.Classroom.Description,
                             SessionType = cs.SessionType,
                             HasAssigned = cs.TeacherSchedules.Any(y => y.TeacherId == teacherId),
                             Duration = (cs.EndTime.ToLocalTimeSpanUtc().Subtract(cs.StartTime.ToLocalTimeSpanUtc()).TotalMinutes) / pedagogical_hour_time,
                             Teacher = string.Join(System.Environment.NewLine, cs.TeacherSchedules.Select(x => x.Teacher.User.FullName))
                         }).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Teacher.Contains(search) || x.Classroom.Contains(search)).ToList();
            }

            return query;
        }
        public async Task<object> GetAllAsModelC(Guid sectionId, string teacherId = null)
        {
            var result = await GetSectionClassSchedules(sectionId, null, teacherId);
            return result;
        }

        public async Task<object> GetSectionClassSchedulesDatatable(DataTablesStructs.SentParameters sentParameters, Guid sectionId, string search = null)
        {
            var result = await GetSectionClassSchedules(sectionId, search);
            var recordsTotal = result.Count();
            return new DataTablesStructs.ReturnedData<SectionClassSchedulesTemplate>
            {
                Data = result,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<ClassSchedule> GetWithSectionCourseTermCourse(Guid classRoomId, int weekDay, TimeSpan timeStart, TimeSpan timeEnd, Guid termId, Guid? id = null)
        {
            var classSchedulesByClassroomId = await _context.ClassSchedules
                .Where(x => x.ClassroomId == classRoomId && x.Section.CourseTerm.TermId == termId && x.WeekDay == weekDay)
                .Select(x => new ClassSchedule
                {
                    Id = x.Id,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    WeekDay = x.WeekDay,
                    Section = new Section
                    {
                        Id = x.Section.Id,
                        Code = x.Section.Code,
                        CourseTerm = new CourseTerm
                        {
                            Id = x.Section.CourseTerm.Id,
                            Course = new Course
                            {
                                Id = x.Section.CourseTerm.CourseId,
                                Code = x.Section.CourseTerm.Course.Code,
                                Name = x.Section.CourseTerm.Course.Name
                            }
                        }
                    }
                })
                .ToListAsync();

            var classroomConflicted = classSchedulesByClassroomId
                .Where(x => x.Id != id &&
                ((x.StartTime.ToLocalTimeSpanUtc() <= timeStart.ToLocalTimeSpanUtc() &&
                x.EndTime.ToLocalTimeSpanUtc() > timeStart.ToLocalTimeSpanUtc()) || (x.StartTime.ToLocalTimeSpanUtc() < timeEnd.ToLocalTimeSpanUtc() &&
                x.EndTime.ToLocalTimeSpanUtc() >= timeEnd.ToLocalTimeSpanUtc()) || (timeStart.ToLocalTimeSpanUtc() <= x.StartTime.ToLocalTimeSpanUtc() &&
                timeEnd.ToLocalTimeSpanUtc() > x.StartTime.ToLocalTimeSpanUtc()) || (timeStart.ToLocalTimeSpanUtc() < x.EndTime.ToLocalTimeSpanUtc() &&
                timeEnd.ToLocalTimeSpanUtc() >= x.EndTime.ToLocalTimeSpanUtc()))
                ).FirstOrDefault();

            return classroomConflicted;

            //if (id.HasValue)
            //{
            //    var classroom = await _context.ClassSchedules
            //        .Include(x => x.Section.CourseTerm.Course)
            //        .Include(x => x.TeacherSchedules)
            //        .FirstOrDefaultAsync(x =>
            //            x.Id != id.Value && x.ClassroomId == classRoomId &&
            //            x.WeekDay == weekDay &&
            //            //IsInConflict(x.StartTime, x.EndTime, timeStart, timeEnd) 
            //            ((x.StartTime <= timeStart && x.EndTime > timeStart) || (x.StartTime < timeEnd && x.EndTime >= timeEnd) || (timeStart <= x.StartTime && timeEnd > x.StartTime) || (timeStart < x.EndTime && timeEnd >= x.EndTime))
            //            &&
            //            x.Section.CourseTerm.TermId == termId);

            //    return classroom;
            //}
            //else
            //{
            //    var classroom = await _context.ClassSchedules.Include(x => x.Section.CourseTerm.Course)
            //        .Include(x => x.Section.CourseTerm.Course)
            //        .Include(x => x.TeacherSchedules)
            //        .FirstOrDefaultAsync(x =>
            //            x.ClassroomId == classRoomId &&
            //            x.WeekDay == weekDay &&
            //            //IsInConflict(x.StartTime, x.EndTime, timeStart, timeEnd) 
            //            ((x.StartTime <= timeStart && x.EndTime > timeStart) || (x.StartTime < timeEnd && x.EndTime >= timeEnd) || (timeStart <= x.StartTime && timeEnd > x.StartTime) || (timeStart < x.EndTime && timeEnd >= x.EndTime))
            //            &&
            //            x.Section.CourseTerm.TermId == termId);

            //    return classroom;
            //}
        }

        public async Task<object> GetStudentScheduleEnrollment(Guid id, Guid? termId = null)
        {
            var result = await _context.ClassSchedules
                .Where(cs => cs.Section.CourseTerm.TermId == termId &&
                             cs.Section.StudentSections.Any(ss => ss.StudentId == id))
                .Select(cs => new
                {
                    title = cs.Section.CourseTerm.Course.Code + " - " + cs.Section.Code,
                    description = cs.Classroom.Description,
                    allDay = false,
                    start = cs.StartTime.ToLocalDateTimeUtc().ToString("HH:mm"),
                    end = cs.EndTime.ToLocalDateTimeUtc().ToString("HH:mm"),
                    dow = new[] { cs.WeekDay + 1 }
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<ClassSchedule>> GetStudentSchedules(Guid studentId, Guid? termId = null)
        {
            var studentSchedules = await _context.ClassSchedules.Where(cs => cs.Section.CourseTerm.TermId == termId && cs.Section.StudentSections.Any(ss => ss.StudentId == studentId)).ToListAsync();

            return studentSchedules;
        }

        public async Task<object> GetAllWithData(Guid termId, string userId)
        {
            var tmpEnrollments = await _context.TmpEnrollments
                .Where(x => x.Section.CourseTerm.TermId == termId && x.Student.UserId == userId)
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

            //var result = await _context.ClassSchedules
            //.Where(cs => cs.Section.CourseTerm.TermId == termId && cs.Section.TmpEnrollments.Any(ss => ss.Student.UserId == userId))
            //.Select(cs => new
            //{
            //    title = cs.Section.CourseTerm.Course.Code + " - " + cs.Section.Code,
            //    description = cs.Classroom.Description,
            //    allDay = false,
            //    start = cs.StartTime.ToLocalDateTimeUtc().ToString("HH:mm"),
            //    end = cs.EndTime.ToLocalDateTimeUtc().ToString("HH:mm"),
            //    dow = new[] { cs.WeekDay + 1 }
            //})
            //.ToListAsync();

            return result;
        }

        public async Task<object> GetStudentSectionsSchedule(Guid termId, string userId)
        {
            var studentSections = await _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == termId && x.Student.UserId == userId)
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

            var result = studentSections.SelectMany(x => x).ToList();

            //var result = await _context.ClassSchedules
            //.Where(cs => cs.Section.CourseTerm.TermId == termId && cs.Section.StudentSections.Any(ss => ss.Student.UserId == userId))
            //.Select(cs => new
            //{
            //    title = cs.Section.CourseTerm.Course.Code + " - " + cs.Section.Code,
            //    description = cs.Classroom.Description,
            //    allDay = false,
            //    start = cs.StartTime.ToLocalDateTimeUtc().ToString("HH:mm"),
            //    end = cs.EndTime.ToLocalDateTimeUtc().ToString("HH:mm"),
            //    dow = new[] { cs.WeekDay + 1 }
            //})
            //.ToListAsync();

            return result;
        }

        public async Task<List<ClassSchedule>> GetStudentSchedulesWithData(Guid termId, string userId)
        {
            var studentSchedules = await _context.ClassSchedules.Where(cs => cs.Section.CourseTerm.TermId == termId && cs.Section.TmpEnrollments.Any(ss => ss.Student.UserId == userId)).ToListAsync();

            return studentSchedules;
        }
        public async Task<object> GetAllByGroupId(Guid id)
        {
            var today = (int)DateTime.UtcNow.DayOfWeek;
            return await _context.ClassSchedules
                .Where(cs => cs.Section.GroupId == id)
                .Select(cs => new
                {
                    title = cs.Section.CourseTerm.Course.Code + " - " + cs.Section.Code,
                    description = cs.Classroom.Description,
                    allDay = false,
                    start = DateTime.Today.AddDays((cs.WeekDay + 1) - today).ToString("yyyy-MM-dd") + "T" + cs.StartTime.ToLocalDateTimeUtc().ToString("HH:mm:ss"),
                    end = DateTime.Today.AddDays((cs.WeekDay + 1) - today).ToString("yyyy-MM-dd") + "T" + cs.EndTime.ToLocalDateTimeUtc().ToString("HH:mm:ss")
                }).ToListAsync();
        }

        public async Task CreateSectionsJob(Term term, string userId)
        {
            var rnd = new Random();
            var courses = await _context.Courses.ToListAsync();
            var classroom = await _context.Classrooms.FirstOrDefaultAsync();

            var sections = new List<Section>();
            var classSchedules = new List<ClassSchedule>();

            var courseCount = 0;
            foreach (var course in courses)
            {
                var section1 = await _context.Sections.Where(x => x.CourseTerm.TermId == term.Id && x.CourseTerm.CourseId == course.Id).FirstOrDefaultAsync();

                if (section1 == null)
                {

                    var courseTerm = await _context.CourseTerms.Where(x => x.CourseId == course.Id && x.TermId == term.Id).FirstOrDefaultAsync();

                    if (courseTerm == null)
                    {
                        courseTerm = new CourseTerm
                        {
                            TermId = term.Id,
                            CourseId = course.Id,
                            CreatedAt = DateTime.UtcNow,
                            CoordinatorId = userId,
                            WeekHours = 6
                        };

                        await _context.CourseTerms.AddAsync(courseTerm);
                        await _context.SaveChangesAsync();
                    }

                    section1 = new Section
                    {
                        CourseTermId = courseTerm.Id,
                        Code = "A",
                        CreatedAt = DateTime.UtcNow,
                        GroupId = null,
                        StudentsCount = 40,
                        Vacancies = 40
                    };
                    sections.Add(section1);

                    var startHour1 = rnd.Next(12, 20);
                    var endHour1 = startHour1 + 1;

                    var classSchedule1 = new ClassSchedule
                    {
                        WeekDay = rnd.Next(0, 5),
                        CreatedAt = DateTime.UtcNow,
                        ClassroomId = classroom.Id,
                        StartTime = new TimeSpan(startHour1, 0, 0),
                        EndTime = new TimeSpan(endHour1, 0, 0),
                        Section = section1,
                        SessionType = CORE.Helpers.ConstantHelpers.SESSION_TYPE.THEORY
                    };
                    classSchedules.Add(classSchedule1);

                    /////

                    var section2 = new Section
                    {
                        CourseTermId = courseTerm.Id,
                        Code = "B",
                        CreatedAt = DateTime.UtcNow,
                        GroupId = null,
                        StudentsCount = 40,
                        Vacancies = 40
                    };
                    sections.Add(section2);

                    var startHour2 = rnd.Next(12, 20);
                    var endHour2 = startHour2 + 1;

                    var classSchedule2 = new ClassSchedule
                    {
                        WeekDay = rnd.Next(0, 5),
                        CreatedAt = DateTime.UtcNow,
                        ClassroomId = classroom.Id,
                        StartTime = new TimeSpan(startHour1, 0, 0),
                        EndTime = new TimeSpan(endHour1, 0, 0),
                        Section = section2,
                        SessionType = CORE.Helpers.ConstantHelpers.SESSION_TYPE.THEORY
                    };
                    classSchedules.Add(classSchedule2);
                }

                courseCount++;

                if (courseCount >= 100)
                {
                    await _context.Sections.AddRangeAsync(sections);
                    await _context.ClassSchedules.AddRangeAsync(classSchedules);

                    await _context.SaveChangesAsync();

                    sections.Clear();
                    classSchedules.Clear();
                    courseCount = 0;
                }
            }
            await _context.Sections.AddRangeAsync(sections);
            await _context.ClassSchedules.AddRangeAsync(classSchedules);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClassSchedule>> GetAllBySections(List<Guid> sectionsId)
        {
            var result = await _context.ClassSchedules.Where(x => sectionsId.Contains(x.SectionId))
                .Include(x => x.Classroom.Building.Campus)
                .ToArrayAsync();

            return result;
        }

        public async Task<UnassignedSchedulesReportTemplate> GetUnassignedSchedulesReportTemplate(Guid termId, Guid? careerId, Guid? curriculumId, ClaimsPrincipal user)
        {
            var model = new UnassignedSchedulesReportTemplate();

            var query = _context.Sections.Where(x => x.CourseTerm.TermId == termId).AsNoTracking();

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

            query = query.Where(x => x.ClassSchedules.Any(y => !y.TeacherSchedules.Any()));

            var sections = await query
                .Select(x => new UnassignedSchedulesSectionReportTemplate
                {
                    Career = x.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Career.Name).FirstOrDefault(),
                    Curriculum = x.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Code).FirstOrDefault(),
                    CurriculumId = x.CourseTerm.Course.AcademicYearCourses.Select(y => y.CurriculumId).FirstOrDefault(),
                    AcademicYear = x.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear).FirstOrDefault(),
                    CourseCode = x.CourseTerm.Course.Code,
                    Course = x.CourseTerm.Course.Name,
                    Section = x.Code,
                    Schedules = x.ClassSchedules.Where(y => !y.TeacherSchedules.Any()).Select(y => new ScheduleTemplate
                    {
                        EndTime = y.EndTime,
                        StartTime = y.StartTime,
                        SessionType = y.SessionType,
                        WeekDay = y.WeekDay
                    }).ToList()
                })
                .ToListAsync();

            sections = sections.OrderBy(x => x.Career).ThenBy(x => x.Curriculum).ToList();

            model.Sections = sections;

            return model;
        }

        public async Task CompleteClassesToActiveTerm()
        {
            var _classes = new List<Class>();

            var currentTerm = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
            var sections = await _context.Sections
                .Include(x => x.ClassSchedules)
                .Where(x => x.ClassSchedules.Any() &&
                x.CourseTerm.TermId == currentTerm.Id)
                .ToListAsync();

            var startClassDay = currentTerm.ClassStartDate.Date;/*.ToDefaultTimeZone();*/
            var endClassDay = currentTerm.ClassEndDate.Date;/*.ToDefaultTimeZone();*/

            var holidays = await _context.Holidays.Where(y => y.Date.Date >= startClassDay && y.Date.Date <= endClassDay).ToListAsync();

            foreach (var section in sections)
            {
                var classSchedules = section.ClassSchedules.OrderBy(x => x.WeekDay).ThenBy(x => x.StartTime.ToLocalTimeSpanUtc()).ToList();
                var classes = await _context.Classes.Where(x => x.SectionId == section.Id).OrderBy(x => x.WeekNumber).ThenBy(x => x.ClassNumber).ToListAsync();

                var lastClass = classes.OrderByDescending(x => x.EndTime).FirstOrDefault();

                var weekNumber = 1;
                var classNumber = 1;
                var guideNumber = 1;
                var tpmClassDay = startClassDay.Date;

                if (lastClass != null)
                {
                    weekNumber = lastClass.WeekNumber;
                    classNumber = lastClass.ClassNumber + 1;
                    guideNumber = ConstantHelpers.WEEKDAY.ENUM_TO_INT(lastClass.StartTime.ToDefaultTimeZone().Date.DayOfWeek) - ConstantHelpers.WEEKDAY.ENUM_TO_INT(startClassDay.Date.DayOfWeek) + 2;
                    tpmClassDay = lastClass.StartTime.ToDefaultTimeZone().Date.AddDays(1);
                }

                var weekDays = classSchedules.Select(x => ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay)).ToList();

                while (tpmClassDay <= endClassDay.Date)
                {
                    if (guideNumber == 8)
                    {
                        weekNumber++;
                        classNumber = 1;
                        guideNumber = 1;
                    }

                    if (weekDays.Contains(tpmClassDay.DayOfWeek))
                    {
                        var holidayTpm = holidays.Where(x => x.Date.Date == tpmClassDay.Date).FirstOrDefault();

                        if (holidayTpm == null || holidayTpm.NeedReschedule)
                        {
                            var classSchedulesTmp = classSchedules.Where(x => x.WeekDay == ConstantHelpers.WEEKDAY.ENUM_TO_INT(tpmClassDay.DayOfWeek)).ToList();
                            foreach (var item in classSchedulesTmp)
                            {
                                var startTime = item.StartTime.ToLocalTimeSpanUtc();
                                var endTime = item.EndTime.ToLocalTimeSpanUtc();

                                var start_day = new DateTime(tpmClassDay.Year, tpmClassDay.Month, tpmClassDay.Day, startTime.Hours, startTime.Minutes, 0);
                                var end_day = new DateTime(tpmClassDay.Year, tpmClassDay.Month, tpmClassDay.Day, endTime.Hours, endTime.Minutes, 0);

                                var _class = new Class
                                {
                                    ClassScheduleId = item.Id,
                                    IsDictated = false,
                                    SectionId = item.SectionId,
                                    ClassNumber = classNumber,
                                    StartTime = start_day.ToUtcDateTime(),
                                    EndTime = end_day.ToUtcDateTime(),
                                    WeekNumber = weekNumber,
                                    ClassroomId = item.ClassroomId,
                                    NeedReschedule = holidayTpm != null && holidayTpm.NeedReschedule
                                };

                                classNumber++;
                                _classes.Add(_class);
                            }
                        }
                    }

                    tpmClassDay = tpmClassDay.AddDays(1);
                    guideNumber++;
                }

            }

            await _context.Classes.AddRangeAsync(_classes);
            await _context.SaveChangesAsync();
        }

        //Create - Update - Delete

        public async Task<ResultTemplate> CreateClassSchedule(ValidateClassScheduleTemplate model)
        {
            var result = new ResultTemplate();

            var validateResult = await ValidateClassSchedule(model);

            if (!validateResult.Succeeded)
            {
                result.Message = validateResult.Message;
                return result;
            }

            #region Crear Horario

            var entity = new ClassSchedule
            {
                SectionId = model.SectionId,
                EndTime = model.EndTimeUTC,
                StartTime = model.StartTimeUTC,
                ClassroomId = model.ClassroomId,
                SessionType = model.SessionType,
                WeekDay = model.WeekDay
            };

            if (model.DividedByGroups && model.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE)
                entity.SectionGroupId = model.SectionGroupId;

            if (model.ValidateTeachers && model.Teachers != null)
            {
                entity.TeacherSchedules = model.Teachers.Select(y => new TeacherSchedule
                {
                    TeacherId = y
                }).ToList();
            }

            await _context.ClassSchedules.AddAsync(entity);
            await _context.SaveChangesAsync();
            await GenerateClasses(model.SectionId);

            #endregion

            result.Succeeded = true;
            return result;
        }

        public async Task<ResultTemplate> EditClassSchedule(ValidateClassScheduleTemplate model)
        {
            var result = new ResultTemplate();

            var entity = await _context.ClassSchedules.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            var allowTeacherTimeCrossingConfiguration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.ALLOW_TEACHER_TIME_CROSSING).FirstOrDefaultAsync();

            if (allowTeacherTimeCrossingConfiguration is null)
            {
                allowTeacherTimeCrossingConfiguration = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.TeacherManagement.ALLOW_TEACHER_TIME_CROSSING,
                    Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.ALLOW_TEACHER_TIME_CROSSING]
                };
            }

            bool.TryParse(allowTeacherTimeCrossingConfiguration.Value, out var allowTeacherTimeCrossing);

            #region Añadir o remover docentes sin cambiar datos del horario

            if (
                model.ValidateTeachers &&
                entity.WeekDay == model.WeekDay &&
                entity.StartTime == model.StartTimeUTC &&
                entity.EndTime == model.EndTimeUTC &&
                entity.SessionType == model.SessionType &&
                entity.ClassroomId == model.ClassroomId
                )
            {
                var termId = await _context.Sections.Where(x => x.Id == entity.SectionId).Select(x => x.CourseTerm.TermId).FirstOrDefaultAsync();

                if (model.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE)
                {
                    if (
                        (entity.SectionGroupId.HasValue && model.DividedByGroups && entity.SectionGroupId == model.SectionGroupId) ||
                        (!entity.SectionGroupId.HasValue && !model.DividedByGroups)
                        )
                    {

                        if (model.ValidateTeachers)
                        {
                            if (!allowTeacherTimeCrossing)
                            {
                                var teacherSchedulesValidate = await ValidateTeacherSchedules(termId, model);

                                if (!teacherSchedulesValidate.Succeeded)
                                    return teacherSchedulesValidate;

                            }

                            await AssignTeachers(entity.Id, model.Teachers);
                        }

                        await _context.SaveChangesAsync();
                        result.Succeeded = true;
                        return result;
                    }
                }
                else
                {
                    if (model.ValidateTeachers)
                    {
                        if (!allowTeacherTimeCrossing)
                        {
                            var teacherSchedulesValidate = await ValidateTeacherSchedules(termId, model);

                            if (!teacherSchedulesValidate.Succeeded)
                                return teacherSchedulesValidate;
                        }

                        await AssignTeachers(entity.Id, model.Teachers);
                    }

                    await _context.SaveChangesAsync();
                    result.Succeeded = true;
                    return result;
                }
            }

            #endregion

            var validateResult = await ValidateClassSchedule(model);

            if (!validateResult.Succeeded)
            {
                result.Message = validateResult.Message;
                return result;
            }

            #region Actualizar Horario

            entity.EndTime = model.EndTimeUTC;
            entity.StartTime = model.StartTimeUTC;
            entity.ClassroomId = model.ClassroomId;
            entity.SessionType = model.SessionType;
            entity.WeekDay = model.WeekDay;
            entity.SectionGroupId = null;

            if (model.DividedByGroups && model.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE)
                entity.SectionGroupId = model.SectionGroupId;

            if (model.ValidateTeachers)
            {
                await AssignTeachers(entity.Id, model.Teachers);
            }

            await _context.SaveChangesAsync();
            await GenerateClasses(model.SectionId);

            #endregion

            result.Succeeded = true;
            return result;
        }

        public async Task<ResultTemplate> DeleteClassSchedule(Guid classScheduleId)
        {
            var result = new ResultTemplate();

            var entity = await _context.ClassSchedules.Where(x => x.Id == classScheduleId).FirstOrDefaultAsync();
            var section = await _context.Sections.Where(x => x.Id == entity.SectionId).FirstOrDefaultAsync();
            var term = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).Select(x => x.Term).FirstOrDefaultAsync();

            if (term.Status == ConstantHelpers.TERM_STATES.FINISHED)
            {
                result.Message = "No puede eliminar un Horario de Clase para un Periodo Académico finalizado.";
                return result;
            }

            var hasClassesDictated = await _context.Classes.Where(x => x.SectionId == entity.SectionId && (x.IsDictated || x.ClassStudents.Any())).AnyAsync();

            if (hasClassesDictated)
            {
                result.Message = "Se encontraron clases dictadas.";
                return result;
            }

            var hasStudentAbsenceJustifications = await _context.StudentAbsenceJustifications.Where(x => x.ClassStudent.Class.SectionId == entity.SectionId).AnyAsync();

            if (hasStudentAbsenceJustifications)
            {
                result.Message = "Se encontraron justificaciones de inasistencia.";
                return result;
            }

            var teacherSchedules = await _context.TeacherSchedules.Where(x => x.ClassScheduleId == entity.Id).ToListAsync();
            var classes = await _context.Classes.Where(x => x.ClassScheduleId == entity.Id).ToListAsync();

            _context.TeacherSchedules.RemoveRange(teacherSchedules);
            _context.Classes.RemoveRange(classes);
            _context.ClassSchedules.Remove(entity);

            await _context.SaveChangesAsync();
            await GenerateClasses(entity.SectionId);

            result.Succeeded = true;
            return result;
        }

        private async Task<ResultTemplate> ValidateClassSchedule(ValidateClassScheduleTemplate model)
        {
            var result = new ResultTemplate();

            #region Variables de Configuración

            var pedagogicalHourTimeConfiguration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME).FirstOrDefaultAsync();

            if (pedagogicalHourTimeConfiguration is null)
            {
                pedagogicalHourTimeConfiguration = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME,
                    Value = ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES[ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME]
                };
            }

            var allowTeacherTimeCrossingConfiguration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.TeacherManagement.ALLOW_TEACHER_TIME_CROSSING).FirstOrDefaultAsync();

            if (allowTeacherTimeCrossingConfiguration is null)
            {
                allowTeacherTimeCrossingConfiguration = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.TeacherManagement.ALLOW_TEACHER_TIME_CROSSING,
                    Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.ALLOW_TEACHER_TIME_CROSSING]
                };
            }

            Int32.TryParse(pedagogicalHourTimeConfiguration.Value, out var pedagogical_hour_time);
            bool.TryParse(allowTeacherTimeCrossingConfiguration.Value, out var allowTeacherTimeCrossing);

            #endregion

            var localStartTime = model.StartTimeUTC.ToLocalTimeSpanUtc();
            var localEndTime = model.EndTimeUTC.ToLocalTimeSpanUtc();

            if (localStartTime >= localEndTime)
            {
                result.Message = "Rango de tiempo no válido";
                return result;
            }

            var hasClassesDictated = await _context.Classes.Where(x => x.SectionId == model.SectionId && (x.IsDictated || x.ClassStudents.Any())).AnyAsync();

            if (hasClassesDictated)
            {
                result.Message = "Se encontraron clases dictadas.";
                return result;
            }

            var hasStudentAbsenceJustifications = await _context.StudentAbsenceJustifications.Where(x => x.ClassStudent.Class.SectionId == model.SectionId).AnyAsync();

            if (hasStudentAbsenceJustifications)
            {
                result.Message = "Se encontraron justificaciones de inasistencia.";
                return result;
            }

            var section = await _context.Sections.Where(x => x.Id == model.SectionId).FirstOrDefaultAsync();
            var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();
            var course = await _context.Courses.Where(x => x.Id == courseTerm.CourseId).FirstOrDefaultAsync();
            var term = await _context.Terms.Where(x => x.Id == courseTerm.TermId).FirstOrDefaultAsync();

            if (term.Status == ConstantHelpers.TERM_STATES.FINISHED)
            {
                result.Message = "No puede crear un Horario de Clase para un Período Académico finalizado.";
                return result;
            }

            var classSchedules = await _context.ClassSchedules.Where(x => x.SectionId == model.SectionId).ToListAsync();
            var classroom = await _context.Classrooms.Where(x => x.Id == model.ClassroomId).FirstOrDefaultAsync();

            if (model.DividedByGroups && !model.SectionGroupId.HasValue && model.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE)
            {
                result.Message = "Es necesario seleccionar el sub-grupo a asignar";
                return result;
            }

            if (model.DividedByGroups && model.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE)
            {
                var schedulesGroup = classSchedules.Where(x => x.SectionGroupId == model.SectionGroupId).ToList();
                if (classSchedules.Any(x => x.Id != model.Id && (x.WeekDay == model.WeekDay && IsInConflict(x.StartTime.ToLocalTimeSpanUtc(), x.EndTime.ToLocalTimeSpanUtc(), localStartTime, localEndTime) && x.SectionGroupId == model.SectionGroupId)))
                {
                    result.Message = "La sección - sub grupo ya cuenta con un horario en las horas seleccionadas.";
                    return result;
                }
            }
            else
            {
                if (classSchedules.Where(x => x.Id != model.Id && !x.SectionGroupId.HasValue).Any(x => x.WeekDay == model.WeekDay && IsInConflict(x.StartTime.ToLocalTimeSpanUtc(), x.EndTime.ToLocalTimeSpanUtc(), localStartTime, localEndTime)))
                {
                    result.Message = "La sección ya cuenta con un horario en las horas seleccionadas.";
                    return result;
                }
            }

            if (classroom.Description.ToLower().Trim() != "sin asignar")
            {
                var classSchedulesByClassroomId = await _context.ClassSchedules
                    .Where(x => x.ClassroomId == classroom.Id && x.Section.CourseTerm.TermId == term.Id).Select(x => new
                    {
                        x.Id,
                        x.StartTime,
                        x.EndTime,
                        x.WeekDay,
                        section = x.Section.Code,
                        courseCode = x.Section.CourseTerm.Course.Code,
                        courseName = x.Section.CourseTerm.Course.Name,
                    })
                    .ToListAsync();

                var classroomConflicted = classSchedulesByClassroomId.Where(x => x.Id != model.Id && x.WeekDay == model.WeekDay && IsInConflict(x.StartTime.ToLocalTimeSpanUtc(), x.EndTime.ToLocalTimeSpanUtc(), localStartTime, localEndTime)).FirstOrDefault();

                if (classroomConflicted != null)
                {
                    result.Message = $"El aula seleccionada se encuentra ocupada en ese rango de horas por una clase de la sección {classroomConflicted.section} del curso {classroomConflicted.courseCode} - {classroomConflicted.courseName}.";
                    return result;
                }
            }

            switch (model.SessionType)
            {
                case ConstantHelpers.SESSION_TYPE.THEORY:
                    if (classSchedules.Where(x => x.Id != model.Id && x.SessionType == ConstantHelpers.SESSION_TYPE.THEORY)
                        .Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalMinutes) / pedagogical_hour_time) + ((localEndTime - localStartTime).TotalMinutes / pedagogical_hour_time) > course.TheoreticalHours)
                    {
                        result.Message = "Las horas dadas sobrepasan el límite de horas de teoría.";
                        return result;
                    }
                    break;

                case ConstantHelpers.SESSION_TYPE.PRACTICE:
                    if (model.DividedByGroups)
                    {
                        if (classSchedules.Where(x => x.Id != model.Id && x.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE && x.SectionGroupId == model.SectionGroupId)
                            .Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalMinutes) / pedagogical_hour_time) + ((localEndTime - localStartTime).TotalMinutes / pedagogical_hour_time) > course.PracticalHours)
                        {
                            result.Message = "Las horas dadas sobrepasan el límite de horas de práctica por subgrupo.";
                            return result;
                        }
                    }
                    else
                    {
                        if (classSchedules.Where(x => x.Id != model.Id && x.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE && !x.SectionGroupId.HasValue)
                            .Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalMinutes) / pedagogical_hour_time) + ((localEndTime - localStartTime).TotalMinutes / pedagogical_hour_time) > course.PracticalHours)
                        {
                            result.Message = "Las horas dadas sobrepasan el límite de horas de práctica.";
                            return result;
                        }
                    }
                    break;
                case ConstantHelpers.SESSION_TYPE.VIRTUAL:
                    if (classSchedules.Where(x => x.Id != model.Id && x.SessionType == ConstantHelpers.SESSION_TYPE.VIRTUAL)
                        .Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalMinutes) / pedagogical_hour_time) + ((localEndTime - localStartTime).TotalMinutes / pedagogical_hour_time) > course.VirtualHours)
                    {
                        result.Message = "Las horas dadas sobrepasan el límite de horas virtuales.";
                        return result;
                    }
                    break;
                case ConstantHelpers.SESSION_TYPE.SEMINAR:
                    if (classSchedules.Where(x => x.Id != model.Id && x.SessionType == ConstantHelpers.SESSION_TYPE.SEMINAR)
                        .Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalMinutes) / pedagogical_hour_time) + ((localEndTime - localStartTime).TotalMinutes / pedagogical_hour_time) > course.SeminarHours)
                    {
                        result.Message = "Las horas dadas sobrepasan el límite de horas de seminario.";
                        return result;
                    }
                    break;

                default:
                    result.Message = "No se encontró el tipo de sesión asignado";
                    return result;
            }

            if (model.ValidateTeachers)
            {
                if (!allowTeacherTimeCrossing)
                {
                    var teacherSchedulesValidate = await ValidateTeacherSchedules(term.Id, model);

                    if (!teacherSchedulesValidate.Succeeded)
                    {
                        return teacherSchedulesValidate;
                    }
                }
            }

            result.Succeeded = true;
            return result;
        }

        private bool IsInConflict(TimeSpan st1, TimeSpan et1, TimeSpan st2, TimeSpan et2)
        {
            return (st1 <= st2 && et1 > st2) || (st1 < et2 && et1 >= et2) || (st2 <= st1 && et2 > st1) || (st2 < et1 && et2 >= et1);
        }

        private async Task GenerateClasses(Guid sectionId)
        {
            var classes = await _context.Classes.Where(x => x.SectionId == sectionId || x.ClassSchedule.SectionId == sectionId).ToListAsync();
            var reschedule = await _context.ClassReschedules.Where(x => x.Class.SectionId == sectionId).ToListAsync();

            _context.ClassReschedules.RemoveRange(reschedule);
            _context.Classes.RemoveRange(classes);

            var term = await _context.Sections.Where(x => x.Id == sectionId).Select(x => x.CourseTerm.Term).FirstOrDefaultAsync();

            var startClassDay = term.ClassStartDate.Date;/*.ToDefaultTimeZone();*/
            var endClassDay = term.ClassEndDate.Date;/*.ToDefaultTimeZone();*/

            var classSchedulesData = await _context.ClassSchedules.Where(y => y.SectionId == sectionId).ToListAsync();
            var classSchedules = classSchedulesData.OrderBy(x => x.WeekDay).ThenBy(x => x.StartTime.ToLocalTimeSpanUtc()).ToList();

            var weekNumber = 1;
            var classNumber = 1;
            var guideNumber = 1;
            var tpmClassDay = startClassDay.Date;
            var _classes = new List<Class>();
            var weekDays = classSchedules.Select(x => ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay)).ToList();

            var holidays = await _context.Holidays.Where(y => y.Date.Date >= startClassDay && y.Date.Date <= endClassDay).ToListAsync();

            while (tpmClassDay <= endClassDay.Date)
            {
                if (guideNumber == 8)
                {
                    weekNumber++;
                    classNumber = 1;
                    guideNumber = 1;
                }

                if (weekDays.Contains(tpmClassDay.DayOfWeek))
                {
                    var holidayTpm = holidays.Where(x => x.Date.Date == tpmClassDay.Date).FirstOrDefault();

                    if (holidayTpm == null || holidayTpm.NeedReschedule)
                    {
                        var classSchedulesTmp = classSchedules.Where(x => x.WeekDay == ConstantHelpers.WEEKDAY.ENUM_TO_INT(tpmClassDay.DayOfWeek)).ToList();
                        foreach (var item in classSchedulesTmp)
                        {
                            var startTime = item.StartTime.ToLocalTimeSpanUtc();
                            var endTime = item.EndTime.ToLocalTimeSpanUtc();

                            var start_day = new DateTime(tpmClassDay.Year, tpmClassDay.Month, tpmClassDay.Day, startTime.Hours, startTime.Minutes, 0);
                            var end_day = new DateTime(tpmClassDay.Year, tpmClassDay.Month, tpmClassDay.Day, endTime.Hours, endTime.Minutes, 0);

                            var _class = new Class
                            {
                                ClassScheduleId = item.Id,
                                IsDictated = false,
                                SectionId = item.SectionId,
                                ClassNumber = classNumber,
                                StartTime = start_day.ToUtcDateTime(),
                                EndTime = end_day.ToUtcDateTime(),
                                WeekNumber = weekNumber,
                                ClassroomId = item.ClassroomId,
                                NeedReschedule = holidayTpm != null && holidayTpm.NeedReschedule
                            };

                            classNumber++;
                            _classes.Add(_class);
                        }
                    }
                }

                tpmClassDay = tpmClassDay.AddDays(1);
                guideNumber++;
            }

            await _context.Classes.AddRangeAsync(_classes);
            await _context.SaveChangesAsync();
        }

        private async Task<ResultTemplate> ValidateTeacherSchedules(Guid termId, ValidateClassScheduleTemplate model)
        {
            var result = new ResultTemplate();

            if (model.Teachers != null && model.Teachers.Any())
            {
                var teacherSchedules = await _context.TeacherSchedules.Where(x =>
                x.ClassSchedule.Section.CourseTerm.TermId == termId &&
                model.Teachers.Contains(x.TeacherId)
                )
                    .Select(x => new
                    {
                        x.Id,
                        x.ClassScheduleId,
                        x.ClassSchedule.StartTime,
                        x.ClassSchedule.EndTime,
                        x.ClassSchedule.WeekDay,
                        section = x.ClassSchedule.Section.Code,
                        courseCode = x.ClassSchedule.Section.CourseTerm.Course.Code,
                        x.TeacherId,
                        teacher = x.Teacher.User.FullName,
                        courseName = x.ClassSchedule.Section.CourseTerm.Course.Name,
                    })
                    .ToListAsync();

                foreach (var item in model.Teachers)
                {
                    var conflictedClass = teacherSchedules.Where(x => x.ClassScheduleId != model.Id && x.TeacherId == item && x.WeekDay == model.WeekDay && IsInConflict(x.StartTime.ToLocalTimeSpanUtc(), x.EndTime.ToLocalTimeSpanUtc(), model.StartTimeUTC.ToLocalTimeSpanUtc(), model.EndTimeUTC.ToLocalTimeSpanUtc())).FirstOrDefault();

                    if (conflictedClass != null)
                    {
                        result.Message = $"El profesor {conflictedClass.teacher} está ocupado en ese rango de horas por una clase de la sección {conflictedClass.section} del curso {conflictedClass.courseCode} - {conflictedClass.courseName}.";
                        return result;
                    }
                }
            }

            result.Succeeded = true;
            return result;
        }

        private async Task AssignTeachers(Guid classScheduleId, List<string> teachers)
        {
            var teacherSchedules = await _context.TeacherSchedules.Where(x => x.ClassScheduleId == classScheduleId).ToListAsync();
            _context.TeacherSchedules.RemoveRange(teacherSchedules);

            if (teachers != null && teachers.Any())
            {
                var teacherSchedulesToAdd = teachers.Select(y => new TeacherSchedule
                {
                    TeacherId = y,
                    ClassScheduleId = classScheduleId
                }).ToList();

                await _context.TeacherSchedules.AddRangeAsync(teacherSchedulesToAdd);
            }
        }


    }
}