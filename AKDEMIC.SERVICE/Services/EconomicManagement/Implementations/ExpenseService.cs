using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task InsertExpense(Expense expense) =>
            await _expenseRepository.Insert(expense);

        public async Task UpdateExpense(Expense expense) =>
            await _expenseRepository.Update(expense);

        public async Task DeleteExpense(Expense expense) =>
            await _expenseRepository.Delete(expense);

        public async Task<Expense> GetExpenseById(Guid id) =>
            await _expenseRepository.Get(id);

        public async Task<IEnumerable<Expense>> GetAllExpenses() =>
            await _expenseRepository.GetAll();

        public async Task<object> GetExpensesBudgetBalance(Guid id)
            => await _expenseRepository.GetExpensesBudgetBalance(id);

        public async Task<List<Expense>> GetExpensesList(Guid id)
            => await _expenseRepository.GetExpensesList(id);
        public IQueryable<Expense> ExpensesQry(DateTime date)
            => _expenseRepository.ExpensesQry(date);
        public async Task<DataTablesStructs.ReturnedData<object>> GetExpenseDatatable(DataTablesStructs.SentParameters sentParameters, string search)
            => await _expenseRepository.GetExpenseDatatable(sentParameters, search);
        public async Task<List<Expense>> GetExpensesListById(Guid id)
            => await _expenseRepository.GetExpensesListById(id);

        public async Task CancelExpense(Guid id)
        {
            await _expenseRepository.CancelExpense(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetOrderExpensesDatatable(DataTablesStructs.SentParameters sentParameters, Guid orderId)
            => await _expenseRepository.GetOrderExpensesDatatable(sentParameters, orderId);
    }
}
