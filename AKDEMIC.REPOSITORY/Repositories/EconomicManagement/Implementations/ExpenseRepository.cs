using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetExpensesBudgetBalance(Guid id)
        {
            var expenses = await _context.Expenses
            .Where(x => x.DependencyId == id)
            .Select(x => new
            {
                date = x.Date.ToLocalDateFormat(),
                siaf = x.Code,
                invoice = x.Invoice,
                document = x.ReferenceDocument,
                order = x.Order,
                concept = x.Concept,
                month = $"{x.Date:MM/yyyy}",
                provision = 0.00M,
                income = 0.00M,
                expense = x.Amount
            })
            .ToListAsync();

            return expenses;
        }

        public async Task<List<Expense>> GetExpensesList(Guid id)
        {
            var expenses = await _context.Expenses
                .Where(x => x.DependencyId == id).ToListAsync();

            return expenses;
        }

        public IQueryable<Expense> ExpensesQry(DateTime date)
            => _context.Expenses.Where(x => x.Date.Date == date).AsQueryable();
        public async Task<DataTablesStructs.ReturnedData<object>> GetExpenseDatatable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<Expense, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Date;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Invoice;
                    break;
                case "3":
                    orderByPredicate = (x) => x.ReferenceDocument;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Order;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Concept;
                    break;
                case "6":
                    orderByPredicate = (x) => x.Date;
                    break;
                case "7":
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
                case "8":
                    orderByPredicate = (x) => x.Type;
                    break;
                case "9":
                    orderByPredicate = (x) => x.Amount;
                    break;
                default:
                    orderByPredicate = (x) => x.Date;
                    break;
            }


            var query = _context.Expenses
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Concept.Contains(search));
            }

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    id = x.Id,
                    code = x.Code,
                    invoice = x.Invoice,
                    document = x.ReferenceDocument,
                    concept = x.Concept,
                    month = $"{x.Month:MM/yyyy}",
                    amount = x.Amount,
                    date = $"{x.Date:dd/MM/yyyy}",
                    order = x.Order,
                    dependencyId = x.DependencyId,
                    dependency = x.Dependency.Name,
                    status = x.IsCanceled,
                    type = x.Type,
                    comment = x.Comment,
                    relatedOrder = x.RelatedOrderId
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
        public async Task<List<Expense>> GetExpensesListById(Guid id)
            => await _context.Expenses
                .Where(x => x.Id == id).ToListAsync();

        public async Task CancelExpense(Guid id)
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);
            expense.IsCanceled = true;
            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetOrderExpensesDatatable(DataTablesStructs.SentParameters sentParameters, Guid orderId)
        {
            Expression<Func<Expense, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Concept;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Invoice;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Amount;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Date;
                    break;
                default:
                    break;
            }


            var query = _context.Expenses
                .Where(x => x.RelatedOrderId == orderId)
                .AsNoTracking();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    receipt = x.Invoice,
                    concept = x.Concept,
                    amount = x.Amount,
                    date = $"{x.Date:dd/MM/yyyy}"
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

    }
}
