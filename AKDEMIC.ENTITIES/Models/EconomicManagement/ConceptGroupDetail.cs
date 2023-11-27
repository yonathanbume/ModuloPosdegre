using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class ConceptGroupDetail : Entity, ITimestamp
    {
        public Guid ConceptId { get; set; }
        public Guid ConceptGroupId { get; set; }

        public Concept Concept { get; set; }
        public ConceptGroup ConceptGroup { get; set; }
    }
}
