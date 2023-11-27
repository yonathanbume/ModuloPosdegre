using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class RecordConcept
    {
        public Guid Id { get; set; }
        public Guid ConceptId { get; set; }
        public int RecordType { get; set; }
        public Concept Concept { get; set; }
    }
}
