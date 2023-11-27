using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class InstitutionalWelfareQuestion
    {
        public Guid Id { get; set; }

        public Guid InstitutionalWelfareSectionId { get; set; }
        public InstitutionalWelfareSection InstitutionalWelfareSection { get; set; }

        [Required]
        public int Type { get; set; }
        public byte DescriptionType { get; set; }

        [Required]
        [StringLength(500)]
        public String Description { get; set; }
        public ICollection<InstitutionalWelfareAnswer> InstitutionalWelfareAnswers { get; set; }
        public ICollection<InstitutionalWelfareAnswerByStudent> InstitutionalWelfareAnswerByStudents { get; set; }
        public int Score { get; set; }
    }
}
