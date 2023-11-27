using AKDEMIC.ENTITIES.Models.Reservations;
using AKDEMIC.REPOSITORY.Repositories.Reservation.Interfaces;
using AKDEMIC.SERVICE.Services.Reservation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Reservation.Implementations
{
    public class EnvironmentScheduleService : IEnvironmentScheduleService
    {
        private readonly IEnvironmentScheduleRepository _environmentScheduleRepository;

        public EnvironmentScheduleService(IEnvironmentScheduleRepository environmentScheduleRepository)
        {
            _environmentScheduleRepository = environmentScheduleRepository;
        }

        public async Task<int> CountByWeekDayAndHour(Guid environmentId, int weekDay, int hour)
            => await _environmentScheduleRepository.CountByWeekDayAndHour(environmentId, weekDay, hour);

        public async Task<EnvironmentSchedule> Get(Guid id)
            => await _environmentScheduleRepository.Get(id);

        public async Task<IEnumerable<EnvironmentSchedule>> GetByEnvironmentId(Guid environmentId)
            => await _environmentScheduleRepository.GetByEnvironmentId(environmentId);

        public async Task<object> GetEventList(Guid id, DateTime start, DateTime end)
            => await _environmentScheduleRepository.GetEventList(id, start, end);

        public async Task Insert(EnvironmentSchedule environmentSchedule)
            => await _environmentScheduleRepository.Insert(environmentSchedule);

        public async Task Delete(EnvironmentSchedule entity)
            => await _environmentScheduleRepository.Delete(entity);

        public void RemoveRange(IEnumerable<EnvironmentSchedule> environmentSchedules)
            => _environmentScheduleRepository.RemoveRange(environmentSchedules);

        public async Task Update(EnvironmentSchedule environmentSchedule)
            => await _environmentScheduleRepository.Update(environmentSchedule);

        public async Task<object> GetEventListByUser(Guid id, DateTime start, DateTime end)
            => await _environmentScheduleRepository.GetEventListByUser(id, start, end);
    }
}
