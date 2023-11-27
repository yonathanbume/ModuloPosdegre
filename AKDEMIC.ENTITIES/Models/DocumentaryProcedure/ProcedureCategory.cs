using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class ProcedureCategory
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public int? StaticType { get; set; }

        public ICollection<ProcedureSubcategory> ProcedureSubCategories { get; set; }
    }
}
