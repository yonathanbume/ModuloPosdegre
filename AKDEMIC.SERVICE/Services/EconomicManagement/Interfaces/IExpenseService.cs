using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IExpenseService
    {
        Task InsertExpense(Expense expense);
        Task UpdateExpense(Expense expense);
        Task DeleteExpense(Expense expense);
        Task<Expense> GetExpenseById(Guid id);
        Task<IEnumerable<Expense>> GetAllExpenses();
        Task<object> GetExpensesBudgetBalance(Guid id);
        Task<List<Expense>> GetExpensesList(Guid id);
        IQueryable<Expense> ExpensesQry(DateTime date);
        Task<DataTablesStructs.ReturnedData<object>> GetExpenseDatatable(DataTablesStructs.SentParameters sentParameters, string search);
        Task<List<Expense>> GetExpensesListById(Guid id);
        Task CancelExpense(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetOrderExpensesDatatable(DataTablesStructs.SentParameters sentParameters, Guid orderId);
    }
}
