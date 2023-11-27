using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class TutoringAnnouncementRoleRepository : Repository<TutoringAnnouncementRole>, ITutoringAnnouncementRoleRepository
    {
        public TutoringAnnouncementRoleRepository(AkdemicContext context) : base(context) { }

        public async Task DeleteByTutoringAnnouncementId(Guid tutoringAnnouncementId)
        {
            var announcementRoles = await _context.TutoringAnnouncementRoles
                .Where(x => x.TutoringAnnouncementId == tutoringAnnouncementId)
                .ToListAsync();
            _context.TutoringAnnouncementRoles.RemoveRange(announcementRoles);
            await _context.SaveChangesAsync();
        }
    }
}
