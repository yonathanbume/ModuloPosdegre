using AKDEMIC.ENTITIES.Models.Reservations;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Reservation.Interfaces
{
    public interface IEnvironmentScheduleRepository : IRepository<EnvironmentSchedule>
    {
        Task<IEnumerable<EnvironmentSchedule>> GetByEnvironmentId(Guid environmentId);
        Task<object> GetEventList(Guid id, DateTime start, DateTime end);
        Task<int> CountByWeekDayAndHour(Guid environmentId, int weekDay, int hour);
        Task<object> GetEventListByUser(Guid id, DateTime start, DateTime end);
    }
}
