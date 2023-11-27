using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class InvoiceDetailsService : IInvoiceDetailsService
    {
        private readonly IInvoiceDetailsRepository _invoiceDetailsRepository;

        public InvoiceDetailsService(IInvoiceDetailsRepository invoiceDetailsRepository)
        {
            _invoiceDetailsRepository = invoiceDetailsRepository;
        }

        public async Task DeleteRange(IEnumerable<InvoiceDetail> invoiceDetails)
            => await _invoiceDetailsRepository.DeleteRange(invoiceDetails);

        public async Task<IEnumerable<InvoiceDetail>> GetByInvoice(Guid invoiceId)
            => await _invoiceDetailsRepository.GetByInvoice(invoiceId);

        public async Task<IEnumerable<InvoiceDetail>> GetByPettyCash(Guid pettyCashId)
            => await _invoiceDetailsRepository.GetByPettyCash(pettyCashId);

        public async Task<IEnumerable<InvoiceDetail>> GetByPettyCashBook(Guid pettyCashBookId)
            => await _invoiceDetailsRepository.GetByPettyCashBook(pettyCashBookId);

        public async Task InsertRange(IEnumerable<InvoiceDetail> invoiceDetails)
            => await _invoiceDetailsRepository.InsertRange(invoiceDetails);
    }
}
