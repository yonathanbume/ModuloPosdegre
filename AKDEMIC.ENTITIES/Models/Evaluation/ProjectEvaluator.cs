using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class ProjectEvaluator
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public string EvaluatorId { get; set; }
        public ApplicationUser Evaluator { get; set; }
    }
}
