using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.HelpDesk
{
    public class Incident
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ComputerId { get; set; }
        public Computer Computer { get; set; }
        public string Description { get; set; }
        public string File { get; set; }
        public DateTime IncidentDate { get; set; }
        public DateTime IncidentReportDate { get; set; }
        public DateTime IncidentAsigmDate { get; set; }
        public byte Status { get; set; }
        public string CodeForUsers { get; set; }

        public Guid DependencyId { get; set; }
        public Dependency Dependency { get; set; }

        public string UserReportingIncidentId { get; set; }
        public virtual ApplicationUser UserReportingIncident { get; set; }

        public string AssignedTechnicianId { get; set; }
        public virtual ApplicationUser AssignedTechnician { get; set; }

        public virtual ICollection<IncidentType> IncidentTypes { get; set; }
        public virtual ICollection<IncidentSolution> IncidentSolutions { get; set; }
    }
    public class IncidentType
    {
        public Guid Id { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public Guid IncidentId { get; set; }
        public virtual Incident Incident { get; set; }
    }

}
