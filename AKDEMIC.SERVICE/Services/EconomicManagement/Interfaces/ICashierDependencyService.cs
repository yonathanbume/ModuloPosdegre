using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface ICashierDependencyService
    {
        Task InsertCashierDependency(CashierDependency cashierDependency);
        Task UpdateCashierDependency(CashierDependency cashierDependency);
        Task DeleteCashierDependency(CashierDependency cashierDependency);
        Task<CashierDependency> GetCashierDependencyById(Guid id);
        Task<IEnumerable<CashierDependency>> GetAllCashierDependencys();
        Task<List<CashierDependency>> GetCashierDependeciesByIdList(string id);
        void RemoveRange(List<CashierDependency> cashierDependencies);
        Task InserRange(List<CashierDependency> cashierDependencies);
        Task<object> GetConcepts(string userId);
        Task<List<CashierDependency>> GetCashierDependenciesByUserId(string userId);
        Task<object> GetCashierDependenciesJsonByUserId(string userId);
        Task<object> GetAllConceptsDatatatable();
    }
}
