using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface ISedeRepository : IRepository<Sede>
    {
        Task<object> GetSedesByCompany(Guid companyId);
        Task<object> GetSedesByUser(string userId);
        Task<IEnumerable<Sede>> GetListSedesByCompany(Guid companyId);
    }
}
