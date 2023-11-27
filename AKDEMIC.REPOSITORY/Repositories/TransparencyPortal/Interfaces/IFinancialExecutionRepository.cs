using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface IFinancialExecutionRepository:IRepository<FinancialExecution>
    {
        Task<List<FinancialExecution>> GetByType(int type);
    }
}
