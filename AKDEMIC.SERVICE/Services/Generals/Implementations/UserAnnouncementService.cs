using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
   public  class UserAnnouncementService : IUserAnnouncementService
    {
        private readonly IUserAnnouncementRepository _userAnnouncementRepository;
        public UserAnnouncementService(IUserAnnouncementRepository UserAnnouncementRepository)
        {
            _userAnnouncementRepository=UserAnnouncementRepository;
        }

        public async Task<bool> AnyInDates(DateTime startDate, DateTime endDate, Guid? id = null)
        {
            return await _userAnnouncementRepository.AnyInDates(startDate, endDate, id);
        }

        public async  Task DeleteById(Guid id)
        {
            await _userAnnouncementRepository.DeleteById(id);
        }

        public async Task<UserAnnouncement> Get(Guid id)
        {
            return await _userAnnouncementRepository.Get(id);
        }

        public async Task<UserAnnouncement> GetActive(string userId)
        {
            return await _userAnnouncementRepository.GetActive(userId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataTable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            return await _userAnnouncementRepository.GetDataTable(sentParameters, search);
        }

        public async Task Insert(UserAnnouncement announcement)
        {
            await _userAnnouncementRepository.Insert(announcement);
        }

        public async Task Update(UserAnnouncement announcement)
        {
            await _userAnnouncementRepository.Update(announcement);
        }
    }
}
