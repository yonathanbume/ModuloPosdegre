using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Tutoring
{
    public class TutoringAnnouncementSeed
    {
        public static TutoringAnnouncement[] Seed(AkdemicContext context)
        {
            var result = new List<TutoringAnnouncement>()
            {
                new TutoringAnnouncement { Title = "Bienvenidos al Sistema de Gestión Integrado de Tutorías", Message = "Para todos los coordinadores de tutorías, tutores y tutorados, les deseo una cordial bienvenida.", DisplayTime = DateTime.UtcNow }
            };
            return result.ToArray();
        }
    }
}
