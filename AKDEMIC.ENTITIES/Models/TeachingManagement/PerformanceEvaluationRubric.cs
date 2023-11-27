using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class PerformanceEvaluationRubric
    {
        public Guid Id { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public string Description { get; set; }
        public string TeacherRaitingDescription { get; set; }
        public Guid PerformanceEvaluationId { get; set; }
        public PerformanceEvaluation PerformanceEvaluation { get; set; }
    }
}
