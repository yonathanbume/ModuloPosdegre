using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Reservation.Interfaces
{
    public interface IEnvironmentRepository : IRepository<ENTITIES.Models.Reservations.Environment>
    {
        Task<IEnumerable<Select2Structs.Result>> GetEnvironmentsSelect2ClientSide();
        Task<bool> AnyByCode(string code, Guid? ignoredId = null);
        Task<object> GetAllByFilters(bool? isActive = null, bool? reservationExternal = null);
    }
}
