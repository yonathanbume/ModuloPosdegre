using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class PerformanceEvaluationCriterion : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid PerformanceEvaluationTemplateId { get; set; }
        public PerformanceEvaluationTemplate PerformanceEvaluationTemplate { get; set; }
        public ICollection<PerformanceEvaluationQuestion> PerformanceEvaluationQuestions { get; set; }
    }
}
