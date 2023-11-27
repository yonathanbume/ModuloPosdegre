using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class NonActivity : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ResolutionId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MinHours { get; set; }
        public decimal MaxHours { get; set; }
        public bool CompleteWholeHours { get; set; }

        public Resolution Resolution { get; set; }
    }
}

