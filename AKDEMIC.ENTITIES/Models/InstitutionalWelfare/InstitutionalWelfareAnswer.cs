using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class InstitutionalWelfareAnswer
    {
        public Guid Id { get; set; }

        public Guid InstitutionalWelfareQuestionId { get; set; }

        [Required]
        [StringLength(400)]
        public string Description { get; set; }

        public int Score { get; set; }

        public InstitutionalWelfareQuestion InstitutionalWelfareQuestion { get; set; }
        public ICollection<InstitutionalWelfareAnswerByStudent> InstitutionalWelfareAnswerByStudents { get; set; }

    }
}
