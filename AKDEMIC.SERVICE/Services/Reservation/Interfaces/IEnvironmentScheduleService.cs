using AKDEMIC.ENTITIES.Models.Reservations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Reservation.Interfaces
{
    public interface IEnvironmentScheduleService
    {
        Task Insert(EnvironmentSchedule environmentSchedule);
        Task Update(EnvironmentSchedule environmentSchedule);
        void RemoveRange(IEnumerable<EnvironmentSchedule> environmentSchedules);
        Task<IEnumerable<EnvironmentSchedule>> GetByEnvironmentId(Guid environmentId);
        Task<object> GetEventList(Guid id, DateTime start, DateTime end);
        Task<int> CountByWeekDayAndHour(Guid environmentId, int weekDay, int hour);
        Task<EnvironmentSchedule> Get(Guid id);
        Task Delete(EnvironmentSchedule entity);
        Task<object> GetEventListByUser(Guid id, DateTime start, DateTime end);
    }
}
