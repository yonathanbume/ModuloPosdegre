using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class WageItem : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public byte VariableType { get; set; }

        public Guid ConceptTypeId { get; set; }
        public ConceptType ConceptType { get; set; }

        public decimal? Amount { get; set; }
        public Guid? PayrollConceptId { get; set; }
        public PayrollConcept PayrollConcept { get; set; }

        public ICollection<PayrollClassWageItemFormula> PayrollWageItemFormulas { get; set; }

        public ICollection<PayrollWorkerWageItem> PayrollWorkerWageItems { get; set; }
    }
}
