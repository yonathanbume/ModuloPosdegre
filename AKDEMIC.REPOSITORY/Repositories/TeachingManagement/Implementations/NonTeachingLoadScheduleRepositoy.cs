using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class NonTeachingLoadScheduleRepositoy : Repository<NonTeachingLoadSchedule> , INonTeachingLoadScheduleRepositoy
    {
        public NonTeachingLoadScheduleRepositoy(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetNonTeachingLoadScheduleDatatable(DataTablesStructs.SentParameters sentParameters, Guid nonTeachingLoadId)
        {
            var query = _context.NonTeachingLoadSchedules.Where(x => x.NonTeachingLoadId == nonTeachingLoadId)
                .AsQueryable();

            int recordsFiltered = await query.CountAsync();

            var dataDB = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    startTime = x.StartTime,
                    endTime = x.EndTime,
                    weekday = x.WeekDay,
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.Id,
                    startTime = x.startTime.ToLocalDateTimeFormatUtc(),
                    endTime = x.endTime.ToLocalDateTimeFormatUtc(),
                    weekDayNumber = x.weekday,
                    weekday = ConstantHelpers.WEEKDAY.VALUES[x.weekday],
                    duration = x.endTime.ToLocalTimeSpanUtc().Subtract(x.startTime.ToLocalTimeSpanUtc()).TotalHours
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

        public async Task<double> GetRemainingMinutesByNonteachingLoadId(Guid nonTeachingLoadId)
        {
            var currentMinutes = await _context.NonTeachingLoadSchedules.Where(x => x.NonTeachingLoadId == nonTeachingLoadId).Select(x => new { x.EndTime, x.StartTime }).ToArrayAsync();
            var sumCurrentMinutes = currentMinutes.Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalMinutes));

            var totalMinutes = await _context.NonTeachingLoads.Where(x => x.Id == nonTeachingLoadId).Select(x => x.Minutes).FirstOrDefaultAsync();

            return totalMinutes - sumCurrentMinutes;
        }

        public async Task<double> GetCurrentMinutesByNonteachingLoadId(Guid nonTeachingLoadId)
        {
            var nonTeachingLoadSchedules = await _context.NonTeachingLoadSchedules.Where(x => x.NonTeachingLoadId == nonTeachingLoadId).ToListAsync();
            var result = nonTeachingLoadSchedules.Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalMinutes));
            return result;
        }

        public async Task<Tuple<string,bool>> HasConflict(string teacherId, int weekDay, TimeSpan startTime, TimeSpan endTime, Guid? ignoredId = null)
        {
            var activeTerm = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
            var result = await _context.NonTeachingLoadSchedules.Where(x => x.NonTeachingLoad.TermId == activeTerm.Id && x.NonTeachingLoad.TeacherId == teacherId && x.WeekDay == weekDay)
                .Include(x=>x.NonTeachingLoad)
                .ToListAsync();

            var conflictedClass = result.Where(x => x.Id != ignoredId && IsInConflict(x.StartTime.ToLocalTimeSpanUtc(), x.EndTime.ToLocalTimeSpanUtc(), startTime.ToLocalTimeSpanUtc(), endTime.ToLocalTimeSpanUtc())).FirstOrDefault();

            if (conflictedClass != null)
                return new Tuple<string, bool>(conflictedClass.NonTeachingLoad.Name, true);

            return new Tuple<string, bool>(string.Empty, false);

        }

        private bool IsInConflict(TimeSpan st1, TimeSpan et1, TimeSpan st2, TimeSpan et2)
        {
            return (st1 <= st2 && et1 > st2) || (st1 < et2 && et1 >= et2) || (st2 <= st1 && et2 > st1) || (st2 < et1 && et2 >= et1);
        }

        public async Task<object> GetScheduleObject(string teacherId, DateTime start, DateTime end)
        {

            //var nonTeachingSchedules = await _context.NonTeachingLoads.Where(x => 
            //    x.TeacherId == teacherId && 
            //    x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE &&
            //    (
            //    (x.StartDate.Value.Date <= start.Date && x.EndDate.Value.Date > end.Date) || 
            //    (x.StartDate.Value.Date < end.Date && x.EndDate.Value.Date >= end.Date)  ||
            //    (start.Date <= x.StartDate.Value.Date && end.Date > x.StartDate.Value.Date) ||
            //    (start.Date < x.EndDate.Value.Date && end.Date >= x.EndDate.Value.Date)
            //    ))
            //    .Select(x => new
            //    {
            //        x.StartDate,
            //        x.EndDate,
            //        x.Location,
            //        x.Name,
            //        schedules = x.NonTeachingLoadSchedules.Select(y=> new
            //        {
            //            y.StartTime,
            //            y.EndTime,
            //            y.WeekDay
            //        }).ToList()
            //    })
            //    .ToListAsync();



            var dates = new List<DateTime>();

            for (DateTime date = start; date <= end; date = date.AddDays(1))
                dates.Add(date);

            var query = _context.NonTeachingLoadSchedules
                .Where(x => x.NonTeachingLoad.TeacherId == teacherId && x.NonTeachingLoad.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            query = query.Where(x => 
                (
                (x.NonTeachingLoad.StartDate.Value.Date <= start.Date && x.NonTeachingLoad.EndDate.Value.Date > end.Date) ||
                (x.NonTeachingLoad.StartDate.Value.Date < end.Date && x.NonTeachingLoad.EndDate.Value.Date >= end.Date) ||
                (start.Date <= x.NonTeachingLoad.StartDate.Value.Date && end.Date > x.NonTeachingLoad.StartDate.Value.Date) ||
                (start.Date < x.NonTeachingLoad.EndDate.Value.Date && end.Date >= x.NonTeachingLoad.EndDate.Value.Date)
                )
                );

            var resultDB = await query
                .Select(x => new
                {
                    id = x.Id,
                    title = x.NonTeachingLoad.Name,
                    description = x.NonTeachingLoad.Location,
                    weekday = x.WeekDay,
                    startTime = x.StartTime,
                    endTime = x.EndTime,
                    x.NonTeachingLoad.StartDate,
                    x.NonTeachingLoad.EndDate
                })
                .ToListAsync();

            var result = resultDB
               .Select(x => new
               {
                   x.id,
                   x.title,
                   x.description,
                   allDay = false,
                   nonTeachingStart = x.StartDate.Value.Date,
                   nonTeachingEnd = x.EndDate.Value.Date,
                   startTpm = (dates.Where(y => y.DayOfWeek == ConstantHelpers.WEEKDAY.TO_ENUM(x.weekday)).FirstOrDefault().Date.Add(x.startTime.ToLocalTimeSpanUtc())),
                   endTpm = (dates.Where(y => y.DayOfWeek == ConstantHelpers.WEEKDAY.TO_ENUM(x.weekday)).FirstOrDefault().Date.Add(x.endTime.ToLocalTimeSpanUtc())),
                   start = (dates.Where(y => y.DayOfWeek == ConstantHelpers.WEEKDAY.TO_ENUM(x.weekday)).FirstOrDefault().Date.Add(x.startTime.ToLocalTimeSpanUtc()).ToString("yyyy-MM-dd HH:mm:ss")),
                   end = (dates.Where(y => y.DayOfWeek == ConstantHelpers.WEEKDAY.TO_ENUM(x.weekday)).FirstOrDefault().Date.Add(x.endTime.ToLocalTimeSpanUtc()).ToString("yyyy-MM-dd HH:mm:ss")),
               })
               .ToList();

            result = result.Where(x => x.startTpm >= x.nonTeachingStart && x.endTpm <= x.nonTeachingEnd).ToList();

            return result;
        }

        public async Task<object> GetScheduleObjectDetail(Guid id)
        {
            var nonTeachingLoadSchedule = await _context.NonTeachingLoadSchedules.Where(x => x.Id == id)
                .Select(x => new
                {
                    classroom = "-",
                    date = "-",
                    start = x.StartTime.ToLocalDateTimeFormat(),
                    end = x.EndTime.ToLocalDateTimeFormat(),
                    course = x.NonTeachingLoad.RelatedCourse.Name??"Sin asignar",
                    section = "-",
                    type = "C.N.L."
                })
                .FirstOrDefaultAsync();

            return nonTeachingLoadSchedule;
        }

        public async Task<List<NonTeachingLoadSchedule>> GetByNonTeachingLoad(Guid id)
            => await _context.NonTeachingLoadSchedules.Where(x => x.NonTeachingLoadId == id).ToListAsync();
    }
}
