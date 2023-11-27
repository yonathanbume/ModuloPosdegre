using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IInvoiceVoucherService
    {
        Task InsertInvoiceVoucher(InvoiceVoucher invoiceVoucher);
        Task UpdateInvoiceVoucher(InvoiceVoucher invoiceVoucher);
        Task DeleteInvoiceVoucher(InvoiceVoucher invoiceVoucher);
        Task<InvoiceVoucher> GetInvoiceVoucherById(Guid id);
        Task<IEnumerable<InvoiceVoucher>> GetAllInvoiceVouchers();
        Task<object> GetInvoiceVoucher(Guid id);
    }
}
