using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class StructureForExpenseService : IStructureForExpenseService
    {
        private readonly IStructureForExpenseRepository _structureForExpenseRepository;

        public StructureForExpenseService(IStructureForExpenseRepository structureForExpenseRepository)
        {
            _structureForExpenseRepository = structureForExpenseRepository;
        }

        public async Task InsertStructureForExpense(StructureForExpense structureForExpense) =>
            await _structureForExpenseRepository.Insert(structureForExpense);

        public async Task UpdateStructureForExpense(StructureForExpense structureForExpense) =>
            await _structureForExpenseRepository.Update(structureForExpense);

        public async Task DeleteStructureForExpense(StructureForExpense structureForExpense) =>
            await _structureForExpenseRepository.Delete(structureForExpense);

        public async Task<StructureForExpense> GetStructureForExpenseById(Guid id) =>
            await _structureForExpenseRepository.Get(id);

        public async Task<IEnumerable<StructureForExpense>> GetAllStructureForExpenses() =>
            await _structureForExpenseRepository.GetAll();
        public async Task<int> Count()
            => await _structureForExpenseRepository.Count();
        public async Task<DateTime> GetLastDate()
            => await _structureForExpenseRepository.GetLastDate();
        public async Task<DataTablesStructs.ReturnedData<object>> GetStructureForExpensesExcelImportDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string caseFile, string year)
            => await _structureForExpenseRepository.GetStructureForExpensesExcelImportDatatable(sentParameters, dependencyId, caseFile, year);
        public async Task InsertRange(List<StructureForExpense> structureForExpense)
            => await _structureForExpenseRepository.InsertRange(structureForExpense);
    }
}
