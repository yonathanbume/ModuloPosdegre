using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IInvoiceDetailsRepository : IRepository<InvoiceDetail>
    {
        Task<IEnumerable<InvoiceDetail>> GetByInvoice(Guid invoiceId);
        Task<IEnumerable<InvoiceDetail>> GetByPettyCash(Guid pettyCashId);
        Task<IEnumerable<InvoiceDetail>> GetByPettyCashBook(Guid pettyCashBookId);
    }
}
