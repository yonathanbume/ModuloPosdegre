using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class InstitutionalWelfareAnswerByStudent
    {
        public Guid Id { get; set; }
        public Guid InstitutionalWelfareQuestionId { get; set; }
        public Guid? InstitutionalWelfareAnswerId { get; set; }
        public Guid TermId { get; set; }
        public Guid StudentId { get; set; }
        public string AnswerDescription { get; set; }
        public InstitutionalWelfareAnswer InstitutionalWelfareAnswer { get; set; }
        public InstitutionalWelfareQuestion InstitutionalWelfareQuestion { get; set; }
        public Student Student { get; set; }
        public Term Term { get; set; }
        public int Score { get; set; }
    }
}
