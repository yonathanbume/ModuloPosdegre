using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class CatalogActivityService : ICatalogActivityService
    {
        private readonly ICatalogActivityRepository _catalogActivityRepository;

        public CatalogActivityService(ICatalogActivityRepository catalogActivityRepository)
        {
            _catalogActivityRepository = catalogActivityRepository;
        }
        public async Task InsertRange(List<CatalogActivity> catalogActivity)
            => await _catalogActivityRepository.InsertRange(catalogActivity);
        public async Task InsertCatalogActivity(CatalogActivity catalogActivity) =>
            await _catalogActivityRepository.Insert(catalogActivity);

        public async Task UpdateCatalogActivity(CatalogActivity catalogActivity) =>
            await _catalogActivityRepository.Update(catalogActivity);

        public async Task DeleteCatalogActivity(CatalogActivity catalogActivity) =>
            await _catalogActivityRepository.Delete(catalogActivity);

        public async Task<CatalogActivity> GetCatalogActivityById(Guid id) =>
            await _catalogActivityRepository.Get(id);

        public async Task<IEnumerable<CatalogActivity>> GetAllCatalogActivitys() =>
            await _catalogActivityRepository.GetAll();
        public async Task<int> Count()
            => await _catalogActivityRepository.Count();
        public async Task<DateTime> GetLastDate()
            => await _catalogActivityRepository.GetLastDate();
        public async Task<DataTablesStructs.ReturnedData<object>> GetCatalogActivityDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _catalogActivityRepository.GetCatalogActivityDatatable(sentParameters, search);
    }
}
