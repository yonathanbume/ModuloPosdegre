using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface ICatalogItemRepository : IRepository<CatalogItem>
    {
        Task<DateTime> GetLastDate();
        Task<DataTablesStructs.ReturnedData<object>> GetCatalogItemDatatable(DataTablesStructs.SentParameters sentParameters, byte? type = null, string searchCode = null, string searchName = null);
        SelectList GetTypeCatalog();
        Task<bool> AnyByCode(string code);
        Task<object> GetCatalogItemsToInternalOuput(Guid dependencyId);
    }
}
