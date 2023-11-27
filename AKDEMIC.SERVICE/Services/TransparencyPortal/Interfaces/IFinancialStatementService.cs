using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Portal;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface IFinancialStatementService
    {
        Task Insert(FinancialStatement regulation);
        Task<FinancialStatement> Get(Guid id);
        Task Update(FinancialStatement regulation);
        Task DeleteById(Guid id);
        Task<IEnumerable<FinancialStatement>> GetAll();
        Task<bool> ExistAnyWithName(Guid id, string name);
        Task<IEnumerable<FinancialStatement>> GetBySlug(string slug);
    }
}
