using System;
using System.Collections.Generic;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class PerformanceEvaluation
    {
        public Guid Id { get; set; }
        public Term Term { get; set; }
        public byte Target { get; set; } = ConstantHelpers.PERFORMANCE_EVALUATION.TARGET.ALL;
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid TermId { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public bool PercentageRaitingScale { get; set; }
        public ICollection<RelatedPerformanceEvaluationTemplate> RelatedPerformanceEvaluationTemplates { get; set; }
        public ICollection<PerformanceEvaluationUser> PerformanceEvaluationUsers { get; set; }
        public ICollection<PerformanceEvaluationRubric> PerformanceEvaluationRubrics { get; set; }
    }
}
