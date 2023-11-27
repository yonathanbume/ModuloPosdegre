using System;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class ResearchSubArea
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ResearchArea ResearchArea { get; set; }
        public Guid ResearchAreaId { get; set; }
    }
}
