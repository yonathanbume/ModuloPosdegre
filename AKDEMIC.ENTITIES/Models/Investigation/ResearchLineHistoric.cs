using System;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class ResearchLineHistoric
    {
        public Guid Id { get; set; }
        public ResearchLine ResearchLine { get; set; }
        public Guid ResearchLineId { get; set; }
        public string Name { get; set; }
        public string Career { get; set; }
        public string ResearchDiscipline { get; set; }
        public string ResearchCategory { get; set; }
        public string ResearchArea { get; set; }
        public string ReseachSubArea { get; set; }
        public bool Active { get; set; }
        public DateTime DateTime { get; set; }
    }
}
