using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IBankTemporalPaymentService
    {
        Task BulkInsertAsync(List<BankTemporalPayment> bankTemporalPayments);
        Task InsertRange(List<BankTemporalPayment> bankTemporalPayments);

        Task BatchDeleteAsync();
        Task DeleteAll();
        Task DeleteRange(IEnumerable<BankTemporalPayment> bankTemporalPayments);

        Task<IEnumerable<BankTemporalPayment>> GetAll();
        Task<List<BankTemporalPayment>> GetAllValidPayments();
        Task<List<BankTemporalPayment>> GetAllInvalidPayments();
    }
}
