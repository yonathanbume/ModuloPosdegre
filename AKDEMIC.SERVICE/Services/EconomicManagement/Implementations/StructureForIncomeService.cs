using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class StructureForIncomeService : IStructureForIncomeService
    {
        private readonly IStructureForIncomeRepository _structureForIncomeRepository;

        public StructureForIncomeService(IStructureForIncomeRepository structureForIncomeRepository)
        {
            _structureForIncomeRepository = structureForIncomeRepository;
        }

        public async Task InsertStructureForIncome(StructureForIncome structureForIncome) =>
            await _structureForIncomeRepository.Insert(structureForIncome);

        public async Task UpdateStructureForIncome(StructureForIncome structureForIncome) =>
            await _structureForIncomeRepository.Update(structureForIncome);

        public async Task DeleteStructureForIncome(StructureForIncome structureForIncome) =>
            await _structureForIncomeRepository.Delete(structureForIncome);

        public async Task<StructureForIncome> GetStructureForIncomeById(Guid id) =>
            await _structureForIncomeRepository.Get(id);

        public async Task<IEnumerable<StructureForIncome>> GetAllStructureForIncomes() =>
            await _structureForIncomeRepository.GetAll();
        public async Task<int> Count()
            => await _structureForIncomeRepository.Count();
        public async Task<DateTime> GetLastDate()
            => await _structureForIncomeRepository.GetLastDate();
        public async Task<DataTablesStructs.ReturnedData<object>> GetStructureForIncomesExcelImportDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string caseFile, string year, string userId)
            => await _structureForIncomeRepository.GetStructureForIncomesExcelImportDatatable(sentParameters, dependencyId, caseFile, year, userId);
        public async Task InsertRange(List<StructureForIncome> structureForIncome)
            => await _structureForIncomeRepository.InsertRange(structureForIncome);
    }
}
