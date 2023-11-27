using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.RolAnnouncement;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class RolAnnouncementService : IRolAnnouncementService
    {
        private readonly IRolAnnouncementRepository _rolAnnouncementRepository;

        public RolAnnouncementService(IRolAnnouncementRepository rolAnnouncementRepository)
        {
            _rolAnnouncementRepository = rolAnnouncementRepository;
        }

        public Task DeleteRange(IEnumerable<RolAnnouncement> rolAnnouncements)
            => _rolAnnouncementRepository.DeleteRange(rolAnnouncements);

        public Task InsertRange(IEnumerable<RolAnnouncement> rolAnnouncements)
            => _rolAnnouncementRepository.InsertRange(rolAnnouncements);

        public async Task<List<AnnouncementTemplate>> GetAnnouncementsHome(DateTime dateNow, IList<string> userRoles)
            => await _rolAnnouncementRepository.GetAnnouncementsHome(dateNow, userRoles);
    }
}
