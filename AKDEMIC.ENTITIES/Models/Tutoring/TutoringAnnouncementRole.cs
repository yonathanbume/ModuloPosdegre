using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringAnnouncementRole
    {
        [Required]
        public string RoleId { get; set; }
        public Guid TutoringAnnouncementId { get; set; }
        
        public ApplicationRole Role { get; set; }
        public TutoringAnnouncement TutoringAnnouncement { get; set; }
    }
}
