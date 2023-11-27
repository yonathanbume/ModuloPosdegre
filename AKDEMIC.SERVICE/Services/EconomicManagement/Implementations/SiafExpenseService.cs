using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class SiafExpenseService : ISiafExpenseService
    {
        private readonly ISiafExpenseRepository _siafExpenseRepository;

        public SiafExpenseService(ISiafExpenseRepository siafExpenseRepository)
        {
            _siafExpenseRepository = siafExpenseRepository;
        }

        public async Task InsertSiafExpense(SiafExpense siafExpense) =>
            await _siafExpenseRepository.Insert(siafExpense);

        public async Task UpdateSiafExpense(SiafExpense siafExpense) =>
            await _siafExpenseRepository.Update(siafExpense);

        public async Task DeleteSiafExpense(SiafExpense siafExpense) =>
            await _siafExpenseRepository.Delete(siafExpense);

        public async Task<SiafExpense> GetSiafExpenseById(Guid id) =>
            await _siafExpenseRepository.Get(id);

        public async Task<IEnumerable<SiafExpense>> GetAllSiafExpenses() =>
            await _siafExpenseRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetSiafExpenseDatatable(DataTablesStructs.SentParameters sentParameters, byte? status = null)
            => await _siafExpenseRepository.GetSiafExpenseDatatable(sentParameters, status);
        public async Task InsertRangeSiafExpense(List<SiafExpense> siafExpense)
            => await _siafExpenseRepository.InsertRange(siafExpense);
        public async Task<DataTablesStructs.ReturnedData<object>> GetDerivedExpensesDatatable(DataTablesStructs.SentParameters sentParameters, string userId)
            => await _siafExpenseRepository.GetDerivedExpensesDatatable(sentParameters, userId);

    }
}
