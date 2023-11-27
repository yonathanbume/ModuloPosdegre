using AKDEMIC.ENTITIES.Models.EconomicManagement;
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
    public class CreditNoteRepository : Repository<CreditNote>, ICreditNoteRepository
    {
        public CreditNoteRepository(AkdemicContext context) : base(context) { }

        public async Task<List<CreditNote>> GetCreditNotesByIdList(Guid id)
            => await _context.CreditNotes.Where(y => y.InvoiceId == id).ToListAsync();
        public async Task<int> GetCreditNoteNextNumber()
            => await _context.CreditNotes.OrderByDescending(x => x.Number).Select(x => x.Number).FirstOrDefaultAsync();
    }
}
