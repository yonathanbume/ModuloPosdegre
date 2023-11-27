using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.ConceptDistribution;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class ConceptDistributionService: IConceptDistributionService
    {
        private readonly IConceptDistributionRepository _conceptDistributionRepository;

        public ConceptDistributionService(IConceptDistributionRepository conceptDistributionRepository)
        {
            _conceptDistributionRepository = conceptDistributionRepository;
        }

        public async Task DeleteById(Guid id)
            => await _conceptDistributionRepository.DeleteById(id);

        public async Task<ConceptDistribution> Get(Guid id)
            => await _conceptDistributionRepository.Get(id);

        public async Task<IEnumerable<ConceptDistribution>> GetAll()
            => await _conceptDistributionRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptDistributionDatatable(DataTablesStructs.SentParameters sentParameters)
            => await _conceptDistributionRepository.GetConceptDistributionDatatable(sentParameters);

        public async Task<List<ConceptDistributionReportTemplate>> GetConceptDistributionExcel(Guid id)
            => await _conceptDistributionRepository.GetConceptDistributionExcel(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptDistributionReportDatatable(DataTablesStructs.SentParameters sentParameters)
            => await _conceptDistributionRepository.GetConceptDistributionReportDatatable(sentParameters);

        public async Task<ConceptDistribution> GetWithIncludes(Guid id)
            => await _conceptDistributionRepository.GetWithIncludes(id);

        public async Task<bool> HasPayments(Guid id)
            => await _conceptDistributionRepository.HasPayments(id);

        public async Task Insert(ConceptDistribution conceptDistribution)
            => await _conceptDistributionRepository.Insert(conceptDistribution);

        public async Task Update(ConceptDistribution conceptDistribution)
            => await _conceptDistributionRepository.Update(conceptDistribution);
    }
}
