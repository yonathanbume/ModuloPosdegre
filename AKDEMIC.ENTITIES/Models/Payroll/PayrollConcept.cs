using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class PayrollConcept : Entity,  ITimestamp
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public int Type { get; set; } // AKDEMIC.CORE.Helpers.ConstantHelpers.PAYROLLCONCEPT_TYPE.VALUES
        public string NormDescription { get; set; }
        public string NormDetail { get; set; }
        public bool IsActive { get; set; }
        public ICollection<WageItem> WageItems { get; set; }
        
    }
}
