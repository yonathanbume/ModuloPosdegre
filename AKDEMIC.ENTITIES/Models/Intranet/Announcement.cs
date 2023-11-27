using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class Announcement 
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public string Pathfile { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

        public ICollection<RolAnnouncement> RolAnnouncements { get; set; }
    }
}
