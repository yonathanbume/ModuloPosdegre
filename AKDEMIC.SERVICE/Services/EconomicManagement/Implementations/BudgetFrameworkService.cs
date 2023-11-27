using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class BudgetFrameworkService : IBudgetFrameworkService
    {
        private readonly IBudgetFrameworkRepository _budgetFrameworkRepository;
        public BudgetFrameworkService(IBudgetFrameworkRepository budgetFrameworkRepository)
        {
            _budgetFrameworkRepository = budgetFrameworkRepository;
        }

        public async Task InsertBudgetFramework(BudgetFramework budgetFramework) =>
            await _budgetFrameworkRepository.Insert(budgetFramework);

        public async Task UpdateBudgetFramework(BudgetFramework budgetFramework) =>
            await _budgetFrameworkRepository.Update(budgetFramework);

        public async Task DeleteBudgetFramework(BudgetFramework budgetFramework) =>
            await _budgetFrameworkRepository.Delete(budgetFramework);

        public async Task<BudgetFramework> GetBudgetFrameworkById(Guid id) =>
            await _budgetFrameworkRepository.Get(id);

        public async Task<IEnumerable<BudgetFramework>> GetAllBudgetFrameworks() =>
            await _budgetFrameworkRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetBudgetFrameworkDatatable(DataTablesStructs.SentParameters sentParameters)
            => await _budgetFrameworkRepository.GetBudgetFrameworkDatatable(sentParameters);

        public async Task<decimal> GetExpenseByDependecyAndYear(string costCenter, int year)
            => await _budgetFrameworkRepository.GetExpenseByDependecyAndYear(costCenter, year);
    }
}
