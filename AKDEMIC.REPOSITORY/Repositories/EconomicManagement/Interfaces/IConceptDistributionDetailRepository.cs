using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IConceptDistributionDetailRepository:IRepository<ConceptDistributionDetail>
    {
        Task<IEnumerable<ConceptDistributionDetail>> GetAllNonUnitByConceptDistribution(Guid conceptDistributionId);
    }
}
