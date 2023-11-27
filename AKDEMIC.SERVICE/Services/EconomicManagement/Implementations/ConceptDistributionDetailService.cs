using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class ConceptDistributionDetailService: IConceptDistributionDetailService
    {
        private readonly IConceptDistributionDetailRepository _conceptDistributionDetailRepository;

        public ConceptDistributionDetailService(IConceptDistributionDetailRepository conceptDistributionDetailRepository)
        {
            _conceptDistributionDetailRepository = conceptDistributionDetailRepository;
        }

        public async Task<IEnumerable<ConceptDistributionDetail>> GetAllNonUnitByConceptDistribution(Guid conceptDistributionId)
            => await _conceptDistributionDetailRepository.GetAllNonUnitByConceptDistribution(conceptDistributionId);

        public void RemoveRange(IEnumerable<ConceptDistributionDetail> conceptDistributionDetails)
            => _conceptDistributionDetailRepository.RemoveRange(conceptDistributionDetails);
    }
}
