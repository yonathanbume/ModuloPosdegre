using System;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class ProjectAdvanceHistoric
    {
        public Guid Id { get; set; }
        public Guid ProjectAdvanceId { get; set; }
        public ProjectAdvance ProjectAdvance { get; set; }
        public DateTime Update { get; set; }
        public string File { get; set; }
        public string Observations { get; set; }
    }
}
