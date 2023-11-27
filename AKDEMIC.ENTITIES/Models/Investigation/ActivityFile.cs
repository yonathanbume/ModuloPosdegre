using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class ActivityFile : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public Activity Activity { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
