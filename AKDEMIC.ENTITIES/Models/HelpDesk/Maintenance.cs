using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.HelpDesk
{
    public class Maintenance
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public byte Status { get; set; }
        public string CodeForUsers { get; set; }

        public Guid DependencyId { get; set; }
        public Dependency Dependency { get; set; }

        public string AssignedTechnicianId { get; set; }
        public virtual ApplicationUser AssignedTechnician { get; set; }

        public string UserReportingMaintenanceId { get; set; }
        public virtual ApplicationUser UserReportingMaintenance { get; set; }
    }
}
