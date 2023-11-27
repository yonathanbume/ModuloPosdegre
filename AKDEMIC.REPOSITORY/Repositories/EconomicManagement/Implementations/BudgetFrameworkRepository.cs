using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class BudgetFrameworkRepository : Repository<BudgetFramework>, IBudgetFrameworkRepository
    {
        public BudgetFrameworkRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetBudgetFrameworkDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<BudgetFramework, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Budget;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Year;
                    break;
                default:
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
            }

            var query = _context.BudgetFrameworks
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    Id = x.Id,
                    costCenter = x.Dependency.Name,
                    budget = x.Budget,
                    year = x.Year
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

        public async Task<decimal> GetExpenseByDependecyAndYear(string costCenter, int year)
        {
            var amounts = await _context.StructureForExpenses
                .Where(x => x.Dependency.Name == costCenter && x.ExecutionYear == year)
                .Select(x => new
                {
                    expense = x.Amount
                }).ToListAsync();

            decimal expense = 0;
            foreach (var item in amounts)
            {
                expense = expense + item.expense;
            }

            return expense;
        }
    }
}
