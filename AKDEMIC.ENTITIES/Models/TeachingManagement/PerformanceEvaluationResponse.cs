using System;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class PerformanceEvaluationResponse
    {
        public Guid Id { get; set; }
        public PerformanceEvaluationQuestion PerformanceEvaluationQuestion { get; set; }
        public Guid PerformanceEvaluationQuestionId { get; set; }
        public byte Value { get; set; }
        public PerformanceEvaluationUser PerformanceEvaluationUser { get; set; }
        public Guid PerformanceEvaluationUserId { get; set; }
    }
}
