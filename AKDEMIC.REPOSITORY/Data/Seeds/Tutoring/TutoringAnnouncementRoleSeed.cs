using AKDEMIC.ENTITIES.Models.Tutoring;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Tutoring
{
    public class TutoringAnnouncementRoleSeed
    {
        public static TutoringAnnouncementRole[] Seed(AkdemicContext context)
        {
            var roles = context.Roles.ToList();
            var announcements = context.TutoringAnnouncements.ToList();
            var result = new List<TutoringAnnouncementRole>()
            {
                new TutoringAnnouncementRole { RoleId = roles.First(r => r.Name == "Coordinador de Tutorias").Id, TutoringAnnouncementId = announcements.First().Id },
                new TutoringAnnouncementRole { RoleId = roles.First(r => r.Name == "Docentes").Id, TutoringAnnouncementId = announcements.First().Id },
                new TutoringAnnouncementRole { RoleId = roles.First(r => r.Name == "Alumnos").Id, TutoringAnnouncementId = announcements.First().Id }
            };
            return result.ToArray();
        }
    }
}
