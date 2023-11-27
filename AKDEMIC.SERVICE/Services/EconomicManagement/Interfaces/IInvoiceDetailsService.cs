using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IInvoiceDetailsService
    {
        Task<IEnumerable<InvoiceDetail>> GetByInvoice(Guid invoiceId);
        Task InsertRange(IEnumerable<InvoiceDetail> invoiceDetails);
        Task DeleteRange(IEnumerable<InvoiceDetail> invoiceDetails);
        Task<IEnumerable<InvoiceDetail>> GetByPettyCash(Guid pettyCashId);
        Task<IEnumerable<InvoiceDetail>> GetByPettyCashBook(Guid pettyCashBookId);
    }
}
