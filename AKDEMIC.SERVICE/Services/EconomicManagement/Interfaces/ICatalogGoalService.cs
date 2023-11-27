using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface ICatalogGoalService
    {
        Task InsertRange(List<CatalogGoal> catalogGoal);
        Task InsertCatalogGoal(CatalogGoal catalogGoal);
        Task UpdateCatalogGoal(CatalogGoal catalogGoal);
        Task DeleteCatalogGoal(CatalogGoal catalogGoal);
        Task<CatalogGoal> GetCatalogGoalById(Guid id);
        Task<IEnumerable<CatalogGoal>> GetAllCatalogGoals();
        Task<int> Count();
        Task<DateTime> GetLastDate();
        Task<DataTablesStructs.ReturnedData<object>> GetCatalogGoalDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<object> GetAllSecFunctions();
        Task<object> GetCatalogGoalBySecFunction(string secFunction);
        Task<CatalogGoal> GetCatalogGoalBySecFunctionClass(string secFunction);
    }
}
