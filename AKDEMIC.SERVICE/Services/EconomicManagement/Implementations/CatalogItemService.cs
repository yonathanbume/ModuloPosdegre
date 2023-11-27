using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class CatalogItemService : ICatalogItemService
    {
        private readonly ICatalogItemRepository _catalogItemRepository;

        public CatalogItemService(ICatalogItemRepository catalogItemRepository)
        {
            _catalogItemRepository = catalogItemRepository;
        }
        public async Task InsertRange(List<CatalogItem> catalogItem)
            => await _catalogItemRepository.InsertRange(catalogItem);
        public async Task InsertCatalogItem(CatalogItem catalogItem) =>
            await _catalogItemRepository.Insert(catalogItem);

        public async Task UpdateCatalogItem(CatalogItem catalogItem) =>
            await _catalogItemRepository.Update(catalogItem);

        public async Task DeleteCatalogItem(CatalogItem catalogItem) =>
            await _catalogItemRepository.Delete(catalogItem);

        public async Task<CatalogItem> GetCatalogItemById(Guid id) =>
            await _catalogItemRepository.Get(id);

        public async Task<IEnumerable<CatalogItem>> GetAllCatalogItems() =>
            await _catalogItemRepository.GetAll();
        public async Task<int> Count()
            => await _catalogItemRepository.Count();
        public async Task<DateTime> GetLastDate()
            => await _catalogItemRepository.GetLastDate();
        public async Task<DataTablesStructs.ReturnedData<object>> GetCatalogItemDatatable(DataTablesStructs.SentParameters sentParameters, byte? type = null, string searchCode = null, string searchName = null)
            => await _catalogItemRepository.GetCatalogItemDatatable(sentParameters, type, searchCode, searchName);

        public SelectList GetTypeCatalog()
            => _catalogItemRepository.GetTypeCatalog();
        public async Task<bool> AnyByCode(string code)
            => await _catalogItemRepository.AnyByCode(code);

        public async Task<object> GetCatalogItemsToInternalOuput(Guid dependencyId)
            => await _catalogItemRepository.GetCatalogItemsToInternalOuput(dependencyId);

        public async Task<CatalogItem> Get(Guid id)
            => await _catalogItemRepository.Get(id);
    }
}
