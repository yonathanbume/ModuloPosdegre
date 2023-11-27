using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface ICatalogActivityRepository : IRepository<CatalogActivity>
    {
        Task<DateTime> GetLastDate();
        Task<DataTablesStructs.ReturnedData<object>> GetCatalogActivityDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
    }
}
