using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IBeginningAnnouncementRepository : IRepository<BeginningAnnouncement>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataTable(DataTablesStructs.SentParameters sentParameters, string search);
        Task<BeginningAnnouncement> GetActive();
        Task<BeginningAnnouncement> GetActiveForLogin();
        Task<bool> AnyInDates(DateTime startDate, DateTime endDate,byte appearsIn, Guid? id);
        Task<List<BeginningAnnouncementRole>> GetRoles(Guid announcementId);
        Task RemoveRoles(Guid announcementId);
        Task<List<BeginningAnnouncement>> GetBeginningAnnouncements(byte system, ClaimsPrincipal user);
        Task<List<BeginningAnnouncement>> GetBeginningAnnouncementsForLogin(byte system);
    }
}
