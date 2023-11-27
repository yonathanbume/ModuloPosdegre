using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassStudent;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class ClassStudentRepository : Repository<ClassStudent>, IClassStudentRepository
    {
        public ClassStudentRepository(AkdemicContext context) : base(context)
        {
        }

        #region PRIVATE
        private Expression<Func<SectionAbsenceDetailDataTableTemplate, dynamic>> GetSectionAbsenceDetailDataTableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Week);
                //case "1":
                //    return ((x) => x.Code);
                //case "2":
                //    return ((x) => x.Career);
                //case "3":
                //    return ((x) => x.AcademicProgram);
                //case "4":
                //    return ((x) => x.Intents);
                //case "5":
                //    return ((x) => x.Grade);
                //case "6":
                //    return ((x) => x.Approbed);
                default:
                    return ((x) => x.Week);
            }
        }
        private Func<SectionAbsenceDetailDataTableTemplate, string[]> GetSectionAbsenceDetailDataTableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.ClassId+"",
                x.Week+"",
                x.SessionNumber+"",
                x.Date+"",
                x.WeekDay+"",
                x.StartTime+"",
                x.EndTime+"",
                x.IsAbsent+"",
        };
        }
        private async Task<DataTablesStructs.ReturnedData<SectionAbsenceDetailDataTableTemplate>> GetSectionAbsenceDetailDataTable(
          DataTablesStructs.SentParameters sentParameters, Guid sid, Guid aid, int filter,
          Expression<Func<SectionAbsenceDetailDataTableTemplate, SectionAbsenceDetailDataTableTemplate>> selectPredicate = null,
          Expression<Func<SectionAbsenceDetailDataTableTemplate, dynamic>> orderByPredicate = null,
          Func<SectionAbsenceDetailDataTableTemplate, string[]> searchValuePredicate = null)
        {
            var student = await _context.Students.Include(x => x.Career).Where(x => x.Id == aid).FirstOrDefaultAsync();

            var query = _context.ClassStudents
                 //.Where(x => x.Class.SectionId.Equals(sid) && x.StudentId.Equals(student.Id) && x.Class.StartTime.ToLocalTime() < DateTime.Now)
                 //.OrderByDescending(x => x.Class.StartTime)
                 .AsQueryable();

            if (filter != -1)
                query = query.Where(x => x.IsAbsent == ConstantHelpers.ASSISTANCE_STATES.INVERSE_VALUES[filter]);

            var queryclient = await query.Select(x => new
            {
                ClassId = x.Class.Id,
                Week = x.Class.WeekNumber,
                SessionNumber = x.Class.ClassNumber,
                Date = x.Class.StartTime.ToLocalDateFormat(),
                WeekDay = ConstantHelpers.WEEKDAY.VALUES[x.Class.ClassSchedule.WeekDay],
                StartTime = x.Class.StartTime.ToLocalDateTimeFormat(),
                DStartTime = x.Class.StartTime,
                EndTime = x.Class.EndTime.ToLocalDateTimeFormat(),
                IsAbsent = x.IsAbsent,
                x.Class.SectionId,
                x.StudentId
            }).ToListAsync();

            queryclient =  queryclient
                .Where(x => x.SectionId.Equals(sid) && x.StudentId.Equals(student.Id) && x.DStartTime.ToLocalTime() < DateTime.Now)
                .ToList();

            var recordsFiltered = queryclient.Count();

            var result = queryclient
                .OrderByDescending(x => x.DStartTime)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new SectionAbsenceDetailDataTableTemplate
                {
                    ClassId = x.ClassId,
                    Week = x.Week,
                    SessionNumber = x.SessionNumber,
                    Date = x.Date,
                    WeekDay = x.WeekDay,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    IsAbsent = x.IsAbsent
                })
                //.OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .ToList();

            var recordsTotal = result.Count;

            return new DataTablesStructs.ReturnedData<SectionAbsenceDetailDataTableTemplate>
            {
                Data = result,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            //return await result.ToDataTables(sentParameters, selectPredicate);
        }
        #endregion

        #region PUBLIC
        public async Task<DataTablesStructs.ReturnedData<SectionAbsenceDetailDataTableTemplate>> GetSectionAbsenceDetailDataTable(DataTablesStructs.SentParameters parameters, Guid sid, Guid aid, int filter)
        {
            return await GetSectionAbsenceDetailDataTable(parameters, sid, aid, filter, null, GetSectionAbsenceDetailDataTableOrderByPredicate(parameters), GetSectionAbsenceDetailDataTableSearchValuePredicate());
        }

        public async Task<IEnumerable<object>> GetStudentAssistances(Class @class, string teacherId, Guid? classId = null)
        {
            if (classId.HasValue && classId != Guid.Empty)
            {
                @class = await _context.Classes.Include(x => x.ClassSchedule).FirstOrDefaultAsync(x => x.Id == classId);
            }

            var maxAbsencesPercentage = await _context.Sections.Where(x => x.Id == @class.SectionId).Select(x => x.CourseTerm.Term.AbsencePercentage).FirstOrDefaultAsync();

            bool.TryParse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.ENABLED_SPECIAL_ABSENCE_PERCENTAGE), out var enabledSpecialAbsencePercentage);

            if (enabledSpecialAbsencePercentage)
            {
                float.TryParse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE), out var specialAbsencePercentage);
                var absencePercentageDescription = await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE_DESCRIPTION);
                var courseName = await _context.Sections.Where(x => x.Id == @class.SectionId).Select(x => x.CourseTerm.Course.Name).FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(absencePercentageDescription) && courseName.ToLower().Trim().Contains(absencePercentageDescription.Trim().ToLower()))
                    maxAbsencesPercentage = specialAbsencePercentage;
            }

            var classStudents = await _context.ClassStudents.Where(x => x.Class.SectionId == @class.SectionId)
                .Select(x => new
                {
                    x.StudentId,
                    x.IsAbsent,
                    x.Class.ClassSchedule.SectionGroupId
                })
                .ToListAsync();

            var classesBySubGroup = await _context.Classes.Where(x => x.ClassSchedule.SectionId == @class.SectionId)
                .GroupBy(x => x.ClassSchedule.SectionGroupId)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                })
                .ToListAsync();

            var query = _context.StudentSections
                .Where(x=>x.SectionId == @class.SectionId)
                .AsNoTracking();

            var studentSectionsTpm = await query
                .Select(x => new
                {
                    x.StudentId,
                    x.SectionGroupId
                })
                .ToListAsync();

            var savedClassStudentsDB = await _context.ClassStudents.Where(x => x.ClassId == @class.Id).Select(x => new
            {
                id = x.Id,
                isAbsent = x.IsAbsent,
                classId = x.ClassId,
                studentId = x.StudentId,
                studentName = x.Student.User.FullName,
            }).OrderBy(x => x.studentName).ToListAsync();

            var classSchedule = await _context.ClassSchedules.Where(x => x.Id == @class.ClassScheduleId).FirstOrDefaultAsync();

            query = query.Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN);

            if (classSchedule.SessionType == ConstantHelpers.SESSION_TYPE.PRACTICE && classSchedule.SectionGroupId.HasValue)
            {
                query = query.Where(x => x.SectionGroupId == classSchedule.SectionGroupId);
            }

            var dataDB = await query.Select(x => new
            {
                classId = @class.Id,
                studentId = x.StudentId,
                studentName = x.Student.User.FullName,
                x.SectionGroupId,
            })
                .OrderBy(x => x.studentName)
                .ToListAsync();

            var result = dataDB.Select(x => new
            {
                id = savedClassStudentsDB.Any(y => y.studentId == x.studentId) ? savedClassStudentsDB.Where(y => y.studentId == x.studentId).Select(y => y.id).FirstOrDefault() : (Guid?)null,
                isAbsent = savedClassStudentsDB.Any(y => y.studentId == x.studentId) ? savedClassStudentsDB.Where(y => y.studentId == x.studentId).Select(y => y.isAbsent).FirstOrDefault() : false,
                x.classId,
                x.studentId,
                x.studentName,
                DPI = classesBySubGroup.Where(y => !y.Key.HasValue || y.Key == x.SectionGroupId).Sum(y => y.count) == 0 ?
                        false :
                        ((decimal)classStudents.Where(y => y.StudentId == x.studentId && (!y.SectionGroupId.HasValue || y.SectionGroupId == x.SectionGroupId) && y.IsAbsent).Count() / (decimal)classesBySubGroup.Where(y => !y.Key.HasValue || y.Key == x.SectionGroupId).Sum(x => x.count)) * 100M > (decimal)maxAbsencesPercentage,
                absencePercentage = classesBySubGroup.Where(y => !y.Key.HasValue || y.Key == x.SectionGroupId).Sum(y => y.count) == 0 ?
                        0 :
                        Math.Round((((decimal)classStudents.Where(y => y.StudentId == x.studentId && (!y.SectionGroupId.HasValue || y.SectionGroupId == x.SectionGroupId) && y.IsAbsent).Count() / (decimal)classesBySubGroup.Where(y => !y.Key.HasValue || y.Key == x.SectionGroupId).Sum(x => x.count)) * 100M),2),
                absences = classStudents.Where(y => y.StudentId == x.studentId && y.IsAbsent && (!y.SectionGroupId.HasValue || y.SectionGroupId == studentSectionsTpm.Where(z => z.StudentId == x.studentId).Select(z => z.SectionGroupId).FirstOrDefault())).Count(),
                maxAbsences = (int)Math.Floor((maxAbsencesPercentage / 100.0) * classesBySubGroup.Where(y => y.Key == null || y.Key == studentSectionsTpm.Where(z => z.StudentId == x.studentId).Select(z => z.SectionGroupId).FirstOrDefault()).Sum(z => z.count))
            })
                .ToList();

            return result;
        }

        public async Task<object> GetClassStudentsOldAssistance(Guid classId, string teacherId = null)
        {
            var @class = await _context.Classes
                .Include(x => x.Section.CourseTerm)
                .FirstOrDefaultAsync(x => x.Id == classId);

            var term = await _context.Terms
                .FindAsync(@class.Section.CourseTerm.TermId);

            var classes = await _context.Classes.Where(x => x.SectionId == @class.SectionId).CountAsync();
            classes = classes == 0 ? classes = 1 : classes;

            var savedClassStudents = await _context.ClassStudents
                .Where(x => x.ClassId == @class.Id)
                .Select(x => new
                {
                    id = x.Id,
                    isAbsent = x.IsAbsent,
                    classId = x.ClassId,
                    studentId = x.StudentId,
                    studentName = x.Student.User.FullName,
                    absences = x.Student.ClassStudents.Count(y => y.IsAbsent && x.Class.ClassSchedule.SectionId == y.Class.ClassSchedule.SectionId && y.Class.IsDictated),
                    maxAbsences = Math.Floor((term.AbsencePercentage / 100.0) * classes)
                }).OrderBy(x => x.studentName).ToListAsync();

            if (savedClassStudents.Count != 0)
            {
                var listWithRepeateds = savedClassStudents.ToList();
                listWithRepeateds.Clear();

                foreach (var item in savedClassStudents)
                {
                    if (listWithRepeateds.Any(x => x.studentId == item.studentId && x.id != item.id))
                        continue;

                    listWithRepeateds.Add(item);
                }

                return listWithRepeateds;
            }

            var query = _context.StudentSections
                .Where(x => x.SectionId == @class.SectionId && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .AsNoTracking();

            //var sectionGroupId = await _context.TeacherSections
            //    .Where(x => x.SectionId == @class.SectionId && x.TeacherId == teacherId && !x.IsPrincipal)
            //    .Select(x => x.SectionGroupId)
            //    .FirstOrDefaultAsync();

            //if (sectionGroupId != null) query = query.Where(x => x.SectionGroupId == sectionGroupId);

            var result = await query.Select(x => new
            {
                id = (Guid?)null,
                isAbsent = false,
                classId = @class.Id,
                studentId = x.StudentId,
                studentName = x.Student.User.FullName,
                absences = x.Student.ClassStudents.Count(y => y.IsAbsent && @class.ClassSchedule.SectionId == y.Class.ClassSchedule.SectionId && y.Class.IsDictated),
                maxAbsences = Math.Floor((term.AbsencePercentage / 100.0) * classes)
            }).OrderBy(x => x.studentName).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<ClassStudent>> GetAll(Guid? sectionId = null, Guid? studentId = null, DateTime? startTime = null, DateTime? endTime = null, bool? absent = null, Guid? classId = null, string teacherId = null)
        {
            var query = _context.ClassStudents
                .AsNoTracking();
            if (sectionId.HasValue)
                query = query.Where(x => x.Class.SectionId == sectionId.Value);
            if (studentId.HasValue)
                query = query.Where(x => x.StudentId == studentId.Value);
            if (startTime.HasValue)
                query = query.Where(x => x.Class.StartTime > startTime.Value);
            if (endTime.HasValue)
                query = query.Where(x => x.Class.EndTime < endTime.Value);
            if (absent.HasValue)
                query = query.Where(x => x.IsAbsent == absent.Value);

            if (!string.IsNullOrEmpty(teacherId))
            {
                var teacherSection = await _context.TeacherSections.Where(x => x.TeacherId == teacherId && x.Section.Classes.Any(y => y.Id == classId)).FirstOrDefaultAsync();
                //if (teacherSection != null && teacherSection.SectionGroupId.HasValue)
                //{
                //    var students = await _context.StudentSections.Where(x => x.SectionGroupId == teacherSection.SectionGroupId && x.Section.Classes.Any(y => y.Id == classId)).ToArrayAsync();
                //    query = query.Where(x => students.Any(y => y.StudentId == x.StudentId));
                //}
            }

            if (classId.HasValue)
                query = query
                    .Where(x => x.ClassId == classId.Value);

            return await query
                    //.Include(x => x.Class.Section)
                    //.Include(x => x.Student.User)
                    //.Include(x => x.Student.ClassStudents)
                    .Include(x => x.Class.ClassSchedule)
                    .ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesAssistanceDetailByStudentAndSectionDatatable(DataTablesStructs.SentParameters parameters, Guid studentId, Guid sectionId)
        {
            Expression<Func<ClassStudent, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.Class.StartTime;
                    break;
            }

            var studentSection = await _context.StudentSections.Where(x => x.StudentId == studentId && x.SectionId == sectionId).FirstOrDefaultAsync();

            var query = _context.ClassStudents
                    .Where(x => x.Class.SectionId == sectionId && x.StudentId == studentId && (!x.Class.ClassSchedule.SectionGroupId.HasValue || x.Class.ClassSchedule.SectionGroupId == studentSection.SectionGroupId))
                    .AsTracking();

            var recordsFiltered = await query.CountAsync();

            var model = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    classId = x.Class.Id,
                    week = x.Class.WeekNumber,
                    sessionNumber = x.Class.ClassNumber,
                    date = x.Class.StartTime.ToLocalTime().ToShortDateString(),
                    weekDay = CORE.Helpers.ConstantHelpers.WEEKDAY.VALUES[x.Class.ClassSchedule.WeekDay],
                    startTime = x.Class.StartTime.ToLocalTime().ToString(CORE.Helpers.ConstantHelpers.FORMATS.TIME, System.Globalization.CultureInfo.InvariantCulture),
                    endTime = x.Class.EndTime.ToLocalTime().ToString(CORE.Helpers.ConstantHelpers.FORMATS.TIME, System.Globalization.CultureInfo.InvariantCulture),
                    isAbsent = x.IsAbsent
                }).ToListAsync();

            var data = model.Select(x => new
            {
                x.classId,
                x.week,
                x.sessionNumber,
                x.date,
                x.weekDay,
                x.startTime,
                x.endTime,
                x.isAbsent
            }).ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetClassStudentSelect2ClientSide(string userId, Guid sectionId, bool isAbsent = false)
        {
            var dbData = await _context.ClassStudents
                .Where(x => x.Student.UserId == userId && x.Class.SectionId == sectionId && x.IsAbsent == isAbsent)
                .Select(x => new
                {
                    x.Id,
                    Date = x.Class.StartTime
                }).ToListAsync();

            var result = dbData
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = $"Clase del {x.Date.ToLocalDateFormat()}"
                }).ToList();

            return result;
        }


        public async Task<List<ClassStudent>> GetClassesBySectionId(Guid sectionId)
        {
            var classes = await _context.ClassStudents
                .Include(x => x.Class)
                .Where(x => x.Class.SectionId == sectionId).ToListAsync();

            return classes;
        }

        public async Task CreateClassStudentsJob(Guid? sectionId, Guid termId)
        {
            var query = _context.Classes
                .Include(x => x.ClassSchedule.Section.StudentSections)
                .Where(x => x.Section.CourseTerm.TermId == termId)
                .AsQueryable();
            if (sectionId.HasValue)
                query = query.Where(x => x.SectionId == sectionId.Value);
            var classes = await query.ToListAsync();
            var classStudents = classes.SelectMany(c =>
                c.ClassSchedule.Section.StudentSections.Select(cs => new ClassStudent
                {
                    ClassId = c.Id,
                    StudentId = cs.StudentId,
                    IsAbsent = false
                })).ToList();
            await _context.ClassStudents.AddRangeAsync(classStudents);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAssistanceByClassScheduleId(Guid classScheduleId, Guid sectionId)
            => await _context.ClassStudents.AnyAsync(x => x.Class.ClassScheduleId == classScheduleId && x.Class.SectionId == sectionId);

        public async Task<List<ClassStudentReportTemplate>> GetClassStudentReport(Guid sectionId)
        {
            var clasesBySubGroup = await _context.Classes.Where(x => x.SectionId == sectionId)
                .GroupBy(x => x.ClassSchedule.SectionGroupId)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                })
                .ToListAsync();

            var classStudents = await _context.ClassStudents.Where(x => x.Class.SectionId == sectionId)
                .Select(x => new
                {
                    x.ClassId,
                    x.StudentId,
                    x.IsAbsent,
                    x.Class.ClassSchedule.SectionGroupId
                })
                .ToListAsync();

            var absencePercentage = await _context.Sections.Where(x => x.Id == sectionId).Select(x => x.CourseTerm.Term.AbsencePercentage).FirstOrDefaultAsync();

            var studentSections = await _context.StudentSections.Where(x => x.SectionId == sectionId)
                .OrderBy(x=>x.Student.User.FullName)
                .Select(x => new ClassStudentReportTemplate
                {
                    StudentId = x.StudentId,
                    SectionGroupId = x.SectionGroupId,
                    UserName = x.Student.User.UserName,
                    FullName = x.Student.User.FullName,
                })
                .ToListAsync();

            foreach (var studentSection in studentSections)
            {
                studentSection.ClassStudentDetail = classStudents.Where(x => x.StudentId == studentSection.StudentId)
                    .Select(x=> new ClassStudentDetailReportTemplate
                    {
                        ClassId = x.ClassId,
                        IsAbsent = x.IsAbsent
                    })
                    .ToList();

                studentSection.Absences = classStudents.Where(x => x.StudentId == studentSection.StudentId && (!x.SectionGroupId.HasValue || x.SectionGroupId == studentSection.SectionGroupId) && x.IsAbsent).Count();
                studentSection.Assisted = classStudents.Where(x => x.StudentId == studentSection.StudentId && (!x.SectionGroupId.HasValue || x.SectionGroupId == studentSection.SectionGroupId) && !x.IsAbsent).Count();
                studentSection.MaxAbsences = (int)Math.Floor((decimal)clasesBySubGroup.Where(x => !x.Key.HasValue || x.Key == studentSection.SectionGroupId).Sum(x => x.count) * (decimal)absencePercentage / 100M);
                studentSection.Dictated = classStudents.Where(x => x.StudentId == studentSection.StudentId && (!x.SectionGroupId.HasValue || x.SectionGroupId == studentSection.SectionGroupId)).Count();
                studentSection.TotalClasses = clasesBySubGroup.Where(x => !x.Key.HasValue || x.Key == studentSection.SectionGroupId).Sum(x => x.count);
                studentSection.AbsencesPercentage = studentSection.TotalClasses == 0 ? 0 : Math.Round(((decimal)studentSection.Absences / (decimal)studentSection.TotalClasses * 100M),1,MidpointRounding.AwayFromZero);
            }

            return studentSections;
        }

        public async Task AssignDPI(Guid sectionId)
        {
            var term = await _context.Sections.Where(x => x.Id == sectionId).Select(x => x.CourseTerm.Term).FirstOrDefaultAsync();

            var maxAbsencesPercentage = term.AbsencePercentage;

            bool.TryParse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.ENABLED_SPECIAL_ABSENCE_PERCENTAGE), out var enabledSpecialAbsencePercentage);

            if (enabledSpecialAbsencePercentage)
            {
                float.TryParse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE), out var specialAbsencePercentage);
                var absencePercentageDescription = await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SPECIAL_ABSENCE_PERCENTAGE_DESCRIPTION);
                var courseName = await _context.Sections.Where(x => x.Id == sectionId).Select(x => x.CourseTerm.Course.Name).FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(absencePercentageDescription) && courseName.ToLower().Trim().Contains(absencePercentageDescription.Trim().ToLower()))
                    maxAbsencesPercentage = specialAbsencePercentage;
            }

            var classesBySubGroup = await _context.Classes.Where(x => x.ClassSchedule.SectionId == sectionId)
                .GroupBy(x => x.ClassSchedule.SectionGroupId)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                })
                .ToListAsync();

            var studentSections = await _context.StudentSections.Where(x => x.SectionId == sectionId && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN).ToListAsync();
            var classStudents = await _context.ClassStudents.Where(x => x.Class.SectionId == sectionId).ToListAsync();

            foreach (var studentSection in studentSections)
            {
                if(studentSection.Status == ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS || studentSection.Status == ConstantHelpers.STUDENT_SECTION_STATES.DPI)
                {
                    var totalClassesByStudent = classesBySubGroup.Where(x => !x.Key.HasValue || x.Key == studentSection.SectionGroupId).Sum(x=>x.count);
                    var absencesByStudent = classStudents.Where(x => x.StudentId == studentSection.StudentId && x.IsAbsent).Count();

                    decimal absencesPercentage = 0M;

                    if(totalClassesByStudent != 0M)
                        absencesPercentage = absencesByStudent * 100M / totalClassesByStudent;

                    if (absencesPercentage > (decimal)maxAbsencesPercentage)
                    {
                        studentSection.Status = ConstantHelpers.STUDENT_SECTION_STATES.DPI;
                    }
                    else
                    {
                        studentSection.Status = ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<ClassStudent>> GetClassStudentsByStudentSection(Guid studentId, Guid sectionId)
        {
            var studentSection = await _context.StudentSections.FirstOrDefaultAsync(x => x.StudentId == studentId && x.SectionId == sectionId);

            var data = await _context.ClassStudents
                    .Where(x => x.Class.SectionId == studentSection.SectionId && x.StudentId == studentSection.StudentId
                    && (!x.Class.ClassSchedule.SectionGroupId.HasValue || x.Class.ClassSchedule.SectionGroupId == studentSection.SectionGroupId))
                    .Include(x => x.Class)
                    .ThenInclude(x => x.ClassSchedule)
                    .ToListAsync();

            return data;
        }
        #endregion
    }
}
