using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class SupervisorCareer : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid? CareerId { get; set; }
        public ApplicationUser User { get; set; }
        public Career Career { get; set; }
    }
}
