using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class ActivityType : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
