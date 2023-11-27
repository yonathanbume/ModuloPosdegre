using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Class;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassSchedule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        public ClassRepository(AkdemicContext context) : base(context) { }

        public override async Task<Class> Get(Guid id)
        {
            return await _context.Classes.Include(x => x.ClassSchedule).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> AnyCrossingByClassroomIdAndDateRange(Guid classroomId, DateTime start, DateTime end, Guid? exceptionId = null)
        {
            var query = _context.Classes.AsQueryable();

            if (exceptionId.HasValue)
                query = query.Where(c => c.Id != exceptionId.Value);
            return await query.AnyAsync(c => c.ClassroomId == classroomId && (c.StartTime < end && start < c.EndTime) /*ConvertHelpers.DateTimeConflict(c.StartTime, c.EndTime, start, end)*/);
        }

        public async Task<IEnumerable<Class>> GetAll(Guid? studentId = null, Guid? termId = null, string teacherId = null, Guid? classroomId = null, DateTime? start = null, DateTime? end = null, bool? isDictated = null)
        {
            var query = _context.Classes
                .Include(x => x.ClassSchedule.Section.CourseTerm.Course)
                .Include(x => x.Classroom)
                .AsNoTracking();

            if (studentId.HasValue)
                query = query.Where(x => x.ClassSchedule.Section.StudentSections.Any(ss => ss.StudentId == studentId.Value));

            if (termId.HasValue)
                query = query.Where(x => x.ClassSchedule.Section.CourseTerm.TermId == termId.Value);

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Include(x => x.Section.TeacherSections).Where(x => x.ClassSchedule.TeacherSchedules.Any(ts => ts.TeacherId == teacherId));

            if (classroomId.HasValue)
                query = query.Where(x => x.ClassroomId == classroomId.Value);

            if (start.HasValue)
                query = query.Where(x => x.StartTime >= start);

            if (end.HasValue)
                query = query.Where(x => x.EndTime <= end);

            if (isDictated.HasValue)
                query = query.Where(x => x.IsDictated == isDictated.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Class>> GetAllBySectionId(Guid sectionId)
        {
            return await _context.Classes
                .Where(x => x.ClassSchedule.SectionId == sectionId && x.UnitActivityId != null).ToListAsync();
        }
        public async Task<IEnumerable<Class>> GetAllBySectionId2(Guid sectionId)
        {
            return await _context.Classes
                .Where(x => x.ClassSchedule.SectionId == sectionId).ToListAsync();
        }

        public async Task<object> GetOldClassesDatatableClientSide(Guid sectionId, DateTime? day = null, string teacherId = null)
        {
            var query = _context.Classes
                .Where(x => x.SectionId == sectionId
                && !x.IsDictated
                && x.EndTime < DateTime.UtcNow)
                .Include(x => x.ClassSchedule)
                .AsNoTracking();

            if (day.HasValue) query = query.Where(x => x.StartTime.Date == day.Value.Date);
            if (!string.IsNullOrEmpty(teacherId)) query = query.Where(x => x.ClassSchedule.TeacherSchedules.Any(y => y.TeacherId == teacherId));

            var result = await query
                .OrderBy(x => x.StartTime)
                .Select(x => new
                {
                    id = x.Id,
                    number = $"{x.WeekNumber}.{x.ClassNumber}",
                    classroom = x.Classroom.Description,
                    type = ConstantHelpers.SESSION_TYPE.VALUES[x.ClassSchedule.SessionType],
                    date = x.StartTime.ToLocalDateFormat(),
                    start = $"{x.StartTime.ToDefaultTimeZone().TimeOfDay}",
                    end = $"{x.EndTime.ToDefaultTimeZone().TimeOfDay}",
                    isDictated = x.IsDictated,
                    x.NeedReschedule
                }).ToListAsync();

            return result;
        }
        public async Task<object> GetHistoryClassesDatatableClientSide(Guid sectionId, DateTime? day = null, string teacherId = null)
        {
            var query = _context.Classes
                .Where(x => x.SectionId == sectionId
                && x.IsDictated
                && x.EndTime < DateTime.UtcNow
                )
                .Include(x => x.ClassSchedule)
                .AsNoTracking();

            if (day.HasValue) query = query.Where(x => x.StartTime.Date == day.Value.Date);
            if (!string.IsNullOrEmpty(teacherId)) query = query.Where(x => x.ClassSchedule.TeacherSchedules.Any(y => y.TeacherId == teacherId));

            var result = await query
                .OrderBy(x => x.StartTime)
                .Select(x => new
                {
                    id = x.Id,
                    number = $"{x.WeekNumber}.{x.ClassNumber}",
                    classroom = x.Classroom.Description,
                    date = x.StartTime.ToLocalDateFormat(),
                    start = $"{x.StartTime.ToDefaultTimeZone().TimeOfDay}",
                    end = $"{x.EndTime.ToDefaultTimeZone().TimeOfDay}",
                    isDictated = x.IsDictated
                }).ToListAsync();

            return result;
        }
        public async Task<object> GetAllByTermIdTeacherIdAndDateRange(Guid termId, string userId, DateTime start, DateTime end)
        {
            var result = await _context.Classes
                .Where(c => c.Section.CourseTerm.TermId == termId && c.ClassSchedule.TeacherSchedules.Any(ts => ts.TeacherId == userId) && c.StartTime >= start && c.EndTime <= end)
                .Select(c => new
                {
                    id = c.Id,
                    title = string.Format("{0}-{1} ({2})", c.ClassSchedule.Section.CourseTerm.Course.Code, c.ClassSchedule.Section.CourseTerm.Course.Name, c.ClassSchedule.Section.Code),
                    description = c.Classroom.Description,
                    allDay = false,
                    start = c.StartTime.ToDefaultTimeZone().ToString("yyyy-MM-dd HH:mm:ss"),
                    end = c.EndTime.ToDefaultTimeZone().ToString("yyyy-MM-dd HH:mm:ss")
                }).ToListAsync();

            return result;
        }

        public async Task<object> GetAsModelAByIdAndTeacherId(Guid id, string teacherId)
        {
            var result = await _context.Classes
                .Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.TeacherId == teacherId))
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    classroom = c.Classroom.Description,
                    date = c.StartTime.ToLocalDateFormat(),
                    start = c.StartTime.ToLocalTimeFormat(),
                    end = c.EndTime.ToLocalTimeFormat(),
                    course = c.ClassSchedule.Section.CourseTerm.Course.FullName,
                    section = c.ClassSchedule.Section.Code,
                    type = ConstantHelpers.SESSION_TYPE.VALUES[c.ClassSchedule.SessionType]
                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<object> GetAllAsModelByTeacherIdWeekNumberAndTermId(string teacherId, int weekNumber, Guid termId)
        {
            var result = await _context.Classes
                .Where(x => x.ClassSchedule.TeacherSchedules.Any(ts => ts.TeacherId == teacherId))
                .Where(x => x.ClassSchedule.Section.CourseTerm.TermId == termId && x.WeekNumber == weekNumber)
                .Select(x => new
                {
                    course = x.ClassSchedule.Section.CourseTerm.Course.FullName,
                    section = x.ClassSchedule.Section.Code,
                    date = x.StartTime.ToLocalDateFormat(),
                    schedule = $"{x.StartTime.ToLocalTimeFormat()} - {x.EndTime.ToLocalTimeFormat()}",
                    duration = x.EndTime.Subtract(x.StartTime).TotalHours,
                    status = x.EndTime < DateTime.UtcNow ? (x.IsDictated ? ConstantHelpers.CLASS_STATES.TAKEN : ConstantHelpers.CLASS_STATES.NOT_TAKEN) : ConstantHelpers.CLASS_STATES.WAITING
                }).ToListAsync();

            return result;
        }

        public async Task<ClassTemplate> GetByUserIdAndDateRange(string userId, DateTime start, DateTime end, Guid? exceptionId = null)
        {
            var query = _context.Classes
                .Where(x => x.StartTime < end && start < x.EndTime)
                .AsQueryable();

            if (exceptionId.HasValue)
                query = query.Where(c => c.Id != exceptionId.Value);

            var isStudent = await _context.Students.AnyAsync(x => x.UserId == userId);

            var isTeacher = await _context.Teachers.AnyAsync(x => x.UserId == userId);

            if (isStudent)
            {
                query = query.Where(x => x.ClassSchedule.Section.StudentSections.Any(y => y.Student.UserId == userId));
            }

            if (isTeacher)
            {
                query = query.Where(x => x.ClassSchedule.Section.TeacherSections.Any(y => y.TeacherId == userId));
            }

            var data = await query
                .Select(x => new ClassTemplate
                {
                    Id = x.Id,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    SectionId = x.SectionId,
                    SectionCode = x.Section.Code,
                    CourseId = x.Section.CourseTerm.CourseId,
                    TermId = x.Section.CourseTerm.TermId,
                    TermName = x.Section.CourseTerm.Term.Name,
                    CourseCode = x.Section.CourseTerm.Course.Code,
                    CourseName = x.Section.CourseTerm.Course.Name,
                })
                .FirstOrDefaultAsync();

            return data;
        }

        public async Task<Class> GetWithTeacherSchedules(Guid id)
        {
            return await _context.Classes
                .IgnoreQueryFilters()
                .Where(x => x.Id == id)
                               .Include(x => x.ClassSchedule.Section.CourseTerm.Course)
                               .Include(x => x.Classroom)
                               .Include(x => x.ClassSchedule.TeacherSchedules)
                               .ThenInclude(x => x.Teacher.User)
                               .FirstOrDefaultAsync();
        }

        public async Task<int> Count(Guid? studentId = null, Guid? termId = null, string teacherId = null, Guid? classroomId = null, DateTime? start = null, DateTime? end = null, bool? isDictated = null, Guid? sectionId = null)
        {
            var query = _context.Classes
                   .Include(x => x.ClassSchedule.Section.CourseTerm.Course)
                   .Include(x => x.Classroom)
                   .AsNoTracking();

            if (studentId.HasValue)
                query = query.Where(x => x.ClassSchedule.Section.StudentSections.Any(ss => ss.StudentId == studentId.Value));

            if (termId.HasValue)
                query = query.Where(x => x.ClassSchedule.Section.CourseTerm.TermId == termId.Value);

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Include(x => x.Section.TeacherSections).Where(x => x.ClassSchedule.TeacherSchedules.Any(ts => ts.TeacherId == teacherId));

            if (classroomId.HasValue)
                query = query.Where(x => x.ClassroomId == classroomId.Value);

            if (start.HasValue)
                query = query.Where(x => x.StartTime >= start);

            if (end.HasValue)
                query = query.Where(x => x.EndTime <= end);

            if (isDictated.HasValue)
                query = query.Where(x => x.IsDictated == isDictated.Value);

            if (sectionId.HasValue)
                query = query.Where(x => x.SectionId == sectionId.Value);

            return await query.CountAsync();
        }

        public Task<IEnumerable<Class>> GetAll(Guid? studentId = null, Guid? termId = null, string teacherId = null, Guid? classroomId = null, DateTime? start = null, DateTime? end = null)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalClassesBySectionId(Guid sectionId)
        {
            var totalClasses = await _context.Classes.Where(x => x.SectionId == sectionId).CountAsync();

            return totalClasses;
        }

        public async Task<object> GetSchedulesHome(Guid studentId, Guid termId, DateTime start, DateTime end)
        {
            var query = _context.Classes
                .Where(x => x.StartTime >= start && x.EndTime <= end)
                .AsNoTracking();

            query = query.Where(x => x.ClassSchedule.Section.CourseTerm.TermId == termId);

            query = query.Where(x => x.ClassSchedule.Section.StudentSections.Any(y => y.StudentId == studentId));

            var schedules = await query
                .OrderBy(x => x.StartTime)
                .Select(x => new
                {
                    id = x.Id,
                    title = $"{x.ClassSchedule.Section.CourseTerm.Course.Code}-{x.ClassSchedule.Section.CourseTerm.Course.Name} ({x.ClassSchedule.Section.Code})",
                    description = x.Classroom.Description,
                    allDay = false,
                    day = x.StartTime.ToLocalDateFormat(),
                    start = x.StartTime.ToLocalTimeFormat(),
                    end = x.EndTime.ToLocalTimeFormat()
                }).ToListAsync();

            return schedules;
        }

        protected virtual bool DateTimeConflict(DateTime startA, DateTime endA, DateTime startB, DateTime endB)
        {
            return startA < endB && startB < endA;
        }

        public async Task<bool> GetExistClassRoom(Guid id, Guid classroomId, DateTime starTime, DateTime endTime)
        {
            return await _context.Classes.AnyAsync(c => c.Id != id && c.ClassroomId == classroomId && DateTimeConflict(c.StartTime, c.EndTime, starTime, endTime));
        }

        public async Task<Class> GetConflictedClass(Guid id, string teacherId, DateTime starTime, DateTime endTime)
        {
            return await _context.Classes.Include(c => c.ClassSchedule.Section.CourseTerm.Course)
                           .FirstOrDefaultAsync(c =>
                               c.Id != id &&
                               c.ClassSchedule.TeacherSchedules.Any(cs => cs.TeacherId == teacherId) && DateTimeConflict(
                                   c.StartTime, c.EndTime, starTime, endTime));
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherClassesReport(SentParameters sentParameters, Guid termId, Guid? careerId, string teacherId, Guid courseId, ClaimsPrincipal user, string startSearchDate, string endSearchDate, Guid? academicDepartmentId)
        {
            var query = _context.Classes.Where(x => x.EndTime <= DateTime.UtcNow).AsNoTracking();

            Expression<Func<Class, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.StartTime);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Career.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Section.CourseTerm.Course.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Section.Code);
                    break;
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.ClassSchedule.TeacherSchedules.Any(ts => ts.Teacher.AcademicDepartment.Faculty.DeanId == userId || ts.Teacher.AcademicDepartment.Faculty.SecretaryId == userId));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    var academicDepartments = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId).Select(x => x.Id).ToListAsync();
                    query = query.Where(x => x.ClassSchedule.TeacherSchedules.Any(ts => ts.Teacher.AcademicDepartmentId.HasValue && academicDepartments.Contains(ts.Teacher.AcademicDepartmentId.Value)));
                }
            }

            if (termId != Guid.Empty)
            {
                query = query.Where(c => c.ClassSchedule.Section.CourseTerm.TermId == termId);
            }
            //if (careerId != Guid.Empty)
            //{
            //    query = query.Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.Teacher.CareerId == careerId));
            //}
            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.Teacher.AcademicDepartmentId == academicDepartmentId));

            if (!string.IsNullOrEmpty(teacherId) && teacherId != Guid.Empty.ToString())
            {
                query = query.Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.TeacherId == teacherId));
            }
            if (courseId != Guid.Empty)
            {
                query = query.Where(c => c.Section.CourseTerm.CourseId == courseId);
            }

            if (!string.IsNullOrEmpty(startSearchDate))
            {
                var startDate = ConvertHelpers.DatepickerToDatetime(startSearchDate);
                query = query.Where(x => x.StartTime.AddHours(-5).Date >= startDate.Date);
            }

            if (!string.IsNullOrEmpty(endSearchDate))
            {
                var endDate = ConvertHelpers.DatepickerToDatetime(endSearchDate);
                query = query.Where(x => x.EndTime.AddHours(-5).Date <= endDate.Date);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    career = x.Section.CourseTerm.Course.Career.Name,
                    teacher = string.Join("; ", x.Section.TeacherSections.Where(y => y.SectionId == x.SectionId).Select(y => y.Teacher.User.FullName).ToList()),
                    course = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    classroom = x.Classroom.Number,
                    schedule = $"{x.StartTime.ToLocalDateFormat()}- {x.StartTime.ToLocalTimeFormat()} - {x.EndTime.ToLocalTimeFormat()}",
                    students = x.ClassStudents.Count(),
                    cycle = ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.Section.CourseTerm.Course.AcademicYearCourses.Select(x => x.AcademicYear).FirstOrDefault()],
                    subject = x.UnitActivity.Name,
                    academicyear = x.Section.CourseTerm.Course.AcademicYearCourses.Select(x => x.AcademicYear).FirstOrDefault(),
                    start = x.StartTime,
                    x.IsDictated,
                    dictatedDate = x.DictatedDate.ToLocalDateTimeFormat()
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

        public async Task<List<ReportTeacherClassTemplate>> GetTeachersClassReportData(Guid termId, Guid? careerId, string teacherId, Guid courseId, ClaimsPrincipal user, string startSearchDate, string endSearchDate, Guid? academicDepartmentId)
        {
            var query = _context.Classes.Where(x => x.EndTime <= DateTime.UtcNow).AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.ClassSchedule.TeacherSchedules.Any(ts => ts.Teacher.Career.Faculty.DeanId == userId || ts.Teacher.Career.Faculty.SecretaryId == userId));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    var academicDepartments = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId).Select(x => x.Id).ToListAsync();
                    query = query.Where(x => x.ClassSchedule.TeacherSchedules.Any(ts => ts.Teacher.AcademicDepartmentId.HasValue && academicDepartments.Contains(ts.Teacher.AcademicDepartmentId.Value)));
                }

            }

            if (termId != Guid.Empty)
            {
                query = query.Where(c => c.ClassSchedule.Section.CourseTerm.TermId == termId);
            }
            //if (careerId != Guid.Empty)
            //{
            //    query = query.Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.Teacher.CareerId == careerId));
            //}
            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.Teacher.AcademicDepartmentId == academicDepartmentId));

            if (!string.IsNullOrEmpty(teacherId) && teacherId != Guid.Empty.ToString())
            {
                query = query.Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.TeacherId == teacherId));
            }
            if (courseId != Guid.Empty)
            {
                query = query.Where(c => c.Section.CourseTerm.CourseId == courseId);
            }

            if (!string.IsNullOrEmpty(startSearchDate))
            {
                var startDate = ConvertHelpers.DatepickerToDatetime(startSearchDate);
                query = query.Where(x => x.StartTime.AddHours(-5).Date >= startDate.Date);
            }

            if (!string.IsNullOrEmpty(endSearchDate))
            {
                var endDate = ConvertHelpers.DatepickerToDatetime(endSearchDate);
                query = query.Where(x => x.EndTime.AddHours(-5).Date <= endDate.Date);
            }

            var dataDB = await query
                .Select(x => new
                {
                    x.Id,
                    career = x.Section.CourseTerm.Course.Career.Name,
                    sectionId = x.SectionId,
                    course = x.Section.CourseTerm.Course.Name,
                    x.StartTime,
                    x.EndTime,
                    section = x.Section.Code,
                    students = x.ClassStudents.Count(),
                    subject = x.UnitActivity.Name,
                    academicyear = x.Section.CourseTerm.Course.AcademicYearCourses.Select(x => x.AcademicYear).FirstOrDefault(),
                    start = x.StartTime,
                    virtualClass = x.VirtualClassId.HasValue ? x.VirtualClass.Name : "-",
                    x.IsDictated,
                    x.DictatedDate,
                    classroom = x.Classroom.Code
                })
                .OrderBy(x => x.start)
                .ThenBy(x => x.academicyear)
                .ToListAsync();

            var sectionsIds = dataDB.Select(x => x.sectionId).ToList();
            var teachers = await _context.TeacherSections.Where(x => sectionsIds.Contains(x.SectionId)).Select(x => new
            {
                x.SectionId,
                teacher = x.Teacher.User.FullName
            })
            .ToListAsync();

            var data = dataDB
                .Select(x => new ReportTeacherClassTemplate
                {
                    career = x.career,
                    teacher = string.Join("; ", teachers.Where(y => y.SectionId == x.sectionId).Select(y => y.teacher).ToList()),
                    course = x.course,
                    section = x.section,
                    classroom = x.classroom,
                    schedule = $"{x.StartTime.ToLocalDateFormat()}- {x.StartTime.ToLocalTimeFormat()} - {x.EndTime.ToLocalTimeFormat()}",
                    students = x.students,
                    cycle = ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.academicyear],
                    subject = x.subject,
                    academicyear = x.academicyear,
                    start = x.start,
                    IsDictated = x.IsDictated,
                    DictatedDate = x.DictatedDate.ToLocalDateTimeFormat(),
                    VirtualClass = x.virtualClass,
                })
                .ToList();

            return data;
        }

        public async Task CreateClassJob()
        {
            var classes = new List<Class>();
            var sections = _context.Sections
                .Include(x => x.CourseTerm.Term)
                .Include(x => x.StudentSections)
                .Include(x => x.ClassSchedules)
                .Where(x => x.StudentSections.Any())
                .ToList();

            for (var i = 0; i < sections.Count; i++)
            {
                var section = sections[i];
                var classSchedules = section.ClassSchedules.ToList();
                var term = section.CourseTerm.Term;
                var weeks = Math.Floor((term.ClassEndDate - term.ClassStartDate).TotalDays / 7);

                for (var j = 0; j < weeks; j++)
                {
                    var classNumber = 1;

                    for (var k = 0; k < classSchedules.Count(); k++)
                    {
                        var classSchedule = classSchedules[k];
                        var time = term.ClassStartDate.ToDefaultTimeZone().Date.AddDays(j * 7).AddDays(classSchedule.WeekDay);
                        var @class = new Class()
                        {
                            ClassNumber = classNumber++,
                            WeekNumber = (j + 1),
                            StartTime = time.Add(classSchedule.StartTime.ToLocalTimeSpanUtc()).ToUniversalTime(),
                            EndTime = time.Add(classSchedule.EndTime.ToLocalTimeSpanUtc()).ToUniversalTime(),
                            ClassroomId = classSchedule.ClassroomId,
                            ClassScheduleId = classSchedule.Id,
                            SectionId = classSchedule.SectionId
                        };

                        classes.Add(@class);
                    }
                }
            }

            await _context.Classes.AddRangeAsync(classes);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Class>> GetClassesByByClassScheduleAndSectionIdAndClassroomId(Guid classScheduleId, Guid sectionId, Guid classroomId)
        {
            var result = await _context.Classes.Where(x => x.ClassScheduleId == classScheduleId && x.SectionId == sectionId && x.ClassroomId == classroomId).ToListAsync();
            return result;
        }

        public async Task<List<ClassExcelTemplate>> GetTeacherClassesExcelData(Guid termId, Guid? careerId, string teacherId, Guid courseId, ClaimsPrincipal user, string startSearchDate, string endSearchDate, Guid? academicDepartmentId)
        {
            var query = _context.Classes.AsQueryable();

            if(user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {

                    if (!string.IsNullOrEmpty(userId))
                    {
                        query = query.Where(x => x.ClassSchedule.TeacherSchedules.Any(ts => ts.Teacher.Career.Faculty.DeanId == userId || ts.Teacher.Career.Faculty.SecretaryId == userId));
                    }
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    var academicDepartments = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId).Select(x => x.Id).ToListAsync();
                    query = query.Where(x => x.ClassSchedule.TeacherSchedules.Any(ts => ts.Teacher.AcademicDepartmentId.HasValue && academicDepartments.Contains(ts.Teacher.AcademicDepartmentId.Value)));
                }
            }

          

            if (termId != Guid.Empty)
            {
                query = query.Where(c => c.ClassSchedule.Section.CourseTerm.TermId == termId);
            }
            //if (careerId != Guid.Empty)
            //{
            //    query = query.Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.Teacher.CareerId == careerId));
            //}
            if (academicDepartmentId.HasValue && academicDepartmentId != Guid.Empty)
                query = query.Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.Teacher.AcademicDepartmentId == academicDepartmentId));

            if (!string.IsNullOrEmpty(teacherId) && teacherId != Guid.Empty.ToString())
            {
                query = query.Where(c => c.ClassSchedule.TeacherSchedules.Any(ts => ts.TeacherId == teacherId));
            }
            if (courseId != Guid.Empty)
            {
                query = query.Where(c => c.Section.CourseTerm.CourseId == courseId);
            }


            if (!string.IsNullOrEmpty(startSearchDate))
            {
                var startDate = ConvertHelpers.DatepickerToDatetime(startSearchDate);
                query = query.Where(x => x.StartTime.AddHours(-5).Date >= startDate.Date);
            }

            if (!string.IsNullOrEmpty(endSearchDate))
            {
                var endDate = ConvertHelpers.DatepickerToDatetime(endSearchDate);
                query = query.Where(x => x.EndTime.AddHours(-5).Date <= endDate.Date);
            }


            var data = await query
                .Select(x => new ClassExcelTemplate
                {
                    Schedule = $"{x.StartTime.ToLocalDateFormat()}- {x.StartTime.ToLocalTimeFormat()} - {x.EndTime.ToLocalTimeFormat()}",
                    Career = x.Section.CourseTerm.Course.Career.Name,
                    Course = x.Section.CourseTerm.Course.Name,
                    AcademicYear = x.Section.CourseTerm.Course.AcademicYearCourses.Select(x => x.AcademicYear).FirstOrDefault(),
                    SectionCode = x.Section.Code,
                    StudentsCount = x.ClassStudents.Count(),
                    Teachers = string.Join("; ", x.Section.TeacherSections.Where(y => y.SectionId == x.SectionId).Select(y => y.Teacher.User.FullName).ToList()),
                    AcademicDepartments = string.Join("; ", x.Section.TeacherSections.Where(y => y.SectionId == x.SectionId).GroupBy(y=>y.Teacher.AcademicDepartment.Name).Select(y => y.Key).ToList()),
                    Subject = x.UnitActivity.Name,
                    Dictated = x.DictatedDate.HasValue ? "Sí" : "No",
                    Classroom = x.Classroom.Code,
                    VirtualClass = x.VirtualClassId.HasValue ? x.VirtualClass.Name : "-"
                })
                .ToListAsync();

            return data;
        }

        public async Task<List<ClassByPlanExcelTemplate>> GetReportClassAssistance(Guid termId, Guid careerId, Guid curriculumId, DateTime endDate)
        {
            var query = _context.Sections.Where(x => x.CourseTerm.TermId == termId && x.CourseTerm.Course.CareerId == careerId && x.CourseTerm.Course.AcademicYearCourses
            .Any(y => y.CurriculumId == curriculumId)).AsNoTracking();

            var academicYears = await _context.AcademicYearCourses.Where(x => x.CurriculumId == curriculumId).ToListAsync();

            var result = await query
                .Select(x => new ClassByPlanExcelTemplate
                {
                    CourseId = x.CourseTerm.CourseId,
                    Course = x.CourseTerm.Course.Name,
                    CourseCode = x.CourseTerm.Course.Code,
                    Section = x.Code,
                    Teachers = string.Join(", ", x.TeacherSections.Select(y => y.Teacher.User.FullName).ToList()),
                    //
                    Scheduled = _context.Classes.Where(y => y.ClassSchedule.SectionId == x.Id && y.EndTime <= endDate).Count(),
                    Taken = _context.Classes.Where(y => y.ClassSchedule.SectionId == x.Id && y.IsDictated && y.EndTime <= endDate).Count()
                })
                .ToListAsync();

            foreach (var item in result)
                item.Semester = academicYears.Where(x => x.CourseId == item.CourseId).Select(x => ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x.AcademicYear]).FirstOrDefault();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetClassesToNeedReschedule(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId)
        {
            var query = _context.Classes.Where(x => x.ClassSchedule.Section.CourseTerm.TermId == termId && x.NeedReschedule && x.ClassSchedule.TeacherSchedules.Any(y => y.TeacherId == teacherId)).AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.StartTime)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    section = x.Section.Code,
                    courseCode = x.Section.CourseTerm.Course.Code,
                    course = x.Section.CourseTerm.Course.Name,
                    day = x.StartTime.ToLocalDateFormat(),
                    schedule = $"{x.StartTime.ToLocalTimeFormat()}-{x.EndTime.ToLocalTimeFormat()}"
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

        public async Task<int> GetMaxAbsencesBySection(Guid sectionId)
        {
            var classes = await _context.Classes.Where(x => x.ClassSchedule.SectionId == sectionId).CountAsync();
            var maxAbsencesPercentage = await _context.Sections.Where(x => x.Id == sectionId).Select(x => x.CourseTerm.Term.AbsencePercentage).FirstOrDefaultAsync();
            var result = (int)Math.Ceiling(classes * (maxAbsencesPercentage / 100.0));
            return result;
        }
    }
}