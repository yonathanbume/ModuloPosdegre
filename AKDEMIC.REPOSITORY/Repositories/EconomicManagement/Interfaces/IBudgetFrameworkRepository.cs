using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IBudgetFrameworkRepository : IRepository<BudgetFramework>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetBudgetFrameworkDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<decimal> GetExpenseByDependecyAndYear(string costCenter, int year);
    }
}
