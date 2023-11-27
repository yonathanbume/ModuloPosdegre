using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeacherSchedule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class TeacherScheduleRepository : Repository<TeacherSchedule>, ITeacherScheduleRepository
    {
        public TeacherScheduleRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetAllAsModelB(string teacherId = null)
        {
            var output = await _context.TeacherSchedules
                                       .Include(x => x.ClassSchedule.Section)
                                       .Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.TeacherId == teacherId))
                                       .Select(x => new
                                       {
                                           Actividad = x.ClassSchedule.Section.CourseTerm.Course.Name,
                                           Faculty = x.ClassSchedule.Section.CourseTerm.Course.Career.Name,
                                           ClassRoom = x.ClassSchedule.Classroom.Description,
                                           StartTime = x.ClassSchedule.StartTime,
                                           EndTime = x.ClassSchedule.EndTime,
                                           NumberWeekDay = x.ClassSchedule.WeekDay,
                                           Ubicacion = x.ClassSchedule.Classroom.Description
                                           //HT = x.ClassSchedule.SessionType == 1 ? x.ClassSchedule.Section.CourseTerm.Course.TheoreticalHours : 0,
                                           //HP = x.ClassSchedule.SessionType == 2 ? x.ClassSchedule.Section.CourseTerm.Course.PracticalHours : 0,

                                       }).ToListAsync();

            return output;
        }

        public async Task<object> GetAllAsModelC(Guid? termId = null, string teacherId = null)
        {
            var term = await _context.Terms.FindAsync(termId);
            var teacher = await _context.Users.FirstOrDefaultAsync(x => x.Id == teacherId);
            var courseSyllabus = await _context.CourseSyllabus.Where(x => x.TermId == termId).ToListAsync();

            var result = _context.TeacherSchedules
                                 .Include(c => c.ClassSchedule.Section.CourseTerm.Course)
                                 .Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.TeacherId == teacherId))
                                 .Select(c => new
                                 {
                                     Time = c.ClassSchedule.StartTime.ToUtcDateTime().ToString("HH:mm") + " - " + c.ClassSchedule.EndTime.ToUtcDateTime().ToString("HH:mm"),
                                     Teacher = teacher.FullName,
                                     Activity = ConstantHelpers.SESSION_TYPE.VALUES[c.ClassSchedule.SessionType],
                                     courseSyllabus = courseSyllabus.FirstOrDefault(s => s.CourseId == c.ClassSchedule.Section.CourseTerm.CourseId),
                                     Week = c.ClassSchedule.WeekDay,
                                     Type = ConstantHelpers.SESSION_TYPE.VALUES[c.ClassSchedule.SessionType],
                                     Classroom = c.ClassSchedule.Classroom.Description,
                                     Section = c.ClassSchedule.Section.Code,
                                     Course = string.Format("{0}-{1}", c.ClassSchedule.Section.CourseTerm.Course.Code, c.ClassSchedule.Section.CourseTerm.Course.Name),
                                 }).ToList();

            //var result = _context.Classes.Include(c => c.Section.CourseTerm.Course)
            //.Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.TeacherId == teacherId))
            //.Where(c => c.StartTime.Date >= DateTime.Now.Date && c.EndTime.Date <= DateTime.Now.Date)
            //.Select(c => new
            //{
            //    Time = c.StartTime.ToDefaultTimeZone().ToString("HH:mm") + " - " + c.EndTime.ToDefaultTimeZone().ToString("HH:mm"),
            //    Teacher = teacher.FullName,
            //    Activity = ConstantHelpers.SESSION_TYPE.VALUES[c.ClassSchedule.SessionType],
            //    courseSyllabus = courseSyllabus.FirstOrDefault(s => s.CourseId == c.Section.CourseTerm.CourseId),
            //    Week = c.WeekNumber,
            //    Type = ConstantHelpers.SESSION_TYPE.VALUES[c.ClassSchedule.SessionType],
            //    Classroom = c.Classroom.Description,
            //    Section = c.ClassSchedule.Section.Code,
            //    Course = string.Format("{0}-{1}", c.ClassSchedule.Section.CourseTerm.Course.Code, c.ClassSchedule.Section.CourseTerm.Course.Name),
            //}).ToList();

            var result2 = result.Select(x => new
            {
                x.Time,
                x.Teacher,
                x.Course,
                x.Activity,
                x.Week,
                x.Type,
                x.Classroom,
                x.Section,
                CourseUnits = x.courseSyllabus == null ? null : _context.CourseUnits.Where(y => y.CourseSyllabusId == x.courseSyllabus.Id)
            }).ToList();

            var output = result2.Select(x => new
            {
                Time = x.Time,
                Teacher = x.Teacher,
                Course = x.Course,
                Activity = x.Activity,
                Type = x.Type,
                Cycle = term.Name,
                Classroom = x.Classroom,
                Section = x.Section,
                Topic = x.CourseUnits == null ? "-" : x.CourseUnits.First(y => y.WeekNumberStart <= x.Week && y.WeekNumberEnd >= x.Week).Name
            }).OrderBy(x => x.Time).ToList();

            return output;
        }

        public async Task<object> GetAllAsModelD(string teacherId = null)
        {
            var teacher = await _context.Users.FirstOrDefaultAsync(x => x.Id == teacherId);

            var output = await _context.TeacherSchedules
                                 .Include(x => x.ClassSchedule.Section.CourseTerm.Term)
                                 .Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.TeacherId == teacherId && c.ClassSchedule.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE))
                                 .Select(c => new
                                 {
                                     Time = c.ClassSchedule.StartTime.ToUtcDateTime().ToString("HH:mm") + " - " + c.ClassSchedule.EndTime.ToUtcDateTime().ToString("HH:mm"),
                                     Teacher = teacher.FullName,
                                     Activity = ConstantHelpers.SESSION_TYPE.VALUES[c.ClassSchedule.SessionType],
                                     HT = c.ClassSchedule.SessionType == 1 ? DateTime.ParseExact(c.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(c.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalHours : 0,
                                     HP = c.ClassSchedule.SessionType == 2 ? DateTime.ParseExact(c.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(c.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalHours : 0,
                                     //courseSyllabus = courseSyllabus.FirstOrDefault(s => s.CourseId == c.Section.CourseTerm.CourseId),
                                     //Week = c.WeekNumber,
                                     Students = c.ClassSchedule.Section.StudentSections.Count(),
                                     Topic = string.Format("{0}-{1} ({2})", c.ClassSchedule.Section.CourseTerm.Course.Code, c.ClassSchedule.Section.CourseTerm.Course.Name, c.ClassSchedule.Section.Code),
                                 }).ToListAsync();

            return output;
        }

        public async Task<IEnumerable<TeacherSchedule>> GetAllByClassSchedule(Guid classScheduleId)
        {
            return await _context.TeacherSchedules.Where(x => x.ClassScheduleId == classScheduleId)
.Select(x => new TeacherSchedule
{
    Id = x.Id,
    TeacherId = x.TeacherId,
    Teacher = new Teacher
    {
        User = new ApplicationUser
        {
            Name = x.Teacher.User.Name,
            MaternalSurname = x.Teacher.User.MaternalSurname,
            PaternalSurname = x.Teacher.User.PaternalSurname,
            FullName = x.Teacher.User.FullName
        }
    }
})
.ToListAsync();
        }

        public async Task<TeacherSchedule> GetConflictedClass(string teacherId, int weekDay, TimeSpan startTime, TimeSpan timeEnd, Guid termId, Guid? id = null)
        {

            var teacherSchedules = await _context
                .TeacherSchedules.Where(x => x.ClassSchedule.Section.CourseTerm.TermId == termId && x.TeacherId == teacherId && x.ClassSchedule.WeekDay == weekDay)
                .Include(x=>x.ClassSchedule)
                .ThenInclude(x=>x.Section)
                .ThenInclude(x=>x.CourseTerm)
                .ThenInclude(x=>x.Course)
                .ToListAsync();

            var conflictedClass = teacherSchedules.Where(x => x.ClassScheduleId != id && IsInConflict(x.ClassSchedule.StartTime.ToLocalTimeSpanUtc(), x.ClassSchedule.EndTime.ToLocalTimeSpanUtc(), startTime.ToLocalTimeSpanUtc(), timeEnd.ToLocalTimeSpanUtc())).FirstOrDefault();

            return conflictedClass;

            //if (id.HasValue)
            //{
            //    var conflictedClass = await _context.TeacherSchedules.Include(x => x.ClassSchedule.Section.CourseTerm.Course)
            //        .FirstOrDefaultAsync(
            //            x => x.ClassScheduleId != id.Value && x.TeacherId == teacherId &&
            //                 x.ClassSchedule.WeekDay == weekDay &&
            //                 ((x.ClassSchedule.StartTime <= startTime && x.ClassSchedule.EndTime > startTime) ||
            //                 (x.ClassSchedule.StartTime < timeEnd && x.ClassSchedule.EndTime >= timeEnd) ||
            //                 (startTime <= x.ClassSchedule.StartTime && timeEnd > x.ClassSchedule.StartTime) ||
            //                 (startTime < x.ClassSchedule.EndTime && timeEnd >= x.ClassSchedule.EndTime))
            //                 //IsInConflict(x.ClassSchedule.StartTime, x.ClassSchedule.EndTime, startTime, timeEnd)
            //                 &&
            //                 x.ClassSchedule.Section.CourseTerm.TermId == termId);

            //    return conflictedClass;
            //}
            //else
            //{
            //    var conflictedClass = await _context.TeacherSchedules.Include(x => x.ClassSchedule.Section.CourseTerm.Course)
            //        .FirstOrDefaultAsync(
            //            x => x.TeacherId == teacherId &&
            //                 x.ClassSchedule.WeekDay == weekDay &&
            //                 ((x.ClassSchedule.StartTime <= startTime && x.ClassSchedule.EndTime > startTime) ||
            //                 (x.ClassSchedule.StartTime < timeEnd && x.ClassSchedule.EndTime >= timeEnd) ||
            //                 (startTime <= x.ClassSchedule.StartTime && timeEnd > x.ClassSchedule.StartTime) ||
            //                 (startTime < x.ClassSchedule.EndTime && timeEnd >= x.ClassSchedule.EndTime))
            //                 //IsInConflict(x.ClassSchedule.StartTime, x.ClassSchedule.EndTime, startTime, timeEnd) 
            //                 &&
            //                 x.ClassSchedule.Section.CourseTerm.TermId == termId);

            //    return conflictedClass;
            //}
        }

        public void TEst(string teacherId, Guid termId)
        {
            var sections = _context.TeacherSchedules
                .Include(x => x.ClassSchedule.Section.CourseTerm.Course.Career.Faculty)
                .Where(x => x.TeacherId == teacherId && x.ClassSchedule.Section.CourseTerm.TermId == termId)
                .Select(x => new
                {
                    course = x.ClassSchedule.Section.CourseTerm.Course.Code + " " + x.ClassSchedule.Section.CourseTerm.Course.Name,
                    section = x.ClassSchedule.Section.Code,
                    classroom = x.ClassSchedule.Classroom.Description,
                    faculty = x.ClassSchedule.Section.CourseTerm.Course.Career.Faculty.Name,
                    teoricHours = x.ClassSchedule.SessionType == ConstantHelpers.SESSION_TYPE.THEORY ? DateTime.ParseExact(x.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(x.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalHours : 0,
                    practicalHours = x.ClassSchedule.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE ? DateTime.ParseExact(x.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(x.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalHours : 0,
                    students = x.ClassSchedule.Section.StudentSections.Count,
                }).ToList();
        }
        //lectivas
        async Task<IEnumerable<TeacherScheduleTemplateA>> ITeacherScheduleRepository.GetAllAsTemplateA(Guid termId, string teacherId)
        {
            var key = ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME;
            var configuration = await _context.Configurations.Where(x => x.Key == key).FirstOrDefaultAsync();

            if (configuration == null)
            {
                var value = ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES[key];
                configuration = new ENTITIES.Models.Configuration
                {
                    Key = key,
                    Value = value
                };

                await _context.AddAsync(configuration);
                await _context.SaveChangesAsync();
            }

            int.TryParse(configuration.Value, out var pedagogical_hour_time);

            var teacherSchedules = await _context.TeacherSchedules
                .Where(x => x.TeacherId == teacherId && x.ClassSchedule.Section.CourseTerm.TermId == termId && !x.ClassSchedule.Section.IsDirectedCourse)
                .Select(x => new
                {
                    SectionId = x.ClassSchedule.SectionId,
                    Modality = x.ClassSchedule.Section.CourseTerm.Modality,
                    Course = x.ClassSchedule.Section.CourseTerm.Course.Code + " " + x.ClassSchedule.Section.CourseTerm.Course.Name,
                    Section = x.ClassSchedule.Section.Code,
                    CourseCode = x.ClassSchedule.Section.CourseTerm.Course.Code,
                    CourseName = x.ClassSchedule.Section.CourseTerm.Course.Name,
                    Classroom = x.ClassSchedule.Classroom.Description,
                    Faculty = x.ClassSchedule.Section.CourseTerm.Course.Career.Faculty.Name,
                    Career = x.ClassSchedule.Section.CourseTerm.Course.Career.Name,
                    x.ClassSchedule.SessionType,
                    Students = x.ClassSchedule.Section.StudentSections.Count,
                    x.ClassSchedule.StartTime,
                    x.ClassSchedule.EndTime,
                    AcademicYears = x.ClassSchedule.Section.CourseTerm.Course.AcademicYearCourses.Select(y=>y.AcademicYear).ToList()
                })
                .ToListAsync();

            var sections = teacherSchedules
                .Select(x => new TeacherScheduleTemplateA
                {
                    SectionId = x.SectionId,
                    CourseTermModality = ConstantHelpers.Course.Modality.VALUES.ContainsKey(x.Modality) ? ConstantHelpers.Course.Modality.VALUES[x.Modality] : "-",
                    Course = x.Course,
                    CourseCode = x.CourseCode,
                    CourseName = x.CourseName,
                    Section = x.Section,
                    Classroom = x.Classroom,
                    Faculty = x.Faculty,
                    Career = x.Career,
                    TeoricHours = x.SessionType == ConstantHelpers.SESSION_TYPE.THEORY ? x.EndTime.ToLocalDateTimeUtc().TimeOfDay.Subtract(x.StartTime.ToLocalDateTimeUtc().TimeOfDay).TotalMinutes / pedagogical_hour_time : 0,
                    PracticalHours = x.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE ? x.EndTime.ToLocalDateTimeUtc().TimeOfDay.Subtract(x.StartTime.ToLocalDateTimeUtc().TimeOfDay).TotalMinutes / pedagogical_hour_time : 0,
                    Students = x.Students,
                    AcademicYear = string.Join(", ",
                    x.AcademicYears.Select(y =>
                    ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS.ContainsKey(y) ?
                    ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[y] : "-"
                    ).Distinct().ToList()
                    )
            }).ToList();

            var sectionsWithSchedule = sections.Select(x => x.SectionId).ToList();

            var sectionsWithOutScheduleData = await _context.TeacherSections.Where(x => x.Section.CourseTerm.TermId == termId && x.TeacherId == teacherId && !x.Section.IsDirectedCourse && !sectionsWithSchedule.Contains(x.SectionId))
                .Select(x => new
                {
                    SectionId = x.SectionId,
                    Modality = x.Section.CourseTerm.Modality,
                    Course = x.Section.CourseTerm.Course.Code + " " + x.Section.CourseTerm.Course.Name,
                    CourseCode = x.Section.CourseTerm.Course.Code,
                    CourseName = x.Section.CourseTerm.Course.Name,
                    Section = x.Section.Code,
                    Classroom = "-",
                    Students = x.Section.StudentSections.Count(),
                    Faculty = x.Section.CourseTerm.Course.Career.Faculty.Name,
                    Career = x.Section.CourseTerm.Course.Career.Name,
                    AcademicYears = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y=> y.AcademicYear).ToList()
                })
                .ToListAsync();

            var sectionsWithOutSchedule = sectionsWithOutScheduleData
                .Select(x => new TeacherScheduleTemplateA
                {
                    SectionId = x.SectionId,
                    CourseTermModality = ConstantHelpers.Course.Modality.VALUES.ContainsKey(x.Modality) ? ConstantHelpers.Course.Modality.VALUES[x.Modality] : "-",
                    Course = x.Course,
                    CourseCode = x.CourseCode,
                    CourseName = x.CourseName,
                    Section = x.Section,
                    Classroom = x.Classroom,
                    Students = x.Students,
                    Faculty = x.Faculty,
                    Career = x.Career,
                    AcademicYear = string.Join(", ",
                    x.AcademicYears.Select(y =>
                    ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS.ContainsKey(y) ?
                    ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[y] : "-"
                    ).Distinct().ToList()
                    )
                })
                .ToList();

            var directedCourseData = await _context.TeacherSections.Where(x => x.TeacherId == teacherId && x.Section.CourseTerm.TermId == termId && x.Section.IsDirectedCourse)
                .Select(x => new
                {
                    Course = $"{x.Section.CourseTerm.Course.Code} {x.Section.CourseTerm.Course.Name}",
                    CourseCode = x.Section.CourseTerm.Course.Code,
                    CourseName = x.Section.CourseTerm.Course.Name,
                    Modality = x.Section.CourseTerm.Course.CourseTerms.Where(y => y.TermId == termId).Select(y => y.Modality).FirstOrDefault(),
                    AcademicYears = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y=> y.AcademicYear).ToList(),
                    Section = "DIRIGIDO",
                    Classroom = "-",
                    Faculty = x.Section.CourseTerm.Course.Career.Faculty.Name,
                    Career = x.Section.CourseTerm.Course.Career.Name,
                    TeoricHours = (double)x.Section.CourseTerm.Course.TheoreticalHours / 2,
                    PracticalHours = (double)x.Section.CourseTerm.Course.PracticalHours / 2,
                    Students = x.Section.StudentSections.Count()
                })
                .ToListAsync();

            var directedCourses = directedCourseData
                .Select(x => new TeacherScheduleTemplateA
                {
                    Course = x.Course,
                    CourseCode = x.CourseCode,
                    CourseName = x.CourseName,
                    CourseTermModality = ConstantHelpers.Course.Modality.VALUES.ContainsKey(x.Modality) ? ConstantHelpers.Course.Modality.VALUES[x.Modality] : "-",
                    AcademicYear = string.Join(", ", x.AcademicYears.Select(y =>
                     ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS.ContainsKey(y) ?
                     ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[y] : "-"
                    ).Distinct().ToList()
                    ),
                    Section = "DIRIGIDO",
                    Classroom = "-",
                    Faculty = x.Faculty,
                    Career = x.Career,
                    TeoricHours = x.TeoricHours,
                    PracticalHours = x.PracticalHours,
                    Students = x.Students
                })
                .ToList();

            sections.AddRange(directedCourses);
            sections.AddRange(sectionsWithOutSchedule);

            return sections;
        }
        async Task<IEnumerable<TeacherScheduleTemplateA>> ITeacherScheduleRepository.GetDaiylyReportAsTemplateA(Guid termId, Guid? careerId, DateTime start, DateTime end, string teacherId, Guid? academicDepartmentId)
        {
            var query = _context.TeacherSchedules
                .Where(x => x.ClassSchedule.Section.CourseTerm.TermId == termId &&
                            x.ClassSchedule.Classes.Any(y => y.StartTime >= start) &&
                            x.ClassSchedule.Classes.Any(y => y.StartTime <= end)
                            )
                //.Include(x => x.ClassSchedule.Section.CourseTerm.Course.Career.Faculty)
                .AsQueryable();

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.ClassSchedule.Section.CourseTerm.Course.CareerId == careerId);

            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.TeacherId == teacherId);

            var sections = await query.Select(x => new TeacherScheduleTemplateA
            {
                Schedule = $"{x.ClassSchedule.StartTimeText} - {x.ClassSchedule.EndTimeText}",//horario
                TeacherName = x.Teacher.User.FullName,
                SectionId = x.ClassSchedule.SectionId,
                Course = x.ClassSchedule.Section.CourseTerm.Course.Code + " " + x.ClassSchedule.Section.CourseTerm.Course.Name,//asignatura
                Section = x.ClassSchedule.Section.Code,//seccion
                Classroom = x.ClassSchedule.Classroom.Description,//aula
                Faculty = x.ClassSchedule.Section.CourseTerm.Course.Career.Faculty.Name,
                TeoricHours = x.ClassSchedule.SessionType == ConstantHelpers.SESSION_TYPE.THEORY ? DateTime.ParseExact(x.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(x.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalHours : 0,
                PracticalHours = x.ClassSchedule.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE ? DateTime.ParseExact(x.ClassSchedule.EndTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay.Subtract(DateTime.ParseExact(x.ClassSchedule.StartTimeText, ConstantHelpers.FORMATS.TIME, CultureInfo.InvariantCulture).TimeOfDay).TotalHours : 0,
                Students = x.ClassSchedule.Section.StudentSections.Count,//num estudiantes
                Subject = x.ClassSchedule.Classes.Where(y => y.ClassScheduleId == x.ClassScheduleId && y.IsDictated).Select(y => y.UnitActivity.Name).FirstOrDefault(),//tema desarrollado
                Cycle = x.ClassSchedule.Section.CourseTerm.Course.AcademicYearCourses.FirstOrDefault().AcademicYear//ciclo
            }).ToListAsync();

            return sections;
        }

        async Task<IEnumerable<TeacherSchedule>> ITeacherScheduleRepository.GetAllByClassSchedule(Guid classScheduleId)
        {
            return await _context.TeacherSchedules.Where(x => x.ClassScheduleId == classScheduleId)
                           .Select(x => new TeacherSchedule
                           {
                               Id = x.Id,
                               TeacherId = x.TeacherId,
                               Teacher = new Teacher
                               {
                                   User = new ApplicationUser
                                   {
                                       Name = x.Teacher.User.Name,
                                       MaternalSurname = x.Teacher.User.MaternalSurname,
                                       PaternalSurname = x.Teacher.User.PaternalSurname,
                                       FullName = x.Teacher.User.FullName
                                   }
                               }
                           })
                           .ToListAsync();
        }

        async Task<IEnumerable<TeacherSchedule>> ITeacherScheduleRepository.GetAllByClassSchedule2(Guid classScheduleId)
        {
            return await _context.TeacherSchedules.Where(x => x.ClassScheduleId == classScheduleId).ToListAsync();
        }

        private bool IsInConflict(TimeSpan st1, TimeSpan et1, TimeSpan st2, TimeSpan et2)
        {
            return (st1 <= st2 && et1 > st2) || (st1 < et2 && et1 >= et2) || (st2 <= st1 && et2 > st1) || (st2 < et1 && et2 >= et1);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherClassesDetailedWithoutAttendance(DataTablesStructs.SentParameters parameters, Guid termId, DateTime endTime, string teacherId)
        {
            //var query = _context.TeacherSchedules.Where(x => x.TeacherId == teacherId && x.ClassSchedule.Section.CourseTerm.TermId == termId && x.ClassSchedule.Classes.Any(y => !y.IsDictated && y.EndTime < endTime)).AsNoTracking();

            var query = _context.TeacherSections.Where(x => x.TeacherId == teacherId && x.Section.ClassSchedules.Any(y => y.Classes.Any(z => !z.IsDictated && z.EndTime < endTime))).AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var dataDB = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    course = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    sectionId = x.Section.Id
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.course,
                    x.section,
                    clases = string.Join(", ", _context.Classes.Where(y => y.SectionId == x.sectionId && !y.IsDictated && y.EndTime < endTime).Select(y => $"{y.StartTime.ToLocalDateTimeFormat()}-{y.EndTime.ToLocalDateTimeFormat()}").ToList())
                })
                .ToList();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }
    
        public async Task<DataTablesStructs.ReturnedData<object>> GetReportTeacherSchedulesDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string search)
        {
            Expression<Func<TeacherSchedule, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Teacher.User.UserName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Teacher.User.FullName);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.ClassSchedule.Section.CourseTerm.Course.AcademicYearCourses.Select(y=>y.Curriculum.Career.Name).FirstOrDefault());
                    break;
                case "4":
                    orderByPredicate = ((x) => x.ClassSchedule.Section.CourseTerm.Course.Code);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.ClassSchedule.Section.CourseTerm.Course.Name);
                    break;
                case "6":
                    orderByPredicate = ((x) => x.ClassSchedule.Section.Code);
                    break;
                default:
                    orderByPredicate = ((x) => x.Teacher.User.UserName);
                    break;
            }

            var query = _context.TeacherSchedules
                .Where(x=>x.ClassSchedule.Section.CourseTerm.TermId == termId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Teacher.User.FullName.ToLower().Trim().Contains(search.ToLower().Trim()) || x.Teacher.User.UserName.ToLower().Trim().Contains(search.ToLower().Trim()));

            var recordsTotal = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new TeacherScheduleReportTemplate
                {
                    Id = x.Id,
                    TeacherFullName = x.Teacher.User.FullName,
                    TeacherUserName = x.Teacher.User.UserName,
                    Career = x.ClassSchedule.Section.CourseTerm.Course.AcademicYearCourses.Select(y=>y.Curriculum.Career.Name).FirstOrDefault(),
                    CourseCode = x.ClassSchedule.Section.CourseTerm.Course.Code,
                    CourseName = x.ClassSchedule.Section.CourseTerm.Course.Name,
                    Section = x.ClassSchedule.Section.Code,
                    WeekDay = ConstantHelpers.WEEKDAY.VALUES[x.ClassSchedule.WeekDay],
                    StartTimeUTC = x.ClassSchedule.StartTime,
                    EndTimeUTC =  x.ClassSchedule.EndTime
                })
                .ToListAsync();

            //var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<TeacherScheduleReportTemplate>> GetReportTeacherSchedulesTemplate(Guid termId)
        {
            var query = _context.TeacherSchedules
              .Where(x => x.ClassSchedule.Section.CourseTerm.TermId == termId)
              .AsNoTracking();

            var data = await query
                .Select(x => new TeacherScheduleReportTemplate
                {
                    Id = x.Id,
                    TeacherFullName = x.Teacher.User.FullName,
                    TeacherUserName = x.Teacher.User.UserName,
                    CourseCode = x.ClassSchedule.Section.CourseTerm.Course.Code,
                    CourseName = x.ClassSchedule.Section.CourseTerm.Course.Name,
                    Section = x.ClassSchedule.Section.Code,
                    WeekDay = ConstantHelpers.WEEKDAY.VALUES[x.ClassSchedule.WeekDay],
                    StartTimeUTC = x.ClassSchedule.StartTime,
                    Career = x.ClassSchedule.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Career.Name).FirstOrDefault(),
                    EndTimeUTC = x.ClassSchedule.EndTime,
                    AcademicDepartment = x.Teacher.AcademicDepartment.Name,
                    AcademicProgram = x.ClassSchedule.Section.CourseTerm.Course.AcademicProgram.Name,
                    AcademicYear = x.ClassSchedule.Section.CourseTerm.Course.AcademicYearCourses.Select(y=>y.AcademicYear).FirstOrDefault(),
                    Curriculum = x.ClassSchedule.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.Curriculum.Code).FirstOrDefault(),
                    Classroom = x.ClassSchedule.Classroom.Code,
                    ScheduleSessionType = ConstantHelpers.SESSION_TYPE.VALUES[x.ClassSchedule.SessionType],
                    Enrolled = x.ClassSchedule.Section.StudentSections.Count()
                })
                .ToListAsync();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSchedulesDatatable(DataTablesStructs.SentParameters parameters, Guid sectionId, string teacherId)
        {
            var query = _context.TeacherSchedules.Where(x => x.ClassSchedule.SectionId == sectionId && x.TeacherId == teacherId).AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    Id = x.Id,
                    x.ClassSchedule.SessionType,
                    WeekDay = ConstantHelpers.WEEKDAY.VALUES[x.ClassSchedule.WeekDay],
                    StartTime = x.ClassSchedule.StartTime.ToLocalDateTimeFormatUtc(),
                    EndTime = x.ClassSchedule.EndTime.ToLocalDateTimeFormatUtc()
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }
    }
}