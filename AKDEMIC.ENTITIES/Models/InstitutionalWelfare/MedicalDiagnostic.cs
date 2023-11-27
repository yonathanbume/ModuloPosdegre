using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class MedicalDiagnostic
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
