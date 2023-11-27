using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class InvoiceVoucherService : IInvoiceVoucherService
    {
        private readonly IInvoiceVoucherRepository _invoiceVoucherRepository;

        public InvoiceVoucherService(IInvoiceVoucherRepository invoiceVoucherRepository)
        {
            _invoiceVoucherRepository = invoiceVoucherRepository;
        }

        public async Task InsertInvoiceVoucher(InvoiceVoucher invoiceVoucher) =>
            await _invoiceVoucherRepository.Insert(invoiceVoucher);

        public async Task UpdateInvoiceVoucher(InvoiceVoucher invoiceVoucher) =>
            await _invoiceVoucherRepository.Update(invoiceVoucher);

        public async Task DeleteInvoiceVoucher(InvoiceVoucher invoiceVoucher) =>
            await _invoiceVoucherRepository.Delete(invoiceVoucher);

        public async Task<InvoiceVoucher> GetInvoiceVoucherById(Guid id) =>
            await _invoiceVoucherRepository.Get(id);

        public async Task<IEnumerable<InvoiceVoucher>> GetAllInvoiceVouchers() =>
            await _invoiceVoucherRepository.GetAll();
        public async Task<object> GetInvoiceVoucher(Guid id)
            => await _invoiceVoucherRepository.GetInvoiceVoucher(id);
    }
}
