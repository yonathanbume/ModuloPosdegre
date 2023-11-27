using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CampusCareer
    {
        public Guid CampusId { get; set; }
        public Guid CareerId { get; set; }
        
        public Campus Campus { get; set; }
        public Career Career { get; set; }
    }
}
