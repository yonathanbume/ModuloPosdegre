using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class BankTemporalPaymentService : IBankTemporalPaymentService
    {
        private readonly IBankTemporalPaymentRepository _bankTemporalPaymentRepository;
        public BankTemporalPaymentService(IBankTemporalPaymentRepository bankTemporalPaymentRepository)
        {
            _bankTemporalPaymentRepository = bankTemporalPaymentRepository;
        }

        public async Task BatchDeleteAsync() => await _bankTemporalPaymentRepository.BatchDeleteAsync();

        public async Task BulkInsertAsync(List<BankTemporalPayment> bankTemporalPayments) => await _bankTemporalPaymentRepository.BulkInsertAsync(bankTemporalPayments);

        public async Task DeleteAll() {

            var tmpPayments = await _bankTemporalPaymentRepository.GetAll();
            await _bankTemporalPaymentRepository.DeleteRange(tmpPayments);
        }

        public async Task DeleteRange(IEnumerable<BankTemporalPayment> bankTemporalPayments)
            => await _bankTemporalPaymentRepository.DeleteRange(bankTemporalPayments);

        public async Task<IEnumerable<BankTemporalPayment>> GetAll() => await _bankTemporalPaymentRepository.GetAll();

        public async Task<List<BankTemporalPayment>> GetAllInvalidPayments()
             => await _bankTemporalPaymentRepository.GetAllInvalidPayments();

        public async Task<List<BankTemporalPayment>> GetAllValidPayments()
             => await _bankTemporalPaymentRepository.GetAllValidPayments();

        public async Task InsertRange(List<BankTemporalPayment> bankTemporalPayments)
            => await _bankTemporalPaymentRepository.InsertRange(bankTemporalPayments);
    }
}
