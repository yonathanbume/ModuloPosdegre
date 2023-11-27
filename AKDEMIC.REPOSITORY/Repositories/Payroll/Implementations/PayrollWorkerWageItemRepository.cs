using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class PayrollWorkerWageItemRepository : Repository<PayrollWorkerWageItem>, IPayrollWorkerWageItemRepository
    {
        public PayrollWorkerWageItemRepository(AkdemicContext context): base(context)
        {
        }

        public async Task DeleteByPayrollWorkerId(Guid payrollWorkerId)
        {
            var payrollWorkersWageItems = await _context.PayrollWorkerWageItems
                .Where(x => x.PayrollWorkerId == payrollWorkerId).ToListAsync();
            _context.PayrollWorkerWageItems.RemoveRange(payrollWorkersWageItems);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PayrollWorkerWageItem>> GetAllByPayrollWorkerId(Guid payrollWorkerId)
            => await _context.PayrollWorkerWageItems
                .Where(x => x.PayrollWorkerId == payrollWorkerId)
                .Select(x => new PayrollWorkerWageItem
                {
                    Id = x.Id,
                    WageItemId = x.WageItemId,
                    WageItem = new WageItem
                    {
                        Amount = x.WageItem.Amount
                    }
                }).ToListAsync();
    }
}
