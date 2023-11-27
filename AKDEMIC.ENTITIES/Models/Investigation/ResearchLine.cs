using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class ResearchLine
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Career Career { get; set; }
        public Guid CareerId { get; set; }
        public ResearchDiscipline ResearchDiscipline { get; set; }
        public Guid ResearchDisciplineId { get; set; }
        public ResearchCategory ResearchCategory { get; set; }
        public Guid ResearchCategoryId { get; set; }
        public bool Active { get; set; }
        public ICollection<ResearchLineHistoric> ResearchLineHistorics { get; set; }
    }
}
