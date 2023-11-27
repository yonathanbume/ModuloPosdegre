using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class MonthlyBalanceRepository : Repository<MonthlyBalance>, IMonthlyBalanceRepository
    {
        public MonthlyBalanceRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<MonthlyBalance> GetLastClosedBalance()
        {
            var lastClosedMonth = await _context.MonthlyBalances
                .Where(x => x.WasClosed)
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .FirstOrDefaultAsync();

            return lastClosedMonth;
        }
    }
}
