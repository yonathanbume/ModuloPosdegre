using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class AdmissionRequirement
    {
        public Guid Id { get; set; }
        public Guid AdmissionTypeId { get; set; }

        public bool IsOptional { get; set; } = false;

        [Required]
        [StringLength(400)]
        public string Name { get; set; }

        public AdmissionType AdmissionType { get; set; }

        public ICollection<PostulantAdmissionRequirement> PostulantAdmissionRequirements { get; set; }
    }
}