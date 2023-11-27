using System;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CompetenceEvaluation
    {
        public Guid Id { get; set; }
        public Guid EvaluationTypeId { get; set; }

        public string Name { get; set; }
        public int Percentage { get; set; }

        public EvaluationType EvaluationType { get; set; }
    }
}
