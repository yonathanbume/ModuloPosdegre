using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IExpenseOutputService
    {
        Task InsertExpenseOutput(ExpenseOutput expenseOutput);
        Task UpdateExpenseOutput(ExpenseOutput expenseOutput);
        Task DeleteExpenseOutput(ExpenseOutput expenseOutput);
        Task<List<ExpenseOutput>> GetExpenseOutputReportList(Guid id);
        Task<ExpenseOutput> GetExpenseOutputById(Guid id);
        Task<IEnumerable<ExpenseOutput>> GetAllExpenseOutputs();
        Task<DataTablesStructs.ReturnedData<object>> GetExpenseOutPutsDatatables(DataTablesStructs.SentParameters sentParameters, string userId, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetExpenseOutPutsTesoDatatables(DataTablesStructs.SentParameters sentParameters, string search);
    }
}
