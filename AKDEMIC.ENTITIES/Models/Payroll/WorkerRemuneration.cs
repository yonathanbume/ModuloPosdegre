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
    public class WorkerRemuneration : Entity, ITimestamp // Plantilla de remuneraciones por trabajador
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public Guid ConceptTypeId { get; set; }

        public ConceptType ConceptType { get; set; }

        public Guid WageItemId { get; set; } //CONCEPTOS REMUNERATIVOS

        public WageItem WageItem { get; set; }

        public Guid StartWorkingTermId { get; set; } //PERIODOS
        [InverseProperty("WorkerRemunerationStartWorkingTerms")]
        public WorkingTerm StartWorkingTerm { get; set; }

        public Guid EndWorkingTermId { get; set; } //PERIODOS
        [InverseProperty("WorkerRemunerationEndWorkingTerms")]
        public WorkingTerm EndWorkingTerm { get; set; }

        public decimal Amount { get; set; }
        public bool IsActive { get; set; }

        public Guid WorkerId { get; set; } //TRABAJADOR

        public Worker Worker { get; set; }
    }
}
