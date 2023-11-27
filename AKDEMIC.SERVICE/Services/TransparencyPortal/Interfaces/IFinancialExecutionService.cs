using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Portal;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface IFinancialExecutionService
    {
        Task Insert(FinancialExecution regulation);
        Task<FinancialExecution> Get(Guid id);
        Task Update(FinancialExecution regulation);
        Task DeleteById(Guid id);
        Task<List<FinancialExecution>> GetByType(int type);
    }
}
