using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IPettyCashRepository : IRepository<PettyCash>
    {
        Task<PettyCash> GetUserPettyCash(string id);
        Task<PettyCash> GetUserPettyCashWithSerialNumber(string id);
        Task<int> GetInvoiceCount(Guid id);
        Task<bool> HasPettyCash(string id);
        Task<IEnumerable<Invoice>> GetPettyCashInvoices(Guid id);
        Task<IEnumerable<Invoice>> GetCurrentPettyCashInvoices(string id);
        Task<decimal> GetTotalAmount(Guid id);
        Task<IEnumerable<PettyCash>> GetUserPettyCashes(string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetInvoiceDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, string searchValue = null);
        Task<List<PettyCash>> GetReportPaymentByDate(string date);
        Task<PettyCash> GetPettyCashByClosedAndUserId(string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetUserPettyCashesDatatable(DataTablesStructs.SentParameters parameters, string userId, DateTime? date = null);
    }
}
