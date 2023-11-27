using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class BeginningAnnouncement: Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string YouTubeUrl { get; set; }
        public string ImageUrl { get; set; }
        public string FileUrl { get; set; }
        public byte Status { get; set; }
        public byte AppearsIn { get; set; }
        public byte Type { get; set; }
        public byte System { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<BeginningAnnouncementRole> BeginningAnnouncementRoles { get; set; }
    }
}
