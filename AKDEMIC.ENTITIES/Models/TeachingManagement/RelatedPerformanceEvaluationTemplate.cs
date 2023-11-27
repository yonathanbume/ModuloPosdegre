using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class RelatedPerformanceEvaluationTemplate
    {
        [Key]
        public Guid PerformanceEvaluationTemplateId { get; set; }
        [Key]
        public Guid PerformanceEvaluationId { get; set; }
        public PerformanceEvaluation PerformanceEvaluation { get; set; }
        public PerformanceEvaluationTemplate PerformanceEvaluationTemplate { get; set; }
    }
}
