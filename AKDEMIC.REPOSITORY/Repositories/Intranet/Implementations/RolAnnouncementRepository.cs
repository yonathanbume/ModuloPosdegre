using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.RolAnnouncement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class RolAnnouncementRepository : Repository<RolAnnouncement>, IRolAnnouncementRepository
    {
        public RolAnnouncementRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<AnnouncementTemplate>> GetAnnouncementsHome(DateTime dateNow, IList<string> userRoles)
        {
            var result = await _context.RolAnnouncements
            .Include(x => x.Announcement)
            .Where(x => userRoles.Contains(x.Rol.NormalizedName) && x.Announcement.StartDate.Date <= dateNow && dateNow <= x.Announcement.EndDate.Date)
            .Select(x => new AnnouncementTemplate
            {
                Id = x.Announcement.Id,
                Title = x.Announcement.Title,
                Description = x.Announcement.Description,
                Pathfile = x.Announcement.Pathfile
            }).ToListAsync();

            return result;
        }
    }
}
