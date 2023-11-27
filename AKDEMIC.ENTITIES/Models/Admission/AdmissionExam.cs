using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class AdmissionExam
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public decimal MinimumScore { get; set; }

        public int Weight { get; set; }

        public bool IsPrincipal { get; set; } = false;
        public DateTime DateEvaluation { get; set; }

        public ICollection<AdmissionExamApplicationTerm> AdmissionExamApplicationTerms { get; set; }
        public ICollection<AdmissionExamClassroom> AdmissionExamClassrooms { get; set; }
        public ICollection<AdmissionExamChannel> AdmissionExamChannels { get; set; }
    }
}
