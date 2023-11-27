using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IBalanceTransferRepository : IRepository<BalanceTransfer>
    {
        IQueryable<BalanceTransfer> TransfersQry(DateTime date);
        Task<DataTablesStructs.ReturnedData<object>> GetBalanceTransferDatatable(DataTablesStructs.SentParameters sentParameters, string search);
        Task<IEnumerable<BalanceTransfer>> GetAllBalanceTransfers();
    }
}
