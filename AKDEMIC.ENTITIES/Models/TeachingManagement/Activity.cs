using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class Activity : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ResolutionId { get; set; }

        public string Description { get; set; }
        public decimal MaxHours { get; set; }
        public decimal MinHours { get; set; }
        public string Name { get; set; }

        public Resolution Resolution { get; set; }
    }
}
