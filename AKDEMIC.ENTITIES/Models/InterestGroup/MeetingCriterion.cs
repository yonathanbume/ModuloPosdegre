using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.InterestGroup
{
    public class MeetingCriterion
    {
        public Guid Id { get; set; }

        public Guid CriterionId { get; set; }
        public Criterion Criterion { get; set; }

        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; }
    }
}
