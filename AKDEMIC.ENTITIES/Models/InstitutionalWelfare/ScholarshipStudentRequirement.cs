using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class ScholarshipStudentRequirement
    {
        [Key]
        public Guid ScholarshipStudentId { get; set; }
        [Key]
        public Guid InstitutionalWelfareScholarshipRequirementId { get; set; }
        public InstitutionalWelfareScholarshipRequirement InstitutionalWelfareScholarshipRequirement { get; set; }
        public ScholarshipStudent ScholarshipStudent { get; set; }
        public string FilePath { get; set; }
    }
}
