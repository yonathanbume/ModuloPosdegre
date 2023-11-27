using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class BudgetBalanceRepository : Repository<BudgetBalance>, IBudgetBalanceRepository
    {
        public BudgetBalanceRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<object> GetStructureExpense()
        {
            var exist = true;
            var content = await _context.BudgetFrameworks.CountAsync();
            if (content == 0)
                exist = false;

            var result = await _context.StructureForExpenses
                                .GroupBy(u => new { u.Dependency.Acronym, u.ExecutionYear })
                                .Select(x => new
                                {
                                    costCenter = x.Key.Acronym,
                                    expense = x.Sum(y => y.Amount),
                                    year = x.Key.ExecutionYear,
                                    exist
                                }).ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetBudgetBalanceDatatable(DataTablesStructs.SentParameters sentParameters)
        {

            var query = _context.StructureForExpenses
                                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var exist = true;
            var content = await _context.BudgetFrameworks.CountAsync();
            var listBudgetFra = new List<BudgetFramework>();

            if (content == 0)
                exist = false;

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Join(_context.BudgetFrameworks, bu => bu.DependencyId, qu => qu.DependencyId, (bu, qu) => new { bu, qu})
                .Join(_context.Dependencies, de => de.bu.DependencyId, p => p.Id, (de, p) => new { de, p})
                .GroupBy(u => new { u.p.Name, u.de.bu.ExecutionYear, u.de.qu.Budget})
                  .Select(x => new
                  {
                      costCenter = x.Key.Name,
                      budgetFramework = exist ? x.Key.Budget : 0,
                      expense = x.Sum(y => y.de.bu.Amount),
                      year = x.Key.ExecutionYear,
                      balance = exist ? x.Key.Budget - x.Sum(y => y.de.bu.Amount) : x.Sum(y => y.de.bu.Amount) * (-1)
                  }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
