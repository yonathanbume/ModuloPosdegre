using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public Task Delete(Announcement announcement)
            => _announcementRepository.Delete(announcement);

        public Task DeleteById(Guid id)
            => _announcementRepository.DeleteById(id);

        public Task<Announcement> Get(Guid id)
            => _announcementRepository.Get(id);

        public Task<IEnumerable<Announcement>> GetAll()
            => _announcementRepository.GetAll();

        public Task Insert(Announcement announcement)
            => _announcementRepository.Insert(announcement);

        public Task Update(Announcement announcement)
            => _announcementRepository.Update(announcement);

        public async Task<object> GetHomeAnnouncement()
            => await _announcementRepository.GetHomeAnnouncement();
    }
}
