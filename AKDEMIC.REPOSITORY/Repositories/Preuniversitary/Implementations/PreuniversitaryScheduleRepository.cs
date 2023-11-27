using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Implementations
{
    public class PreuniversitaryScheduleRepository : Repository<PreuniversitarySchedule> ,IPreuniversitaryScheduleRepository
    {
        public PreuniversitaryScheduleRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetWeekDetails(Guid termId, Guid userGroupId, bool? absent = null, int? week = null)
        {
            var term = await _context.PreuniversitaryTerms.FindAsync(termId);
            var weeks = 0;
            if (DateTime.UtcNow >= term.ClassStartDate && DateTime.UtcNow <= term.ClassEndDate)
                weeks = (int)Math.Ceiling((DateTime.UtcNow - term.ClassStartDate).TotalDays / 7);
            else if (DateTime.UtcNow > term.ClassEndDate)
                weeks = (int)Math.Ceiling((term.ClassEndDate - term.ClassStartDate).TotalDays / 7);
            var schedules = await _context.PreuniversitarySchedules
                .Where(x => x.PreuniversitaryGroup.PreuniversitaryUserGroups.Any(pug => pug.Id == userGroupId))
                .ToListAsync();
            var data = await _context.PreuniversitaryAssistanceStudents
                .Include(x => x.PreuniversitaryAssistance)
                .Where(x => x.PreuniversitaryUserGroupId == userGroupId).ToListAsync();
            var result = Enumerable.Range(1, weeks)
                .SelectMany(x => schedules.Select((s, i) => new
                {
                    week = x,
                    session = i + 1,
                    date = term.ClassStartDate.AddDays(7 * x).AddDays(s.DayOfWeek).ToLocalDateFormat(),
                    day = ConstantHelpers.WEEKDAY.VALUES[s.DayOfWeek],
                    start = s.StartTime.ToLocalDateTimeFormatUtc(),
                    end = s.EndTime.ToLocalDateTimeFormatUtc(),
                    status = data.Where(d => d.PreuniversitaryAssistance.PreuniversitaryScheduleId == s.Id && d.PreuniversitaryAssistance.DateTime.ToDefaultTimeZone().Date == term.ClassStartDate.AddDays(7 * x).AddDays(s.DayOfWeek).ToDefaultTimeZone().Date).Select(d => (bool?)d.IsAbsent).DefaultIfEmpty(null).FirstOrDefault()
                })).Reverse().ToList();
            if (absent.HasValue)
                result = result.Where(x => x.status.HasValue && x.status.Value == absent.Value).ToList();
            if (week.HasValue)
                result = result.Where(x => x.week == week.Value).ToList();
            return result;
        }

        public async Task<object> GetWeeks(Guid termId)
        {
            var term = await _context.PreuniversitaryTerms.FindAsync(termId);
            var weeks = 0;
            if (DateTime.UtcNow >= term.ClassStartDate && DateTime.UtcNow <= term.ClassEndDate)
                weeks = (int)Math.Ceiling((DateTime.UtcNow - term.ClassStartDate).TotalDays / 7);
            else if (DateTime.UtcNow > term.ClassEndDate)
                weeks = (int)Math.Ceiling((term.ClassEndDate - term.ClassStartDate).TotalDays / 7);
            var result = Enumerable.Range(1, weeks)
                .Select(x => new
                {
                    id = x,
                    text = $"Semana {x}"
                }).Reverse().ToList();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSchedulesDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId, Guid groupId, string searchValue = null)
        {
            Expression<Func<PreuniversitarySchedule, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.DayOfWeek); break;
                case "2":
                    orderByPredicate = ((x) => x.StartTime); break;
                case "3":
                    orderByPredicate = ((x) => x.Classroom.Description); break;
                default:
                    orderByPredicate = ((x) => x.DayOfWeek); break;
            }

            var preuniversitaryTermAsync = await _context.PreuniversitaryTerms.FindAsync(termId);

            var query = _context.PreuniversitarySchedules
                .Where(x => x.PreuniversitaryGroupId == groupId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => ConstantHelpers.WEEKDAY.VALUES[x.DayOfWeek].Contains(searchValue) ||
                                            x.StartTime.Hours.ToString().Contains(searchValue) ||
                                            x.StartTime.Minutes.ToString().Contains(searchValue) ||
                                            x.EndTime.Hours.ToString().Contains(searchValue) ||
                                            x.EndTime.Minutes.ToString().Contains(searchValue) ||
                                            x.Classroom.Description.Contains(searchValue) ||
                                            x.Classroom.Building.Name.Contains(searchValue) ||
                                            x.Classroom.Building.Campus.Name.Contains(searchValue));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      dayOfWeek = ConstantHelpers.WEEKDAY.VALUES[x.DayOfWeek],
                      startTime = x.StartTime.ToLocalDateTimeFormatUtc(),
                      endTime = x.EndTime.ToLocalDateTimeFormatUtc(),
                      classroom = x.Classroom.Description,
                      building = x.Classroom.Building.Name,
                      campus = x.Classroom.Building.Campus.Name,
                      duration = x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalHours
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

        public async override Task<PreuniversitarySchedule> Get(Guid id)
            => await _context.PreuniversitarySchedules.Include(x => x.Classroom).ThenInclude(x => x.Building).Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<bool> AnyGroupIdByDayOfWeekAndTimeConflict(Guid groupId,byte dayOfWeek,TimeSpan st, TimeSpan et, Guid? ignoredId = null)
        {
            var preuniversitarySchedules = await _context.PreuniversitarySchedules.Where(x => x.PreuniversitaryGroupId == groupId && x.Id != ignoredId)
                .ToListAsync();
            return preuniversitarySchedules.Any(x => x.DayOfWeek == dayOfWeek && ConvertHelpers.TimeSpanConflict(x.StartTime, x.EndTime, st, et));
        }
            

        public async Task<PreuniversitarySchedule> GetClassroomConflicted(Guid classroomId, byte dayOfWeek, TimeSpan startTime, TimeSpan endTime, Guid? ignoredId = null)
        {
            var preuniversitarySchedules = await _context.PreuniversitarySchedules
                .Include(x => x.PreuniversitaryGroup.PreuniversitaryCourse)
                .Where(x => x.Id == classroomId && x.Id != ignoredId && x.DayOfWeek == dayOfWeek)
                .ToListAsync();

            return preuniversitarySchedules.Where(x => ConvertHelpers.TimeSpanConflict(x.StartTime, x.EndTime, startTime, endTime)).FirstOrDefault();
        }

        public async Task<PreuniversitarySchedule> GetTeacherConflicted(string teacherId, byte dayOfWeek, TimeSpan startTime, TimeSpan endTime, Guid? ignoredId = null)
        {
            var preuniversitarySchedules = await _context.PreuniversitarySchedules
                .Include(x => x.PreuniversitaryGroup.PreuniversitaryCourse)
                .Where(x => x.PreuniversitaryGroup.TeacherId == teacherId && x.Id != ignoredId && x.DayOfWeek == dayOfWeek)
                .ToListAsync();

            return preuniversitarySchedules.Where(x => ConvertHelpers.TimeSpanConflict(x.StartTime, x.EndTime, startTime, endTime)).FirstOrDefault();
        }

        public async Task<PreuniversitarySchedule> GetScheduleToTeacher(string userId, int currentDayOfWeek)
            => await _context.PreuniversitarySchedules.Where(x => x.PreuniversitaryGroup.TeacherId == userId && x.DayOfWeek == currentDayOfWeek && x.StartTime <= DateTime.UtcNow.TimeOfDay && x.EndTime > DateTime.UtcNow.TimeOfDay).Include(x=>x.PreuniversitaryGroup).ThenInclude(x=>x.PreuniversitaryCourse).FirstOrDefaultAsync();

        public async Task<PreuniversitarySchedule> GetNextCurrentOfWeek(string userId, int currentDayOfWeek)
        {
            var preuniversitarySchedules = await _context.PreuniversitarySchedules
                .Where(x => x.PreuniversitaryGroup.TeacherId == userId && x.DayOfWeek >= currentDayOfWeek && x.StartTime > DateTime.UtcNow.TimeOfDay)
                .ToListAsync();

            return preuniversitarySchedules.OrderByDescending(x => x.StartTime + new TimeSpan(x.DayOfWeek - currentDayOfWeek, 0, 0, 0) - DateTime.UtcNow.TimeOfDay).FirstOrDefault();
        }

        public async Task<PreuniversitarySchedule> GetScheduleToTemaries(string userId , int currentDayOfWeek)
        {
            var fecha = DateTime.UtcNow.TimeOfDay;

            var preuniversitarySchedules = await _context.PreuniversitarySchedules
                .Include(x => x.PreuniversitaryGroup)
                .Where(x => x.PreuniversitaryGroup.TeacherId == userId && x.DayOfWeek == currentDayOfWeek)
                .ToListAsync();
                
            var quack = preuniversitarySchedules
                .Where(x => x.StartTime < DateTime.UtcNow.TimeOfDay && x.EndTime > DateTime.UtcNow.TimeOfDay)
                .FirstOrDefault();

            return quack;

        }

    }
}
