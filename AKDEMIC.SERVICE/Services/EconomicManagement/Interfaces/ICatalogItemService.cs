using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface ICatalogItemService
    {
        Task InsertRange(List<CatalogItem> catalogItem);
        Task InsertCatalogItem(CatalogItem catalogItem);
        Task UpdateCatalogItem(CatalogItem catalogItem);
        Task DeleteCatalogItem(CatalogItem catalogItem);
        Task<CatalogItem> GetCatalogItemById(Guid id);
        Task<IEnumerable<CatalogItem>> GetAllCatalogItems();
        Task<int> Count();
        Task<DateTime> GetLastDate();
        Task<DataTablesStructs.ReturnedData<object>> GetCatalogItemDatatable(DataTablesStructs.SentParameters sentParameters, byte? type = null, string searchCode = null, string searchName = null);
        SelectList GetTypeCatalog();
        Task<bool> AnyByCode(string code);
        Task<object> GetCatalogItemsToInternalOuput(Guid dependencyId);
        Task<CatalogItem> Get(Guid id);
    }
}
