using AKDEMIC.ENTITIES.Models.Reservations;
using AKDEMIC.REPOSITORY.Repositories.Reservation.Interfaces;
using AKDEMIC.SERVICE.Services.Reservation.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Reservation.Implementations
{
    public class EnvironmentReservationService : IEnvironmentReservationService
    {
        private readonly IEnvironmentReservationRepository _environmentReservationRepository;

        public EnvironmentReservationService(IEnvironmentReservationRepository environmentReservationRepository)
        {
            _environmentReservationRepository = environmentReservationRepository;
        }

        public async Task<bool> AnyByEnvironmentId(Guid environmentId)
            => await _environmentReservationRepository.AnyByEnvironmentId(environmentId);

        public async Task<EnvironmentReservation> Get(Guid id)
            => await _environmentReservationRepository.Get(id);

        public async Task<object> GetAllByFilters(Guid rid, int reservationstate)
            => await _environmentReservationRepository.GetAllByFilters(rid, reservationstate);

        public async Task<object> GetAllEnvironmentReservationsByExternalUser(string userId)
            => await _environmentReservationRepository.GetAllEnvironmentReservationsByExternalUser(userId);

        public async Task<object> GetEnvironmentReservationsByUser(string userId)
            => await _environmentReservationRepository.GetEnvironmentReservationsByUser(userId);

        public async Task<object> GetNextEnvironmetnsByUserId(string userId)
            => await _environmentReservationRepository.GetNextEnvironmetnsByUserId(userId);

        public async Task<object> GetReservationStatesByUserId(string userId)
            => await _environmentReservationRepository.GetReservationStatesByUserId(userId);

        public async Task<object> GetTopHours()
            => await _environmentReservationRepository.GetTopHours();

        public async Task<object> GetTopReservations()
            => await _environmentReservationRepository.GetTopReservations();

        public async Task<Tuple<bool, string>> ReserveEnrionmentByUser(Guid Id, string Date, string StartTime, string EndTime, string userId)
            => await _environmentReservationRepository.ReserveEnrionmentByUser(Id, Date, StartTime, EndTime, userId);

        public async Task<Tuple<bool, string>> ReserveEnvironmentByExternalUser(Guid Id, string Date, string startTime, string endTime, Guid currentExternalUser)
            => await _environmentReservationRepository.ReserveEnvironmentByExternalUser(Id, Date, startTime, endTime, currentExternalUser);

        public async Task Update(EnvironmentReservation entity)
            => await _environmentReservationRepository.Update(entity);
    }
}
