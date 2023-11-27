using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface ICashierDependencyRepository : IRepository<CashierDependency>
    {
        Task<List<CashierDependency>> GetCashierDependeciesByIdList(string id);
        Task<object> GetConcepts(string userId);
        Task<List<CashierDependency>> GetCashierDependenciesByUserId(string userId);
        Task<object> GetCashierDependenciesJsonByUserId(string userId);
        Task<object> GetAllConceptsDatatatable();
    }
}
