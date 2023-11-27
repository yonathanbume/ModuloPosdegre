using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class PayrollType : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public ICollection<Payroll> Payrolls { get; set; }
    }
}
