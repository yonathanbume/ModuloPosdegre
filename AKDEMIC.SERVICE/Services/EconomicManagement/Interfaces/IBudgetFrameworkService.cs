using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IBudgetFrameworkService
    {
        Task InsertBudgetFramework(BudgetFramework budgetFramework);
        Task UpdateBudgetFramework(BudgetFramework budgetFramework);
        Task DeleteBudgetFramework(BudgetFramework budgetFramework);
        Task<BudgetFramework> GetBudgetFrameworkById(Guid id);
        Task<IEnumerable<BudgetFramework>> GetAllBudgetFrameworks();
        Task<DataTablesStructs.ReturnedData<object>> GetBudgetFrameworkDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<decimal> GetExpenseByDependecyAndYear(string costCenter, int year);
    }
}
