using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class PerformanceEvaluationQuestion : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid PerformanceEvaluationTemplateId { get; set; }
        public Guid? PerformanceEvaluationCriterionId { get; set; }
        public PerformanceEvaluationCriterion PerformanceEvaluationCriterion { get; set; }
        public PerformanceEvaluationTemplate PerformanceEvaluationTemplate { get; set; }
        public ICollection<PerformanceEvaluationResponse> Responses { get; set; }
    }
}
