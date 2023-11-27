using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Portal;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface IFinancialStatementFilesService
    {
        Task Insert(FinancialStatementFile regulation);
        Task<FinancialStatementFile> Get(Guid id);
        Task Update(FinancialStatementFile regulation);
        Task DeleteById(Guid id);
        Task<List<FinancialStatementFile>> GetByFinancialStatementId(Guid FinancialStatementId);
    }
}
