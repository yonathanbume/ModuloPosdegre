using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class InstitutionalWelfareSection
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int MaxScore { get; set; }
        public Guid InstitutionalWelfareRecordId { get; set; }
        public InstitutionalWelfareRecord InstitutionalWelfareRecord { get; set; }
        public ICollection<InstitutionalWelfareQuestion> InstitutionalWelfareQuestions { get; set; }
        public int Score { get; set; }

    }
}
