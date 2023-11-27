using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class WorkingTerm : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public int MonthNumber { get; set; }
        public int Number { get; set; }
        public bool Processed { get; set; } = false;
        public int Status { get; set; } // AKDEMIC.CORE.Helpers.ConstantHelpers.WORKINGTERM_STATUS.INACTIVE
        public bool IsExtraTerm { get; set; } = false; //Es periodo adicional

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public ICollection<WorkerRemuneration> WorkerRemunerationStartWorkingTerms { get; set; }

        public ICollection<WorkerRemuneration> WorkerRemunerationEndWorkingTerms { get; set; }
    }
}
