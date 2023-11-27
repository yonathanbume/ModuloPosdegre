using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class PettyCashService : IPettyCashService
    {
        private readonly IPettyCashRepository _pettyCashRepository;

        public PettyCashService(IPettyCashRepository pettyCashRepository)
        {
            _pettyCashRepository = pettyCashRepository;
        }

        public async Task<IEnumerable<PettyCash>> GetPettyCashs() => await _pettyCashRepository.GetAll();

        public async Task<PettyCash> GetPettyCash(Guid id) => await _pettyCashRepository.Get(id);

        public async Task InsertPettyCash(PettyCash pettyCash) => await _pettyCashRepository.Insert(pettyCash);
        
        Task IPettyCashService.ClosePettyCash(Guid id)
        {
            throw new NotImplementedException();
        }

        public void ClosePettyCash(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<PettyCash> GetUserPettyCash(string id) => await _pettyCashRepository.GetUserPettyCash(id);
        public async Task<PettyCash> GetUserPettyCashWithSerialNumber(string id) => await _pettyCashRepository.GetUserPettyCashWithSerialNumber(id);

        public async Task<int> GetInvoiceCount(Guid id) => await _pettyCashRepository.GetInvoiceCount(id);
        
        public async Task<bool> HasPettyCash(string id) => await _pettyCashRepository.HasPettyCash(id);

        public async Task<IEnumerable<Invoice>> GetCurrentPettyCashInvoices(string id) =>
            await _pettyCashRepository.GetCurrentPettyCashInvoices(id);

        public async Task<IEnumerable<Invoice>> GetPettyCashInvoices(Guid id) =>
            await _pettyCashRepository.GetPettyCashInvoices(id);

        public async Task<decimal> GetTotalAmount(Guid id) => await _pettyCashRepository.GetTotalAmount(id);
        public async Task Update(PettyCash pettyCash) => await _pettyCashRepository.Update(pettyCash);

        public async Task<IEnumerable<PettyCash>> GetUserPettyCashes(string userId) =>
            await _pettyCashRepository.GetUserPettyCashes(userId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetInvoiceDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, string searchValue = null)
            => await _pettyCashRepository.GetInvoiceDatatable(sentParameters, id, searchValue);

        public async Task<List<PettyCash>> GetReportPaymentByDate(string date)
            => await _pettyCashRepository.GetReportPaymentByDate(date);
        public async Task<PettyCash> GetPettyCashByClosedAndUserId(string userId)
            => await _pettyCashRepository.GetPettyCashByClosedAndUserId(userId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserPettyCashesDatatable(DataTablesStructs.SentParameters parameters, string userId, DateTime? date = null)
            => await _pettyCashRepository.GetUserPettyCashesDatatable(parameters, userId, date);
    }
}
