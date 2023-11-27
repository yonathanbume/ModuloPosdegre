using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class TemporalGrade
    {
        public Guid Id { get; set; }
        public Guid EvaluationId { get; set; }
        public Guid StudentSectionId { get; set; }

        public bool Attended { get; set; } = true;
        public string CreatorIP { get; set; }
        public decimal Value { get; set; } = 0;

        public virtual Enrollment.Evaluation Evaluation { get; set; }
        public StudentSection StudentSection { get; set; }
    }
}
