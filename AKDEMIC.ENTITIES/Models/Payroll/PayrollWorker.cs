using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class PayrollWorker
    {
        public Guid Id { get; set; }

        public Guid PayrollId { get; set; }
        public Guid WorkerId { get; set; }
        
        public Worker Worker { get; set; }
        public Payroll Payroll { get; set; }

        public ICollection<PayrollWorkerWageItem> PayrollWorkerWageItems { get; set; }

        [NotMapped]
        public int PayrollWorkerWageItemsCount { get; set; }
    }
}
