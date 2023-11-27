using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class InvoiceDetailsRepository : Repository<InvoiceDetail>, IInvoiceDetailsRepository
    {
        public InvoiceDetailsRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<IEnumerable<InvoiceDetail>> GetByInvoice(Guid invoiceId)
            => await _context.InvoiceDetails
                .Include(x => x.Invoice)
                .Include(x => x.CurrentAccount)
                .Include(x => x.Concept.CurrentAccount)
                .Include(x => x.Concept.AccountingPlan)
                .Include(x => x.Concept.Classifier)
                .Where(x => x.InvoiceId == invoiceId)
                .ToListAsync();

        public async Task<IEnumerable<InvoiceDetail>> GetByPettyCash(Guid pettyCashId)
            => await _context.InvoiceDetails
                .Where(x => x.Invoice.PettyCashId == pettyCashId)
                .Include(x => x.Invoice)
                .Include(x => x.CurrentAccount)
                .Include(x => x.Concept.CurrentAccount)
                .Include(x => x.Concept.AccountingPlan)
                .ToListAsync();

        public async Task<IEnumerable<InvoiceDetail>> GetByPettyCashBook(Guid pettyCashBookId)
        {
            var book = await _context.PettyCashBooks.FindAsync(pettyCashBookId);

            var startDate = book.Date.Date.ToUtcDateTime();
            var endDate = book.Date.Date.AddDays(1).AddTicks(-1).ToUtcDateTime();

            var details = await _context.InvoiceDetails
                  .Where(x => startDate <= x.Invoice.PettyCash.InitialDate && x.Invoice.PettyCash.InitialDate <= endDate
                  && x.Invoice.PettyCash.UserId == book.UserId && x.Invoice.Canceled)
                  .Include(x => x.Invoice)
                  .Include(x => x.CurrentAccount)
                  .Include(x => x.Concept.CurrentAccount)
                  .Include(x => x.Concept.AccountingPlan)
                  .ToListAsync();

            return details;
        }
    }
}
