using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface ISiafExpenseService
    {
        Task InsertSiafExpense(SiafExpense siafExpense);
        Task UpdateSiafExpense(SiafExpense siafExpense);
        Task DeleteSiafExpense(SiafExpense siafExpense);
        Task<SiafExpense> GetSiafExpenseById(Guid id);
        Task<IEnumerable<SiafExpense>> GetAllSiafExpenses();
        Task<DataTablesStructs.ReturnedData<object>> GetSiafExpenseDatatable(DataTablesStructs.SentParameters sentParameters, byte? status = null);
        Task InsertRangeSiafExpense(List<SiafExpense> siafExpense);
        Task<DataTablesStructs.ReturnedData<object>> GetDerivedExpensesDatatable(DataTablesStructs.SentParameters sentParameters, string userId);
    }
}
