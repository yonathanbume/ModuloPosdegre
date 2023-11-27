using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ExtraordinaryEvaluationCommittee : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string TeacherId { get; set; }
        public Guid ExtraordinaryEvaluationId { get; set; }
        public ExtraordinaryEvaluation ExtraordinaryEvaluation { get; set; }
        public Teacher Teacher { get; set; }
    }
}
