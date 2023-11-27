using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class CatalogItemGoalService : ICatalogItemGoalService
    {
        private readonly ICatalogItemGoalRepository _catalogItemGoalRepository;

        public CatalogItemGoalService(ICatalogItemGoalRepository catalogItemGoalRepository)
        {
            _catalogItemGoalRepository = catalogItemGoalRepository;
        }

        public async Task InsertCatalogItemGoal(CatalogItemGoal catalogItemGoal) =>
            await _catalogItemGoalRepository.Insert(catalogItemGoal);

        public async Task UpdateCatalogItemGoal(CatalogItemGoal catalogItemGoal) =>
            await _catalogItemGoalRepository.Update(catalogItemGoal);

        public async Task DeleteCatalogItemGoal(CatalogItemGoal catalogItemGoal) =>
            await _catalogItemGoalRepository.Delete(catalogItemGoal);

        public async Task<CatalogItemGoal> GetCatalogItemGoalById(Guid id) =>
            await _catalogItemGoalRepository.Get(id);

        public async Task<IEnumerable<CatalogItemGoal>> GetAllCatalogItemGoals() =>
            await _catalogItemGoalRepository.GetAll();
        public async Task AddCatalogItemGoal(CatalogItemGoal catalogItemGoal)
            => await _catalogItemGoalRepository.Add(catalogItemGoal);
    }
}
