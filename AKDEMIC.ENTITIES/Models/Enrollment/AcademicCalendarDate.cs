using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class AcademicCalendarDate
    {
        public Guid Id { get; set; }
        public Guid? ProcedureId { get; set; }
        public Guid TermId { get; set; }

        public DateTime? EndDate { get; set; }

        [NotMapped]
        public string EndFormattedDate { get; set; }
        public bool IsRange { get; set; } = false; // please delete

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [NotMapped]
        public string StartFormattedDate { get; set; }

        public Procedure Procedure { get; set; }
        public Term Term { get; set; }
    }
}
