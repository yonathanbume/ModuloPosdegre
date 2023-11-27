using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class ProjectAdvance
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public bool IsFinal { get; set; }
        public decimal Qualification { get; set; }
        public ICollection<ProjectAdvanceHistoric> ProjectAdvanceHistorics { get; set; }
        public ICollection<ProjectItemScore> ProjectItemScores { get; set; }
    }
}
