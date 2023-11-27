using System;

namespace AKDEMIC.ENTITIES.Models.HelpDesk
{
    public class Solution
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CodeForUsers { get; set; }
        public string Description { get; set; }

        //public Guid HardwareId { get; set; }
        //public virtual ComputersManagement.Hardware {}

        //public Guid IncidentId { get; set; }
        //public virtual Incident Incident { get; set; }
    }
}
