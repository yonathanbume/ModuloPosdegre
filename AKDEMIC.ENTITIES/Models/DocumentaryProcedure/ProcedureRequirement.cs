using AKDEMIC.ENTITIES.Models.Degree;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class ProcedureRequirement
    {
        public Guid Id { get; set; }
        public Guid ProcedureId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Cost { get; set; }
        public byte Type { get; set; }

        public byte? SystemValidationType { get; set; }

        [NotMapped]
        public bool HasUserProcedureRecordRequirement { get; set; }
        [NotMapped]
        public string SystemvalidationTypeStr { get; set; }

        public Procedure Procedure { get; set; }
    }
}
