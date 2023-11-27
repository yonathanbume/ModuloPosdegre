using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface IFinancialExecutionDetailsRepository:IRepository<FinancialExecutionDetail>
    {
        Task<List<FinancialExecutionDetail>> GetByFinancialExecutionId(Guid id);
    }
}
