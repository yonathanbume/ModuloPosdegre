using AKDEMIC.ENTITIES.Models.Server;
using AKDEMIC.REPOSITORY.Repositories.Server.Interfaces;
using AKDEMIC.SERVICE.Services.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Server.Implementations
{
    public class GeneralLinkRoleService : IGeneralLinkRoleService
    {
        private readonly IGeneralLinkRoleRepository _generalLinkRoleRepository;

        public GeneralLinkRoleService(IGeneralLinkRoleRepository generalLinkRoleRepository)
        {
            _generalLinkRoleRepository = generalLinkRoleRepository;
        }

        public async Task DeleteRange(IEnumerable<GeneralLinkRole> entities)
            => await _generalLinkRoleRepository.DeleteRange(entities);

        public async Task<List<GeneralLinkRole>> GetGeneralLinkRoles(Guid generalLinkId)
            => await _generalLinkRoleRepository.GetGeneralLinkRoles(generalLinkId);

        public async Task InsertRange(IEnumerable<GeneralLinkRole> entities)
            => await _generalLinkRoleRepository.InsertRange(entities);
    }
}
