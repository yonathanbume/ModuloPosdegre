using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface ICatalogItemGoalService
    {
        Task InsertCatalogItemGoal(CatalogItemGoal catalogItemGoal);
        Task UpdateCatalogItemGoal(CatalogItemGoal catalogItemGoal);
        Task DeleteCatalogItemGoal(CatalogItemGoal catalogItemGoal);
        Task<CatalogItemGoal> GetCatalogItemGoalById(Guid id);
        Task<IEnumerable<CatalogItemGoal>> GetAllCatalogItemGoals();
        Task AddCatalogItemGoal(CatalogItemGoal catalogItemGoal);
    }
}
