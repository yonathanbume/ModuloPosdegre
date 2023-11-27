using System;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class PayrollClassWageItemFormula
    {
        public Guid Id { get; set; }

        public Guid WageItemId { get; set; }

        public WageItem WageItem { get; set; }

        public Guid PayrollClassId { get; set; }

        public PayrollClass PayrollClass { get; set; }

        public string Formula { get; set; }

        public string Condition { get; set; }
    }
}
