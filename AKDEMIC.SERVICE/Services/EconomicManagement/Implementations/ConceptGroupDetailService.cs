using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class ConceptGroupDetailService : IConceptGroupDetailService
    {
        private readonly IConceptGroupDetailRepository _conceptGroupDetailRepository;

        public ConceptGroupDetailService(IConceptGroupDetailRepository conceptGroupDetailRepository)
        {
            _conceptGroupDetailRepository = conceptGroupDetailRepository;
        }

        public async Task Delete(ConceptGroupDetail detail)
            => await _conceptGroupDetailRepository.Delete(detail);

        public async Task DeleteRange(IEnumerable<ConceptGroupDetail> details)
            => await _conceptGroupDetailRepository.DeleteRange(details);

        public async Task<ConceptGroupDetail> Get(Guid grouptId, Guid conceptId)
            => await _conceptGroupDetailRepository.Get(grouptId, conceptId);

        public async Task<List<ConceptGroupDetail>> GetAllByGroupId(Guid conceptGroupId)
            => await _conceptGroupDetailRepository.GetAllByGroupId(conceptGroupId);

        public async Task<List<ConceptGroupDetail>> GetAllWithDataByGroupId(Guid conceptGroupId)
            => await _conceptGroupDetailRepository.GetAllWithDataByGroupId(conceptGroupId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetGroupDetailsDatatable(DataTablesStructs.SentParameters sentParameters, Guid groupId)
            => await _conceptGroupDetailRepository.GetGroupDetailsDatatable(sentParameters, groupId);

        public async Task Insert(ConceptGroupDetail detail)
            => await _conceptGroupDetailRepository.Insert(detail);

        public async Task Update(ConceptGroupDetail detail)
            => await _conceptGroupDetailRepository.Update(detail);
    }
}
