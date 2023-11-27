using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.ContinuingEducation
{
    public class ActivityType : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

    }
}
