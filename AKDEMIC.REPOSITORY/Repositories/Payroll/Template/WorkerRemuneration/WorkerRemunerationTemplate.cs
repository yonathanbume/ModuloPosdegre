using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkerRemuneration
{
    public class WorkerRemunerationTemplate
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public Guid ConceptTypeId { get; set; }

        public Guid WageItemId { get; set; } //CONCEPTOS REMUNERATIVOS

        public WorkingTermInfoTemplate StartWorkingTerm { get; set; }

        public WorkingTermInfoTemplate EndWorkingTerm { get; set; }

        public decimal Amount { get; set; }
        public bool IsActive { get; set; }

        public Guid WorkerId { get; set; } 
    }
    public class WorkingTermInfoTemplate
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateText { get; set; }
        public DateTime EndDate { get; set; }
        public string EndDateText { get; set; }
        public string Correlative { get; set; }
        public int Year { get; set; }
        public int MonthNumber { get; set; }
        public int Number { get; set; }
        public bool IsExtraTerm { get; set; } = false; //Es periodo adicional
    }
}
