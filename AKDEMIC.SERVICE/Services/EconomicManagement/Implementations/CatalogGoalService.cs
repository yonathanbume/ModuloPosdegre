using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class CatalogGoalService : ICatalogGoalService
    {
        private readonly ICatalogGoalRepository _catalogGoalRepository;

        public CatalogGoalService(ICatalogGoalRepository catalogGoalRepository)
        {
            _catalogGoalRepository = catalogGoalRepository;
        }
        public async Task InsertRange(List<CatalogGoal> catalogGoal)
            => await _catalogGoalRepository.InsertRange(catalogGoal);
        public async Task InsertCatalogGoal(CatalogGoal catalogGoal) =>
            await _catalogGoalRepository.Insert(catalogGoal);

        public async Task UpdateCatalogGoal(CatalogGoal catalogGoal) =>
            await _catalogGoalRepository.Update(catalogGoal);

        public async Task DeleteCatalogGoal(CatalogGoal catalogGoal) =>
            await _catalogGoalRepository.Delete(catalogGoal);

        public async Task<CatalogGoal> GetCatalogGoalById(Guid id) =>
            await _catalogGoalRepository.Get(id);

        public async Task<IEnumerable<CatalogGoal>> GetAllCatalogGoals() =>
            await _catalogGoalRepository.GetAll();
        public async Task<int> Count()
            => await _catalogGoalRepository.Count();
        public async Task<DateTime> GetLastDate()
            => await _catalogGoalRepository.GetLastDate();
        public async Task<DataTablesStructs.ReturnedData<object>> GetCatalogGoalDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _catalogGoalRepository.GetCatalogGoalDatatable(sentParameters, search);
        public async Task<object> GetAllSecFunctions()
            => await _catalogGoalRepository.GetAllSecFunctions();
        public async Task<object> GetCatalogGoalBySecFunction(string secFunction)
            => await _catalogGoalRepository.GetCatalogGoalBySecFunction(secFunction);
        public async Task<CatalogGoal> GetCatalogGoalBySecFunctionClass(string secFunction)
            => await _catalogGoalRepository.GetCatalogGoalBySecFunctionClass(secFunction);
    }
}
