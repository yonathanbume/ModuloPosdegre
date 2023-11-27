using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class AdministrativeTable : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Type { get; set; } // AKDEMIC.CORE.Helpers.ConstantHelpers.ADMINISTRATIVETABLE_TYPE

        public ICollection<Worker> WorkersServerType { get; set; }
        public ICollection<Worker> WorkersSituation { get; set; }
        
        public ICollection<RemunerationMaintenance> RemunerationMaintenancesServerType { get; set; }

    }
}
