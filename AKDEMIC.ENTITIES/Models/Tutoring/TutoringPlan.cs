using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringPlan : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid CareerId { get; set; }

        public Career Career { get; set; }

        public string Url { get; set; }

        public string TutoringCoordinatorId { get; set; }

        public TutoringCoordinator TutoringCoordinator { get; set; }
        public ICollection<TutoringPlanHistory> TutoringPlanHistories { get; set; }
    }
}
