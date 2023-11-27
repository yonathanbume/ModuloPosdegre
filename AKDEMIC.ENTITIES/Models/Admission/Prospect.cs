using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class Prospect
    {
        public Guid Id { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Status { get; set; }
        public Guid ApplicationTermId { get; set; }
        public Guid? ConceptId { get; set; }
        public Concept Concept { get; set; }
        public string File { get; set; }
        public ApplicationTerm ApplicationTerm { get; set; }
    }
}
