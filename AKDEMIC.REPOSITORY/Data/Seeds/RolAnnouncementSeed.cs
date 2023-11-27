using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Intranet;


namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class RolAnnouncementSeed
    {
        public static RolAnnouncement[] Seed(AkdemicContext context)
        {
            var rol_SUPERADMIN = context.Roles.Where(x => x.NormalizedName == "SUPERADMIN").ToList(); 
            var announcements = context.Announcements.ToList();

            var result = new List<RolAnnouncement>()
            {
                new RolAnnouncement {AnnouncementId = announcements[0].Id , RolId = rol_SUPERADMIN[0].Id }
            };

            return result.ToArray();


        }
    }
}
