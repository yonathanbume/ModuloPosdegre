using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.RolAnnouncement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IRolAnnouncementRepository : IRepository<RolAnnouncement>
    {
        Task<List<AnnouncementTemplate>> GetAnnouncementsHome(DateTime dateNow, IList<string> userRoles);
    }
}
