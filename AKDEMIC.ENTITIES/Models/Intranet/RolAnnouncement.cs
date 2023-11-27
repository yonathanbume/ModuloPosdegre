using System;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class RolAnnouncement
    { 
        public Guid Id { get; set; }    
        public Guid AnnouncementId { get; set; }
        public string RolId { get; set; }
        public ApplicationRole Rol { get; set; }
        public Announcement Announcement { get; set; }

    }
}
