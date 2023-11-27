using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Calendar : Entity, ITimestamp
    {
        [Key]
        public Guid CalendarId { get; set; }
        public Guid? SectionId { get; set; }
        public string UserId { get; set; }
        public string ClassName { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
        public string GEventId { get; set; }
        public string HangoutLink { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public ApplicationUser User { get; set; }
        public Section Section { get; set; }
    }
}
