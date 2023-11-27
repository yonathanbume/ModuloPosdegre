using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class RemunerationMaintenance : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string RemunerationCode { get; set; }

        public string RemunerationDescription { get; set; }

        public decimal Import { get; set; }

        public bool Pdt { get; set; }

        public Guid? ServerTypeId { get; set; }
        public AdministrativeTable ServerType { get; set; }

        public Guid? ConceptTypeId { get; set; }
        public ConceptType ConceptType { get; set; }

        public string ConceptCode { get; set; }

    }
}
