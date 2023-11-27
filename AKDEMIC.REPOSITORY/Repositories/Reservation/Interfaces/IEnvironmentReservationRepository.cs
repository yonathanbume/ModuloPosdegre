using AKDEMIC.ENTITIES.Models.Reservations;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Reservation.Interfaces
{
    public interface IEnvironmentReservationRepository : IRepository<EnvironmentReservation>
    {
        Task<bool> AnyByEnvironmentId(Guid environmentId);
        Task<object> GetAllByFilters(Guid rid, int reservationstate);
        Task<Tuple<bool, string>> ReserveEnvironmentByExternalUser(Guid Id, string Date, string startTime, string endTime, Guid currentExternalUser);
        Task<object> GetAllEnvironmentReservationsByExternalUser(string userId);
        Task<Tuple<bool, string>> ReserveEnrionmentByUser(Guid Id, string Date, string StartTime, string EndTime, string userId);
        Task<object> GetEnvironmentReservationsByUser(string userId);
        Task<object> GetReservationStatesByUserId(string userId);
        Task<object> GetTopReservations();
        Task<object> GetTopHours();
        Task<object> GetNextEnvironmetnsByUserId(string userId);
    }
}
