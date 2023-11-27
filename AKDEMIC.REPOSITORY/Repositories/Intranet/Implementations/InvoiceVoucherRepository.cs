using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class InvoiceVoucherRepository : Repository<InvoiceVoucher>, IInvoiceVoucherRepository
    {
        public InvoiceVoucherRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetInvoiceVoucher(Guid id)
        {
            var result = await _context.InvoiceVouchers
                .Where(i => i.InvoiceId == id)
                .Select(i => new
                {
                    id = i.Id,
                    code = i.Code,
                    cost = i.Amount
                }).ToListAsync();

            return result;  
        }
    }
}
