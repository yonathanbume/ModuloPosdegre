using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class BeginningAnnouncementRole
    {
        [Key]
        public string RoleId { get; set; }
        public ApplicationRole Role { get; set; }
        [Key]
        public Guid BeginningAnnouncementId { get; set; }
        public BeginningAnnouncement BeginningAnnouncement { get; set; }
    }
}
