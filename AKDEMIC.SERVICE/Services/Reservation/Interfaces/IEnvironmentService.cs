using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Reservation.Interfaces
{
    public interface IEnvironmentService
    {
        Task<IEnumerable<ENTITIES.Models.Reservations.Environment>> GetAll();
        Task<ENTITIES.Models.Reservations.Environment> Get(Guid id);
        Task Insert(ENTITIES.Models.Reservations.Environment entity);
        Task Update(ENTITIES.Models.Reservations.Environment entity);
        Task Delete(ENTITIES.Models.Reservations.Environment entity);
        Task<IEnumerable<Select2Structs.Result>> GetEnvironmentsSelect2ClientSide();
        Task<bool> AnyByCode(string code, Guid? ignoredId = null);
        Task<object> GetAllByFilters(bool? isActive = null, bool? reservationExternal = null);
    }
}
