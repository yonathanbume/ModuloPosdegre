using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IConceptDistributionDetailService
    {
        Task<IEnumerable<ConceptDistributionDetail>> GetAllNonUnitByConceptDistribution(Guid conceptDistributionId);
        void RemoveRange(IEnumerable<ConceptDistributionDetail> conceptDistributionDetails);
    }
}
