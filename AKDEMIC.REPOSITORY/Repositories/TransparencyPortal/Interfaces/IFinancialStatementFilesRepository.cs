using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces
{
    public interface IFinancialStatementFilesRepository:IRepository<FinancialStatementFile>
    {
        Task<List<FinancialStatementFile>> GetByFinancialStatementId(Guid id);
    }
}
