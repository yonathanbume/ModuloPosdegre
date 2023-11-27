using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Portal;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface IFinancialExecutionDetailsService
    {
        Task Insert(FinancialExecutionDetail regulation);
        Task<FinancialExecutionDetail> Get(Guid id);
        Task Update(FinancialExecutionDetail regulation);
        Task DeleteById(Guid id);
        Task<List<FinancialExecutionDetail>> GetByFinancialExecutionId(Guid financialId);
    }
}
