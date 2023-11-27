using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
   public  class BeginningAnnouncementService : IBeginningAnnouncementService
    {
        private readonly IBeginningAnnouncementRepository _beginningAnnouncementRepository;
        public BeginningAnnouncementService(IBeginningAnnouncementRepository beginningAnnouncementRepository)
        {
            _beginningAnnouncementRepository=beginningAnnouncementRepository;
        }

        public async Task<bool> AnyInDates(DateTime startDate, DateTime endDate,byte appearsIn, Guid? id = null)
        {
            return await _beginningAnnouncementRepository.AnyInDates(startDate, endDate, appearsIn, id);
        }

        public async  Task DeleteById(Guid id)
        {
            await _beginningAnnouncementRepository.DeleteById(id);
        }

        public async Task<BeginningAnnouncement> Get(Guid id)
        {
            return await _beginningAnnouncementRepository.Get(id);
        }

        public async Task<BeginningAnnouncement> GetActive()
        {
            return await _beginningAnnouncementRepository.GetActive();
        }
        public async Task<BeginningAnnouncement> GetActiveForLogin()
        {
            return await _beginningAnnouncementRepository.GetActiveForLogin();
        }

        public async Task<List<BeginningAnnouncement>> GetBeginningAnnouncements(byte system, ClaimsPrincipal user)
            => await _beginningAnnouncementRepository.GetBeginningAnnouncements(system, user);

        public async Task<List<BeginningAnnouncement>> GetBeginningAnnouncementsForLogin(byte system)
            => await _beginningAnnouncementRepository.GetBeginningAnnouncementsForLogin(system);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataTable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            return await _beginningAnnouncementRepository.GetDataTable(sentParameters, search);
        }

        public async Task<List<BeginningAnnouncementRole>> GetRoles(Guid announcementId)
            => await _beginningAnnouncementRepository.GetRoles(announcementId);

        public async Task Insert(BeginningAnnouncement announcement)
        {
            await _beginningAnnouncementRepository.Insert(announcement);
        }

        public async Task RemoveRoles(Guid announcementId)
            => await _beginningAnnouncementRepository.RemoveRoles(announcementId);

        public async Task Update(BeginningAnnouncement announcement)
        {
            await _beginningAnnouncementRepository.Update(announcement);
        }
    }
}
