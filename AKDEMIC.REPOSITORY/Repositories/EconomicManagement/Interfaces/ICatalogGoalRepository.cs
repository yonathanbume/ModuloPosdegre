using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface ICatalogGoalRepository : IRepository<CatalogGoal>
    {
        Task<DateTime> GetLastDate();
        Task<DataTablesStructs.ReturnedData<object>> GetCatalogGoalDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<object> GetAllSecFunctions();
        Task<object> GetCatalogGoalBySecFunction(string secFunction);
        Task<CatalogGoal> GetCatalogGoalBySecFunctionClass(string secFunction);
    }
}
