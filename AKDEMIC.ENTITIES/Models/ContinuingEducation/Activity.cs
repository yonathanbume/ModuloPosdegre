using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.ContinuingEducation
{
    public class Activity : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ActivityTypeId { get; set; }
        public ActivityType ActivityType { get; set; }
        public bool IsPrivate { get; set; }
        public string Image { get; set; }
        public DateTime DateTime { get; set; }
        public string Location { get; set; }
        public string To { get; set; }
        public decimal Price { get; set; }
        public string Objective { get; set; }
        public string Strategy { get; set; }
        public string Activities { get; set; }
        public string Competencies { get; set; }

    }
}
