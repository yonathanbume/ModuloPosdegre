using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class CareerApplicationTerm : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid CareerId { get; set; }
        public Guid ApplicationTermId { get; set; }
        public Guid? CampusId { get; set; }

        public int VacantsTotal { get; set; }

        public ApplicationTerm ApplicationTerm { get; set; }
        public Career Career { get; set; }
        public Campus Campus { get; set; }

        public ICollection<Vacant> Vacants { get; set; }
    }

}
