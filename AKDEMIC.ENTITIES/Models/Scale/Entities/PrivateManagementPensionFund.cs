using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Payroll;
using System;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class PrivateManagementPensionFund : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public decimal Contribution { get; set; } //Aporte
        public decimal Insurance { get; set; } //Seguro
        public decimal Commission { get; set; } // Comision
        public decimal InsurableAmount { get; set; } //Monto Asegurable

        public Guid? ConceptTypeId { get; set; }
        public ConceptType ConceptType { get; set; }
    }
}
