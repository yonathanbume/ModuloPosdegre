using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class BudgetBalanceService : IBudgetBalanceService
    {
        private readonly IBudgetBalanceRepository _budgetBalanceRepository;
        public BudgetBalanceService(IBudgetBalanceRepository budgetBalanceRepository)
        {
            _budgetBalanceRepository = budgetBalanceRepository;
        }

        public async Task InsertBudgetBalance(BudgetBalance budgetBalance) =>
            await _budgetBalanceRepository.Insert(budgetBalance);

        public async Task UpdateBudgetBalance(BudgetBalance budgetBalance) =>
            await _budgetBalanceRepository.Update(budgetBalance);

        public async Task DeleteBudgetBalance(BudgetBalance budgetBalance) =>
            await _budgetBalanceRepository.Delete(budgetBalance);

        public async Task<BudgetBalance> GetBudgetBalanceById(Guid id) =>
            await _budgetBalanceRepository.Get(id);

        public async Task<IEnumerable<BudgetBalance>> GetAllBudgetBalances() =>
            await _budgetBalanceRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetBudgetBalanceDatatable(DataTablesStructs.SentParameters sentParameters)
            => await _budgetBalanceRepository.GetBudgetBalanceDatatable(sentParameters);
    }
}
