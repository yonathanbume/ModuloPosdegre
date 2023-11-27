using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class VirtualClassStudent : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public Guid VirtualClassId { get; set; }
        public VirtualClass VirtualClass { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
    }
}
