using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IUserAnnouncementService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataTable(DataTablesStructs.SentParameters sentParameters, string search);
        Task Insert(UserAnnouncement announcement);
        Task<UserAnnouncement> Get(Guid id);
        Task Update(UserAnnouncement announcement);
        Task<UserAnnouncement> GetActive(string userId);
        Task<bool> AnyInDates(DateTime startDate, DateTime endDate, Guid? id = null);
        Task DeleteById(Guid id);
    }
}
