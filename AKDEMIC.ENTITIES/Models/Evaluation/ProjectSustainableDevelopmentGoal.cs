using System;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class ProjectSustainableDevelopmentGoal
    {
        public Guid Id { get; set; }
        public Guid SustainableDevelopmentGoalId { get; set; }
        public SustainableDevelopmentGoal SustainableDevelopmentGoal { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
