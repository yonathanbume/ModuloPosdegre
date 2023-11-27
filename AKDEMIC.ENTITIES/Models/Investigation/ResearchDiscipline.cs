using System;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class ResearchDiscipline
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public ResearchSubArea ResearchSubArea { get; set; }
        public Guid ResearchSubAreaId { get; set; }
    }
}
