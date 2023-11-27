using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class ConceptGroupService : IConceptGroupService
    {
        private readonly IConceptGroupRepository _conceptGroupRepository;
        public ConceptGroupService(IConceptGroupRepository conceptGroupRepository)
        {
            _conceptGroupRepository = conceptGroupRepository;
        }

        public async Task DeleteById(Guid id)
            => await _conceptGroupRepository.DeleteById(id);

        public async Task<ConceptGroup> Get(Guid id)
            => await _conceptGroupRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptGroupsDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string search = null)
            => await _conceptGroupRepository.GetConceptGroupsDatatable(sentParameters, user, search);

        public async Task Insert(ConceptGroup group)
            => await _conceptGroupRepository.Insert(group);

        public async Task Update(ConceptGroup group)
            => await _conceptGroupRepository.Update(group);
    }
}
