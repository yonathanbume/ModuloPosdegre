using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.WorkingDay;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Teacher;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public sealed class WorkingDayRepository : Repository<WorkingDay>, IWorkingDayRepository
    {
        public WorkingDayRepository(AkdemicContext context) : base(context) { }

        Task<WorkingDay> IWorkingDayRepository.GetByFilter(DateTime? date, string userId)
        {
            var query = _context.WorkingDays.AsQueryable();

            if (date.HasValue)
                query = query.Where(x => x.RegisterDate.Date == date.Value.Date);

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(x => x.UserId == userId);

            return query.FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkingDaysDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? initDatetime, DateTime? finishDatetime, string academicCoordinatorId = null, string searchValue = null)
        {
            Expression<Func<WorkingDay, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.WorkingDays.Where(w => w.RegisterDate >= initDatetime && w.RegisterDate <= finishDatetime)
                    .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                    .AsNoTracking();

            if (!string.IsNullOrEmpty(academicCoordinatorId))
            {
                var users = await _context.Teachers.Where(x => x.Career.AcademicCoordinatorId == academicCoordinatorId || x.Career.CareerDirectorId == academicCoordinatorId).Select(x => x.UserId).ToArrayAsync();
                query = query.Where(x => users.Any(y => y == x.UserId));
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.FullName.ToLower().Contains(searchValue.Trim().ToLower()));
            }

            var result = await query.GroupBy(x => x.User)
                .Select(x => new
                {
                    name = x.Key.FullName,
                    assistance = x.Count(y => y.Status != ConstantHelpers.WORKING_DAY.STATUS.ABSENT),
                    nonAssistance = x.Count(y => y.Status == ConstantHelpers.WORKING_DAY.STATUS.ABSENT),
                    firstLate = x.Count(y => y.Status == ConstantHelpers.WORKING_DAY.STATUS.LATE),
                    secondLate = x.Count(y => y.Status == ConstantHelpers.WORKING_DAY.STATUS.LATE)
                })
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .ToListAsync();

            var filterRecord = result.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = result,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = 0,
                RecordsTotal = filterRecord
            };
        }

        public async Task<object> GetReportTeachearAssistenace(DateTime initDatetime, DateTime finishDatetime)
        {
            var report = await (from w in _context.WorkingDays
                                join u in _context.Users on w.UserId equals u.Id
                                join t in _context.Teachers on u.Id equals t.UserId
                                where w.RegisterDate >= initDatetime && w.RegisterDate <= finishDatetime
                                group new { w, u } by new { w.UserId, u } into g
                                select new
                                {
                                    name = g.Key.u.FullName,
                                    //assistance = g.Count(x => !x.w.IsAbsent),
                                    //nonAssistance = g.Count(x => x.w.IsAbsent),
                                    //firstLate = g.Count(x => x.w.FirstLate != null && x.w.FirstLate.Value != false),
                                    //secondLate = g.Count(x => x.w.SecondLate != null && x.w.SecondLate.Value != false)
                                }
           ).ToListAsync();

            return report;
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetWorkingDaySelect2ClientSide(Guid termId, string userId, bool? isAbsent = false)
        {
            var query = _context.WorkingDays
                .Where(x => x.TermId == termId && x.UserId == userId)
                .AsQueryable();

            if (isAbsent.HasValue)
            {
                query = query.Where(x => (isAbsent.Value ? x.Status == ConstantHelpers.WORKING_DAY.STATUS.ABSENT : x.Status != ConstantHelpers.WORKING_DAY.STATUS.ABSENT));
            }

            var result = await query.Select(x => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.RegisterDate.ToDateFormat()
            })
            .ToArrayAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersDailyAssitance(DataTablesStructs.SentParameters sentParameters, string teacherId, DateTime startDate, string search, ClaimsPrincipal user)
        {
            Expression<Func<WorkingDay, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            if (term == null)
                term = new Term();
            var assists = _context.WorkingDays
                .Where(x => x.TermId == term.Id)
                .AsEnumerable()
                .Where(x => x.RegisterDate.ToDefaultTimeZone().Date == startDate.ToDefaultTimeZone().Date && x.TermId == term.Id)
                .OrderByDescending(y => y.RegisterDate)
                .ToArray();

            var query = _context.Teachers.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.AcademicDepartment.Career.Faculty.DeanId == userId || x.AcademicDepartment.Career.Faculty.SecretaryId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    var academicDeparments = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId).Select(x => x.Id).ToListAsync();
                    query = query.Where(x => x.AcademicDepartmentId.HasValue && academicDeparments.Contains(x.AcademicDepartmentId.Value));
                }

            }

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.UserId == teacherId);

            //if (!string.IsNullOrEmpty(search))
            //{
            //    query = query.Where(x => x.User.FullName.Trim().ToLower().Contains(search.Trim().ToLower()));
            //}

            var count = await query
                .Select(x => new TeacherPlusAssitance
                {
                    TeacherId = x.UserId,
                    MaternalSurname = x.User.MaternalSurname,
                    PaternalSurname = x.User.PaternalSurname,
                    Name = x.User.Name,
                    UserName = x.User.UserName,
                    FullName = x.User.FullName
                }, search)
                .CountAsync();

            var resultDB = await query
                .Select(
                    x => new TeacherPlusAssitance
                    {
                        TeacherId = x.UserId,
                        MaternalSurname = x.User.MaternalSurname,
                        PaternalSurname = x.User.PaternalSurname,
                        Name = x.User.Name,
                        UserName = x.User.UserName,
                        FullName = x.User.FullName
                    }
                , search)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var result = resultDB
                .Select(x => new TeacherPlusAssitance
                {
                    TeacherId = x.TeacherId,
                    FullName = x.FullName,
                    UserName = x.UserName,
                    StartDate = assists.Where(y => y.UserId == x.TeacherId && y.StartTime.HasValue).Select(y => y.StartTime.Value.ToLocalDateTimeFormatUtc()).FirstOrDefault(),
                    EndDate = assists.Where(y => y.UserId == x.TeacherId && y.Endtime.HasValue).Select(y => y.Endtime.Value.ToLocalDateTimeFormatUtc()).FirstOrDefault(),
                    Status = 0,
                    IsAbsent = assists.Where(y => y.UserId == x.TeacherId).Select(y => y.Status == ConstantHelpers.WORKING_DAY.STATUS.ABSENT).FirstOrDefault(),
                    IsLate = assists.Where(y => y.UserId == x.TeacherId).Select(y => y.Status == ConstantHelpers.WORKING_DAY.STATUS.LATE).FirstOrDefault(),
                    Time = DateTime.UtcNow
                })
                .ToList();

            var filterRecord = result.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = result,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = count,
                RecordsTotal = filterRecord
            };
        }
        public async Task<IEnumerable<TeacherPlusAssitance>> GetTeachersDailyAssitance(string teacherId, DateTime startDate, string search, ClaimsPrincipal user)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            var assists = _context.WorkingDays.Where(x => x.TermId == term.Id).AsEnumerable().Where(y => y.RegisterDate.ToShortDateString() == startDate.ToShortDateString()
              && y.TermId == term.Id).ToList();

            var query = _context.Teachers.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.AcademicDepartment.Career.Faculty.DeanId == userId || x.AcademicDepartment.Career.Faculty.SecretaryId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    var academicDeparments = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId).Select(x => x.Id).ToListAsync();
                    query = query.Where(x => x.AcademicDepartmentId.HasValue && academicDeparments.Contains(x.AcademicDepartmentId.Value));
                }
            }


            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.UserId == teacherId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.User.FullName.Trim().ToLower().Contains(search.Trim().ToLower()));

            var resultDB = await query
                .Select(
                x => new TeacherPlusAssitance
                {
                    TeacherId = x.UserId,
                    Name = x.User.Name,
                    MaternalSurname = x.User.MaternalSurname,
                    PaternalSurname = x.User.PaternalSurname,
                    UserName = x.User.UserName,
                })
                .ToListAsync();

            var result = resultDB.Select(
                    x => new TeacherPlusAssitance
                    {
                        TeacherId = x.TeacherId,
                        FullName = $"{x.Name} {x.PaternalSurname} {x.MaternalSurname}",
                        UserName = x.UserName,
                        Status = 0,
                        StartDate = assists.Where(y => y.UserId == x.TeacherId && y.StartTime.HasValue).Select(y => y.StartTime.Value.ToLocalDateTimeFormatUtc()).FirstOrDefault(),
                        EndDate = assists.Where(y => y.UserId == x.TeacherId && y.Endtime.HasValue).Select(y => y.Endtime.Value.ToLocalDateTimeFormatUtc()).FirstOrDefault(),
                        IsAbsent = assists.Where(y => y.UserId == x.TeacherId).Select(y => y.Status == ConstantHelpers.WORKING_DAY.STATUS.ABSENT).FirstOrDefault(),
                        IsLate = assists.Where(y => y.UserId == x.TeacherId).Select(y => y.Status == ConstantHelpers.WORKING_DAY.STATUS.LATE).FirstOrDefault(),
                        Time = DateTime.UtcNow
                    }
                )
                .ToList();
            return result;
        }

        public async Task<TeacherPlusMonthlyAssitance> GetMonthlyAssistance(string teacherId, string startDate, string search, ClaimsPrincipal user)
        {
            var month = string.IsNullOrEmpty(startDate) ? DateTime.UtcNow.Month : Convert.ToInt32(startDate.Split("-")[0]);
            var year = string.IsNullOrEmpty(startDate) ? DateTime.UtcNow.Year : Convert.ToInt32(startDate.Split("-")[1]);
            DateTime dateee = new DateTime(year, month, 01);


            var assists = await _context.WorkingDays.Where(y => y.RegisterDate.Month == dateee.Month && y.RegisterDate.Year == dateee.Year).ToListAsync();
            var query = _context.Teachers.AsQueryable();
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(x => x.AcademicDepartment.Career.Faculty.DeanId == userId || x.AcademicDepartment.Career.Faculty.SecretaryId == userId);
                }
            }

            if (!string.IsNullOrEmpty(teacherId))
                query = query.Where(x => x.UserId == teacherId);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.User.FullName.Trim().ToLower().Contains(search.Trim().ToLower()));
            }

            var count = await query.CountAsync();
            var result = await query.Select(
                    x => new TeacherPlusMonthlyAssitanceItemList
                    {
                        TeacherId = x.UserId,
                        FullName = $"{x.User.Name} {x.User.PaternalSurname} {x.User.MaternalSurname}",
                        UserName = x.User.UserName,
                        Career = x.Career.Name,
                        Months = new List<int>()
                    }
                )
                .ToListAsync();
            result.ForEach(item =>
            {
                var newList = new List<int>();
                for (int i = 1; i <= DateTime.DaysInMonth(dateee.Year, dateee.Month); i++)
                {
                    var status = assists.FirstOrDefault(y => y.UserId == item.TeacherId && y.RegisterDate.Day == i && y.RegisterDate.Month == dateee.Month && y.RegisterDate.Year == dateee.Year);
                    if (status != null)
                        newList.Add(status.Status);
                    else
                        newList.Add(2);

                    item.Months = newList;
                }
            });
            var model = new TeacherPlusMonthlyAssitance()
            {
                List = result,
                Assist = assists
            };

            return model;
        }












        public async Task<IEnumerable<WorkingDay>> GetWorkingDayByDateAndUser(DateTime registerDate, string userId)
        {
            var workingDays = await _context.WorkingDays.Where(x => x.UserId == userId).ToArrayAsync();
            var result = workingDays.Where(x => x.RegisterDate.ToDefaultTimeZone().Date == registerDate.ToDefaultTimeZone().Date).ToArray();
            return result;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersToTakeAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string searchValue = null)
        {
            Expression<Func<Teacher, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
            }

            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();

            if (term == null)
                term = new Term();
            var assists = _context.WorkingDays
                .Where(x => x.TermId == term.Id)
                .AsEnumerable()
                .Where(x => x.RegisterDate.ToDefaultTimeZone().Date == DateTime.UtcNow.ToDefaultTimeZone().Date)
                .OrderByDescending(y => y.RegisterDate)
                .ToArray();

            var query = _context.Teachers.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    query = query.Where(x => x.AcademicDepartment.Career.AcademicSecretaryId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    query = query.Where(x => x.AcademicDepartment.AcademicDepartmentDirectorId == userId || x.AcademicDepartment.AcademicDepartmentSecretaryId == userId);
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.User.FullName.ToLower().Trim().Contains(searchValue.Trim().ToLower()) || x.User.UserName.ToLower().Contains(searchValue.Trim().ToLower()));

            int recordsFiltered = await query.CountAsync();
            var dataDB = await query
                 .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                 .Skip(sentParameters.PagingFirstRecord)
                 .Take(sentParameters.RecordsPerDraw)
                 .Select(x => new
                 {
                     id = x.UserId,
                     fullName = x.User.FullName,
                     userName = x.User.UserName
                 })
                 .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.id,
                    x.fullName,
                    x.userName,
                    startDate = assists.Where(y => y.UserId == x.id && y.StartTime.HasValue).Select(y => y.StartTime.Value.ToLocalDateTimeFormatUtc()).FirstOrDefault(),
                    endDate = assists.Where(y => y.UserId == x.id && y.Endtime.HasValue).Select(y => y.Endtime.Value.ToLocalDateTimeFormatUtc()).FirstOrDefault(),
                    isAbsent = assists.Where(y => y.UserId == x.id).Select(y => y.Status == ConstantHelpers.WORKING_DAY.STATUS.ABSENT).FirstOrDefault(),
                    isLate = assists.Where(y => y.UserId == x.id).Select(y => y.Status == ConstantHelpers.WORKING_DAY.STATUS.LATE).FirstOrDefault(),
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

        public async Task<IEnumerable<WorkingDayConsolited>> GetConsolidatedWorkingDayByMonth(DateTime startDate, DateTime endDate, List<string> usersId = null)
        {
            var startDateFk = startDate.AddDays(-1);
            var endDateFk = endDate.AddDays(1);

            var query = _context.WorkingDays
                .Where(x => x.RegisterDate.Date >= startDateFk.Date
                && x.RegisterDate.Date <= endDateFk.Date)
                .Include(x => x.User)
                .AsQueryable();

            if (usersId != null)
                query = query.Where(x => usersId.Contains(x.UserId));

            var dataDB = await query.ToListAsync();

            dataDB = dataDB.Where(x => x.RegisterDate.ToDefaultTimeZone().Date >= startDate.Date
                && x.RegisterDate.ToDefaultTimeZone().Date <= endDateFk.Date)
                .ToList();

            var teachers = dataDB
                .GroupBy(x => x.User)
                .Select(x => new WorkingDayConsolited
                {
                    User = x.Key.FullName,
                    UserId = x.Key.Id,
                    Details = x.GroupBy(y => y.RegisterDate.ToDefaultTimeZone().Date).Select(y => new WorkingDayConsolitedDetail
                    {
                        Date = $"{y.Key.Date:dd/MM/yyyy}",
                        DateTime = y.Key,
                        Day = y.Key.Day,
                        DayOfWeek = ConstantHelpers.WEEKDAY.ENUM_TO_STRING(y.Key.DayOfWeek),
                        FirstEntryStr = y.OrderBy(h => h.StartTime).Where(h => h.StartTime.HasValue).Select(z => z.StartTime.Value.ToLocalDateTimeFormatUtc()).FirstOrDefault(),
                        FirstEntry = y.OrderBy(h => h.StartTime).Where(h => h.StartTime.HasValue).Select(z => z.StartTime.Value.ToLocalTimeSpanUtc()).FirstOrDefault(),
                        LastExitStr = y.OrderByDescending(h => h.Endtime).Where(h => h.Endtime.HasValue).Select(z => z.Endtime.Value.ToLocalDateTimeFormatUtc()).FirstOrDefault()
                    })
                    .ToList()
                })
                .ToList();

            //var teachers = await query
            //    .GroupBy(x => x.User)
            //    .Select(x => new WorkingDayConsolited
            //    {
            //        User = x.Key.FullName,
            //        UserId = x.Key.Id,
            //        Details = x.GroupBy(y => y.RegisterDate.ToDefaultTimeZone().Date).Select(y => new WorkingDayConsolitedDetail
            //        {
            //            Date = $"{y.Key.Date:dd/MM/yyyy}",
            //            DateTime = y.Key,
            //            Day = y.Key.Day,
            //            DayOfWeek = ConstantHelpers.WEEKDAY.ENUM_TO_STRING(y.Key.DayOfWeek),
            //            FirstEntryStr = y.OrderBy(h => h.StartTime).Where(h => h.StartTime.HasValue).Select(z => z.StartTime.Value.ToLocalDateTimeFormatUtc()).FirstOrDefault(),
            //            FirstEntry = y.OrderBy(h => h.StartTime).Where(h => h.StartTime.HasValue).Select(z => z.StartTime.Value.ToLocalTimeSpanUtc()).FirstOrDefault(),
            //            LastExitStr = y.OrderByDescending(h => h.Endtime).Where(h => h.Endtime.HasValue).Select(z => z.Endtime.Value.ToLocalDateTimeFormatUtc()).FirstOrDefault()
            //        })
            //        .ToList()
            //    })
            //    .ToListAsync();

            if (usersId != null)
            {
                var listToAdd = usersId.Where(x => !teachers.Any(y => y.UserId == x)).ToArray();

                foreach (var userToAdd in listToAdd)
                {
                    var user = await _context.Users.Where(x => x.Id == userToAdd).FirstOrDefaultAsync();

                    if (user != null)
                    {
                        teachers.Add(new WorkingDayConsolited
                        {
                            User = user.FullName,
                            UserId = user.Id
                        });
                    }
                }
            }

            foreach (var teacher in teachers)
            {
                for (DateTime init = startDate; init.Date <= endDate.Date; init = init.AddDays(1))
                {
                    if (!teacher.Details.Any(y => y.DateTime.Date == init.Date))
                        teacher.Details.Add(new WorkingDayConsolitedDetail
                        {
                            Date = $"{init.Date:dd/MM/yyyy}",
                            DateTime = init.Date,
                            Day = init.Day,
                            DayOfWeek = ConstantHelpers.WEEKDAY.ENUM_TO_STRING(init.DayOfWeek)
                        });
                }

                teacher.Details = teacher.Details.OrderBy(y => y.DateTime).ToList();
                var classes = await _context.TeacherSchedules.Where(x => x.TeacherId == teacher.UserId).Select(x => x.ClassSchedule).ToArrayAsync();

                foreach (var detail in teacher.Details)
                {
                    var byDay = classes.Where(x => ConstantHelpers.WEEKDAY.VALUES[x.WeekDay] == detail.DayOfWeek).FirstOrDefault();
                    if (byDay != null)
                    {
                        var startTime = byDay.StartTime.ToLocalTimeSpanUtc();
                        detail.ScheduleRange = $"{byDay.StartTime.ToLocalDateTimeFormatUtc()} - {byDay.EndTime.ToLocalDateTimeFormatUtc()}";
                        var delayed = detail.FirstEntry.Subtract(startTime);
                        detail.Delay = delayed < TimeSpan.Zero ? "0:00" : $"{delayed}";
                    }
                }

            }


            return teachers;

        }
    }
}