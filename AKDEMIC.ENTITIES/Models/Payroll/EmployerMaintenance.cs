using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class EmployerMaintenance : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public decimal Percentage { get; set; }

        public bool Pdt { get; set; }

        public Guid ConceptTypeId { get; set; }
        public ConceptType ConceptType { get; set; }


    }
}
