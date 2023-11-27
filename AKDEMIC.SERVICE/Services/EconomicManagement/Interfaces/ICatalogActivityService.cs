using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface ICatalogActivityService
    {
        Task InsertRange(List<CatalogActivity> catalogActivity);
        Task InsertCatalogActivity(CatalogActivity catalogActivity);
        Task UpdateCatalogActivity(CatalogActivity catalogActivity);
        Task DeleteCatalogActivity(CatalogActivity catalogActivity);
        Task<CatalogActivity> GetCatalogActivityById(Guid id);
        Task<IEnumerable<CatalogActivity>> GetAllCatalogActivitys();
        Task<int> Count();
        Task<DateTime> GetLastDate();
        Task<DataTablesStructs.ReturnedData<object>> GetCatalogActivityDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
    }
}
