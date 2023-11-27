using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IPettyCashService
    {
        Task<IEnumerable<PettyCash>> GetPettyCashs();
        Task<PettyCash> GetPettyCash(Guid id);
        Task InsertPettyCash(PettyCash pettyCash);
        Task ClosePettyCash(Guid id);
        Task<PettyCash> GetUserPettyCash(string id);
        Task<PettyCash> GetUserPettyCashWithSerialNumber(string id);
        Task<int> GetInvoiceCount(Guid id);
        Task<bool> HasPettyCash(string id);
        Task<DataTablesStructs.ReturnedData<object>> GetUserPettyCashesDatatable(DataTablesStructs.SentParameters parameters, string userId, DateTime? date = null);
        Task<IEnumerable<Invoice>> GetCurrentPettyCashInvoices(string id);
        Task<IEnumerable<Invoice>> GetPettyCashInvoices(Guid id);
        Task<decimal> GetTotalAmount(Guid id);
        Task Update(PettyCash pettyCash);
        Task<IEnumerable<PettyCash>> GetUserPettyCashes(string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetInvoiceDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, string searchValue = null);
        Task<List<PettyCash>> GetReportPaymentByDate(string date);
        Task<PettyCash> GetPettyCashByClosedAndUserId(string userId);
    }
}
