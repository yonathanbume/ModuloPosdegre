using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class PerformanceEvaluationTemplate
    {
        public Guid Id { get; set; }
        public bool EnabledCriterions { get; set; }
        public byte Max { get; set; }
        public string Name { get; set; }
        public string RoleId { get; set; }
        public ApplicationRole Role { get; set; }
        public bool IsActive { get; set; }
        public string Target { get; set; }
        public string Instructions { get; set; }
        public ICollection<PerformanceEvaluationUser> Users { get; set; }
        public ICollection<PerformanceEvaluationCriterion> Criterions { get; set; }
        public ICollection<PerformanceEvaluationQuestion> Questions { get; set; }
        public ICollection<RelatedPerformanceEvaluationTemplate> RelatedPerformanceEvaluationTemplates { get; set; }
    }
}
