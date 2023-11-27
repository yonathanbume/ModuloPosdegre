using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringAnnouncementRoleService : ITutoringAnnouncementRoleService
    {
        private readonly ITutoringAnnouncementRoleRepository _tutoringAnnouncementRoleRepository;

        public TutoringAnnouncementRoleService(ITutoringAnnouncementRoleRepository tutoringAnnouncementRoleRepository)
        {
            _tutoringAnnouncementRoleRepository = tutoringAnnouncementRoleRepository;
        }

        public async Task DeleteByTutoringAnnouncementId(Guid tutoringAnnouncementId)
            => await _tutoringAnnouncementRoleRepository.DeleteByTutoringAnnouncementId(tutoringAnnouncementId);

        public async Task InsertRange(IEnumerable<TutoringAnnouncementRole> tutoringAnnouncementRoles)
            => await _tutoringAnnouncementRoleRepository.InsertRange(tutoringAnnouncementRoles);
    }
}
