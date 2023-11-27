using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class BankTemporalPaymentRepository : Repository<BankTemporalPayment>, IBankTemporalPaymentRepository
    {
        public BankTemporalPaymentRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task BulkInsertAsync(List<BankTemporalPayment> bankTemporalPayments)
        {
            if (bankTemporalPayments.Any()) await _context.BulkInsertAsync(bankTemporalPayments);
        }

        public async Task BatchDeleteAsync()
        {
            if (await _context.BankTemporalPayments.AnyAsync()) await _context.BankTemporalPayments.BatchDeleteAsync();
        }

        public Task<List<BankTemporalPayment>> GetAllValidPayments()
        {
            var payments = _context.BankTemporalPayments.Where(x => x.IsValid).ToListAsync();
            return payments;
        }

        public Task<List<BankTemporalPayment>> GetAllInvalidPayments()
        {
            var payments = _context.BankTemporalPayments.Where(x => !x.IsValid).ToListAsync();
            return payments;
        }
    }
}
