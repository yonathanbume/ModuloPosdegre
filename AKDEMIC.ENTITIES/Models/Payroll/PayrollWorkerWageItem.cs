using System;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class PayrollWorkerWageItem
    {
        public Guid Id { get; set; }

        public Guid PayrollWorkerId { get; set; }
        public Guid WageItemId { get; set; }

        public PayrollWorker PayrollWorker { get; set; }
        public WageItem WageItem { get; set; }
    }
}
