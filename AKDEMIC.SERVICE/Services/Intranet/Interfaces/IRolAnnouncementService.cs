using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.RolAnnouncement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IRolAnnouncementService
    {
        Task InsertRange(IEnumerable<RolAnnouncement> rolAnnouncements);
        Task DeleteRange(IEnumerable<RolAnnouncement> rolAnnouncements);
        Task<List<AnnouncementTemplate>> GetAnnouncementsHome(DateTime dateNow , IList<string> userRoles);
    }
}
