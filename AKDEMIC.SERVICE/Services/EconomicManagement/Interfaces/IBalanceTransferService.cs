using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IBalanceTransferService
    {
        Task InsertBalanceTransfer(BalanceTransfer balanceTransfer);
        Task UpdateBalanceTransfer(BalanceTransfer balanceTransfer);
        Task DeleteBalanceTransfer(BalanceTransfer balanceTransfer);
        Task DeleteById(Guid id);
        Task<BalanceTransfer> GetBalanceTransferById(Guid id);
        Task<IEnumerable<BalanceTransfer>> GetAllBalanceTransfers();
        IQueryable<BalanceTransfer> TransfersQry(DateTime date);
        Task<DataTablesStructs.ReturnedData<object>> GetBalanceTransferDatatable(DataTablesStructs.SentParameters sentParameters, string search);
    }
}
