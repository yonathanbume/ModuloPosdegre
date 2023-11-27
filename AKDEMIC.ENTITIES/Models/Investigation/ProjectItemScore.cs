using System;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class ProjectItemScore
    {
        public Guid Id { get; set; }
        public decimal Score { get; set; }
        public Guid ProjectRubricItemId { get; set; }
        public ProjectRubricItem ProjectRubricItem { get; set; }
        public Guid ProjectAdvanceId { get; set; }
        public ProjectAdvance ProjectAdvance { get; set; }
    }
}