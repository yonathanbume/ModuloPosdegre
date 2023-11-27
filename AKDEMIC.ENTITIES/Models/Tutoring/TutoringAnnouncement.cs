using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringAnnouncement : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public bool AllCareers { get; set; }
        public bool AllRoles { get; set; }
        public DateTime DisplayTime { get; set; }
        public string File { get; set; }
        public DateTime EndTime { get; set; }
        public string Message { get; set; }
        public byte System { get; set; }
        public string Title { get; set; }
        
        public IEnumerable<TutoringAnnouncementCareer> TutoringAnnouncementCareers { get; set; }
        public IEnumerable<TutoringAnnouncementRole> TutoringAnnouncementRoles { get; set; }
    }
}
