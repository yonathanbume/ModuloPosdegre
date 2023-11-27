using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IBudgetBalanceService
    {
        Task InsertBudgetBalance(BudgetBalance budgetBalance);
        Task UpdateBudgetBalance(BudgetBalance budgetBalance);
        Task DeleteBudgetBalance(BudgetBalance budgetBalance);
        Task<BudgetBalance> GetBudgetBalanceById(Guid id);
        Task<IEnumerable<BudgetBalance>> GetAllBudgetBalances();
        Task<DataTablesStructs.ReturnedData<object>> GetBudgetBalanceDatatable(DataTablesStructs.SentParameters sentParameters);
    }
}
