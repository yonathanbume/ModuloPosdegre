using System;

namespace AKDEMIC.ENTITIES.Models.HelpDesk
{
    public class IncidentSolution
    {
        public Guid Id { get; set; }
        public DateTime SolutionDate { get; set; }
        public byte Status { get; set; }

        public Guid SolutionId { get; set; }
        public virtual Solution Solution { get; set; }

        public Guid IncidentId { get; set; }
        public virtual Incident Incident { get; set; }
    }
}
