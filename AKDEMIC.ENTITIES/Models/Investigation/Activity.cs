using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class Activity : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ActivityTypeId { get; set; }
        public ActivityType ActivityType { get; set; }
        public string Description { get; set; }
        public string Objectives { get; set; }
        public bool Group { get; set; }
        public Guid SectionId { get; set; }
        public Section Section { get; set; }
        public List<ActivityFile> ActivityFiles { get; set; }
        public List<StudentActivity> StudentActivities { get; set; }
    }
}
