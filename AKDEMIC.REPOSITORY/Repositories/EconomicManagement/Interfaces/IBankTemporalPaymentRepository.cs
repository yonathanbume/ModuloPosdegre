using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IBankTemporalPaymentRepository : IRepository<BankTemporalPayment>
    {
        Task BulkInsertAsync(List<BankTemporalPayment> bankTemporalPayments);

        Task BatchDeleteAsync();

        Task<List<BankTemporalPayment>> GetAllValidPayments();
        Task<List<BankTemporalPayment>> GetAllInvalidPayments();
    }
}
