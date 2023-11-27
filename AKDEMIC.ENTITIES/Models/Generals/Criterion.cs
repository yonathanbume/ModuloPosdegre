using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class Criterion : ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string UrlFile { get; set; }
        public string RelatedStandars { get; set; }

        public ICollection<MeetingCriterion> MeetingCriterions { get; set; }
    }
}
