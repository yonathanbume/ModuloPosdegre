using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class ConceptHistory: Entity ,ITimestamp
    {
        public Guid Id { get; set; }

        public decimal OldAmount { get; set; }
        public decimal Amount { get; set; }

        public bool IsDividedAmount { get; set; }

        public string OldConceptDistribution { get; set; }
        public string NewConceptDistribution { get; set; }

        public Guid ConceptId { get; set; }
        public Concept Concept { get; set; }
    }
}
