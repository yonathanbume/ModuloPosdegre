using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class AnnouncementRepository : Repository<Announcement>, IAnnouncementRepository
    {
        public AnnouncementRepository(AkdemicContext context) : base(context)
        {
        }

        public override Task<Announcement> Get(Guid id)
            => _context.Announcements
                .Include(x => x.RolAnnouncements)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

        public async Task<object> GetHomeAnnouncement()
        {
            await Task.Delay(10);
            var announcements = _context.Announcements
                .Select(x => new
                {
                    title = x.Title,
                    description = x.Description,
                    date = x.RegisterDate
                }).OrderByDescending(x => x.date).Take(3);

            return announcements;
        }
    }
}
