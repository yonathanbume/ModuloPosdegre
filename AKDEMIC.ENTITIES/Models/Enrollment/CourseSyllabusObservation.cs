using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CourseSyllabusObservation : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid CourseSyllabusId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
