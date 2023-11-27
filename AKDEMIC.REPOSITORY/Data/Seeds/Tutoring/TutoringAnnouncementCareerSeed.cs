using AKDEMIC.ENTITIES.Models.Tutoring;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Tutoring
{
    public class TutoringAnnouncementCareerSeed
    {
        public static TutoringAnnouncementCareer[] Seed(AkdemicContext context)
        {
            var careers = context.Careers.ToList();
            var announcements = context.TutoringAnnouncements.ToList();
            var result = new List<TutoringAnnouncementCareer>()
            {
                new TutoringAnnouncementCareer { CareerId = careers.First().Id, TutoringAnnouncementId = announcements.First().Id }
            };
            return result.ToArray();
        }
    }
}
