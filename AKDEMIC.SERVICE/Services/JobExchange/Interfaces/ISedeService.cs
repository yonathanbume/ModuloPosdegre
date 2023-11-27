using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface ISedeService
    {
        Task<object> GetSedesByCompany(Guid companyId);
        Task<object> GetSedesByUser(string userId);
        Task<IEnumerable<Sede>> GetListSedesByCompany(Guid companyId);
        Task DeleteRange(IEnumerable<Sede> sedes);
        Task InsertRange(IEnumerable<Sede> sedes);
       
    }
}
