using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class CashierDependencyService : ICashierDependencyService
    {
        private readonly ICashierDependencyRepository _cashierDependencyRepository;

        public CashierDependencyService(ICashierDependencyRepository cashierDependencyRepository)
        {
            _cashierDependencyRepository = cashierDependencyRepository;
        }

        public async Task InsertCashierDependency(CashierDependency cashierDependency) =>
            await _cashierDependencyRepository.Insert(cashierDependency);

        public async Task UpdateCashierDependency(CashierDependency cashierDependency) =>
            await _cashierDependencyRepository.Update(cashierDependency);

        public async Task DeleteCashierDependency(CashierDependency cashierDependency) =>
            await _cashierDependencyRepository.Delete(cashierDependency);

        public async Task<CashierDependency> GetCashierDependencyById(Guid id) =>
            await _cashierDependencyRepository.Get(id);

        public async Task<IEnumerable<CashierDependency>> GetAllCashierDependencys() =>
            await _cashierDependencyRepository.GetAll();
        public async Task<List<CashierDependency>> GetCashierDependeciesByIdList(string id)
            => await _cashierDependencyRepository.GetCashierDependeciesByIdList(id);
        public  void RemoveRange(List<CashierDependency> cashierDependencies)
            =>  _cashierDependencyRepository.RemoveRange(cashierDependencies);
        public async Task InserRange(List<CashierDependency> cashierDependencies)
            => await _cashierDependencyRepository.InsertRange(cashierDependencies);
        public async Task<object> GetConcepts(string userId)
            => await _cashierDependencyRepository.GetConcepts(userId);

        public async Task<List<CashierDependency>> GetCashierDependenciesByUserId(string userId)
            => await _cashierDependencyRepository.GetCashierDependenciesByUserId(userId);
        public async Task<object> GetCashierDependenciesJsonByUserId(string userId)
            => await _cashierDependencyRepository.GetCashierDependenciesJsonByUserId(userId);

        public async Task<object> GetAllConceptsDatatatable() => await _cashierDependencyRepository.GetAllConceptsDatatatable();
    }
}
