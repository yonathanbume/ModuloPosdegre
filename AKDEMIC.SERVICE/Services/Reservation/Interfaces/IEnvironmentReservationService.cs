using AKDEMIC.ENTITIES.Models.Reservations;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Reservation.Interfaces
{
    public interface IEnvironmentReservationService
    {
        Task<bool> AnyByEnvironmentId(Guid environmentId);
        Task<object> GetAllByFilters(Guid rid, int reservationstate);
        Task<EnvironmentReservation> Get(Guid id);
        Task Update(EnvironmentReservation entity);
        Task<Tuple<bool, string>> ReserveEnvironmentByExternalUser(Guid Id, string Date, string startTime, string endTime, Guid currentExternalUser);
        Task<object> GetAllEnvironmentReservationsByExternalUser(string userId);
        Task<Tuple<bool, string>> ReserveEnrionmentByUser(Guid Id, string Date, string StartTime, string EndTime, string userId);
        Task<object> GetEnvironmentReservationsByUser(string userId);
        Task<object> GetReservationStatesByUserId(string userId);
        Task<object> GetTopReservations();
        Task<object> GetNextEnvironmetnsByUserId(string userId);
        Task<object> GetTopHours();
    }
}
