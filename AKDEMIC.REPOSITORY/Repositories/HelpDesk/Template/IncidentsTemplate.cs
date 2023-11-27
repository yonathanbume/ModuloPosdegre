using System;

namespace AKDEMIC.REPOSITORY.Repositories.HelpDesk.Template
{
    public class IncidentTemplate
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid ComputerId { get; set; }
        public string Description { get; set; }
        public byte StatusId { get; set; }
        public string UserReporting { get; set; }
        public string AssignedTechnician { get; set; }
        public string Date { get; set; }
        public string Dependency { get; set; }
        public int Solutions { get; set; }
    }
    public class MaintenanceTemplate
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte StatusId { get; set; }
        public string UserReporting { get; set; }
        public string AssignedTechnician { get; set; }
        public string Date { get; set; }
        public string Dependency { get; set; }
    }
}
