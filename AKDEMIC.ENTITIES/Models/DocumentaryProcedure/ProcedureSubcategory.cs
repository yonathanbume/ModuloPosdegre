using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class ProcedureSubcategory
    {
        public Guid Id { get; set; }
        public Guid ProcedureCategoryId { get; set; }

        [Required]
        public string Name { get; set; }
        public int? StaticType { get; set; }

        public ProcedureCategory ProcedureCategory { get; set; }
    }
}
