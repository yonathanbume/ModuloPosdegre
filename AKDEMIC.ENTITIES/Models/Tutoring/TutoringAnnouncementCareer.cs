using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringAnnouncementCareer
    {
        public Guid CareerId { get; set; }
        public Guid TutoringAnnouncementId { get; set; }
        
        public Career Career { get; set; }
        public TutoringAnnouncement TutoringAnnouncement { get; set; }
    }
}
