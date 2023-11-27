using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class CatalogItemGoalRepository : Repository<CatalogItemGoal>, ICatalogItemGoalRepository
    {
        public CatalogItemGoalRepository(AkdemicContext context) : base(context) { }
    }
}
