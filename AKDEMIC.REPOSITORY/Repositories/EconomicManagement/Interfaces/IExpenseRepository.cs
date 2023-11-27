using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        Task<object> GetExpensesBudgetBalance(Guid id);
        Task<List<Expense>> GetExpensesList(Guid id);
        IQueryable<Expense> ExpensesQry(DateTime date);
        Task<DataTablesStructs.ReturnedData<object>> GetExpenseDatatable(DataTablesStructs.SentParameters sentParameters, string search);
        Task<List<Expense>> GetExpensesListById(Guid id);
        Task CancelExpense(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetOrderExpensesDatatable(DataTablesStructs.SentParameters sentParameters, Guid orderId);
    }
}
