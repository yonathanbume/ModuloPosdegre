using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringPlanHistory : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid TutoringPlanId { get; set; }
        public string Url { get; set; }
        public TutoringPlan TutoringPlan { get; set; }
    }
}
