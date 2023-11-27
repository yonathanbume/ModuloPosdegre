using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Reservations;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Reservation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Reservation.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Reservation.Implementations
{
    public class EnvironmentScheduleRepository : Repository<EnvironmentSchedule>, IEnvironmentScheduleRepository
    {
        public EnvironmentScheduleRepository(AkdemicContext context) : base(context) { }

        public async Task<int> CountByWeekDayAndHour(Guid environmentId, int weekDay, int hour)
        {
            var data = await _context.EnvironmentSchedules.Where(x => x.EnvironmentId == environmentId && x.WeekDay == weekDay).ToArrayAsync();
            return data.Where(x => x.StartTime.Hours == hour).Count();
        }

        public async Task<IEnumerable<EnvironmentSchedule>> GetByEnvironmentId(Guid environmentId)
            => await _context.EnvironmentSchedules.Where(X => X.EnvironmentId == environmentId).ToArrayAsync();

        public async Task<object> GetEventList(Guid id, DateTime start, DateTime end)
        {
            List<object> listEvents = new List<object>();

            var selectedDates = Enumerable
               .Range(0, int.MaxValue)
               .Select(
                   index => new DateTime?(start.AddDays(index))
                   )
               .TakeWhile(date => date <= end)
               .ToList();

            var result = await _context.EnvironmentSchedules
                .Where(c => c.EnvironmentId == id)
                .Select(c => new
                {
                    id = c.Id.ToString(),
                    title = string.Format("Aforo ({0})", c.Environment.Capacity.ToString()),
                    description = c.Environment.Name,
                    allDay = false,
                    weekday = c.WeekDay,
                    start = c.StartTime,
                    end = c.EndTime
                }).ToListAsync();
            for (int i = 0; i <= 5; i++)
            {
                var Events = result
                    .Where(a => a.weekday.Equals(i))
                    .Select(a => new
                    {
                        a.id,
                        a.title,
                        a.description,
                        a.allDay,
                        start = (selectedDates[i].Value.Date + a.start).ToString("yyyy-MM-dd HH:mm:ss"),
                        end = (selectedDates[i].Value.Date + a.end).ToString("yyyy-MM-dd HH:mm:ss")
                    }).ToList<object>();
                listEvents.AddRange(Events);
            }

            return listEvents;
        }

        public async Task<object> GetEventListByUser(Guid id, DateTime start, DateTime end)
        {
            List<object> listEvents = new List<object>();
            try
            {
                if (DateTime.Now.DayOfYear >= end.DayOfYear)
                    return listEvents;

                List<DateTime?> selectedDates = Enumerable
                .Range(0, int.MaxValue)
                .Select(index => new DateTime?(start.AddDays(index)))
                .TakeWhile(date => date <= end.AddDays(7 - (int)end.DayOfWeek))
                .ToList();

                var result = await _context.EnvironmentSchedules.Where(c => c.EnvironmentId == id).Select(c => new ScheduleTemplate
                {
                    id = c.Id.ToString(),
                    title = string.Format("{0}-{1} ({2})", c.Environment.Code, c.Environment.Name, c.Environment.Capacity.ToString()),
                    description = "Disponible",
                    allDay = false,
                    weekday = c.WeekDay,
                    start = c.StartTime,
                    end = c.EndTime
                }).ToListAsync();

                var reservations = await _context.EnvironmentReservations.Where(x => x.EnvironmentId == id && (x.ReservationStart > start && x.ReservationEnd < end
                && (x.State == ConstantHelpers.RESERVATION.STATUS.PENDING || x.State == ConstantHelpers.RESERVATION.STATUS.APPROVED))).Select(x => new ScheduleTemplate
                {
                    weekday = (int)x.ReservationStart.DayOfWeek - 1,
                    start = x.ReservationStart.TimeOfDay,
                    end = x.ReservationEnd.TimeOfDay
                }).ToListAsync();

                //reservas
                var _exclude = result.Where(x => reservations.Any(y => y.weekday == x.weekday && y.start <= x.start && y.end >= x.end));

                var _result = result.Except(_exclude);

                foreach (var item in _exclude)
                {
                    item.id = "";
                    item.description = "Reservado";
                }

                var _result2 = result.Union(_exclude);

                var now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));

                //pasadas
                var _include = result.Where(c => (c.weekday + 1 < (int)now.DayOfWeek || (c.end < now.AddMinutes(30).TimeOfDay && c.weekday < (int)now.DayOfWeek)) && start < now);

                _result2 = result.Except(_include);

                foreach (var item in _include)
                {
                    item.id = "";
                    item.description = "No Disponible";
                }

                var _result3 = result.Union(_include);

                for (int i = 0; i <= 5; i++)
                {
                    listEvents.AddRange(_result3.Where(a => a.weekday.Equals(i)).Select(a => new
                    {
                        a.id,
                        a.title,
                        a.description,
                        a.allDay,
                        start = (selectedDates[i].Value.Date + a.start).ToString("yyyy-MM-dd HH:mm:ss"),
                        end = (selectedDates[i].Value.Date + a.end).ToString("yyyy-MM-dd HH:mm:ss")
                    }).ToList());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listEvents;
        }

    }
}
