using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class InstitutionalWelfareRecord
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Guid CategorizationLevelHeaderId { get; set; }      
        public CategorizationLevelHeader CategorizationLevelHeader { get; set; }
        public ICollection<InstitutionalWelfareSection> InstitutionalWelfareSections { get; set; }
        public ICollection<InstitutionalRecordCategorizationByStudent> InstitutionalRecordCategorizationByStudents { get; set; }

    }
}
