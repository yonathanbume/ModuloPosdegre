using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IBeginningAnnouncementService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataTable(DataTablesStructs.SentParameters sentParameters, string search);
        Task Insert(BeginningAnnouncement announcement);
        Task<BeginningAnnouncement> Get(Guid id);
        Task Update(BeginningAnnouncement announcement);
        Task<BeginningAnnouncement> GetActive();
        Task<BeginningAnnouncement> GetActiveForLogin();
        
        Task<bool> AnyInDates(DateTime startDate, DateTime endDate,byte appearsIn, Guid? id = null);
        Task DeleteById(Guid id);
        Task<List<BeginningAnnouncementRole>> GetRoles(Guid announcementId);
        Task RemoveRoles(Guid announcementId);
        Task<List<BeginningAnnouncement>> GetBeginningAnnouncements(byte system, ClaimsPrincipal user);
        Task<List<BeginningAnnouncement>> GetBeginningAnnouncementsForLogin(byte system);
    }
}
