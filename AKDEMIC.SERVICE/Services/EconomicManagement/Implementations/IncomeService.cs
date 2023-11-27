using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Income;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class IncomeService : IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;

        public IncomeService(IIncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }

        public async Task InsertIncome(Income income) =>
            await _incomeRepository.Insert(income);

        public async Task UpdateIncome(Income income) =>
            await _incomeRepository.Update(income);

        public async Task DeleteIncome(Income income) =>
            await _incomeRepository.Delete(income);

        public async Task<Income> GetIncomeById(Guid id) =>
            await _incomeRepository.Get(id);

        public async Task<IEnumerable<Income>> GetAllIncomes() =>
            await _incomeRepository.GetAll();
        public async Task AddRangeAsync(List<Income> incomes)
            => await _incomeRepository.AddRange(incomes);

        public async Task<DataTablesStructs.ReturnedData<DailyIncomeTemplate>> GetDailyIncomeDatatableClientSide(DateTime startDate, DateTime endDate, Guid? dependencyId = null)
            => await _incomeRepository.GetDailyIncomeDatatableClientSide(startDate, endDate, dependencyId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetIncomesByDependencyDatatable(DataTablesStructs.SentParameters sentParameters, int year,Guid? dependencyId = null)
            => await _incomeRepository.GetIncomesByDependencyDatatable(sentParameters,year,dependencyId);

        public async Task<IEnumerable<Income>> GetAllByDependency(Guid dependencyId)
            => await _incomeRepository.GetAllByDependency(dependencyId);

        public async Task InsertRange(List<Income> incomes)
            => await _incomeRepository.InsertRange(incomes);

        public async Task<List<Income>> GetByInvoiceId(Guid invoiceId)
            => await _incomeRepository.GetByInvoiceId(invoiceId);

        public async Task DeleteRange(List<Income> incomes)
            => await _incomeRepository.DeleteRange(incomes);
    }
}
