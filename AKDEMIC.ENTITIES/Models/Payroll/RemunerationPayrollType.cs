using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class RemunerationPayrollType : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid WageItemId { get; set; }
        public WageItem WageItem { get; set; }
        public decimal Amount { get; set; }
        public Guid PayrollTypeId { get; set; }
        public PayrollType PayrollType { get; set; }
    }
}
