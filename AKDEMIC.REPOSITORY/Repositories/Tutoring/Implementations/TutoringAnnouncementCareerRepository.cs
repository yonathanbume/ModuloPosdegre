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
    public class TutoringAnnouncementCareerRepository : Repository<TutoringAnnouncementCareer>, ITutoringAnnouncementCareerRepository
    {
        public TutoringAnnouncementCareerRepository(AkdemicContext context) : base(context) { }

        public async Task DeleteByTutoringAnnouncementId(Guid tutoringAnnouncementId)
        {
            var announcementCareers = await _context.TutoringAnnouncementCareers
                .Where(x => x.TutoringAnnouncementId == tutoringAnnouncementId)
                .ToListAsync();
            _context.TutoringAnnouncementCareers.RemoveRange(announcementCareers);
            await _context.SaveChangesAsync();
        }
    }
}
