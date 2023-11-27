using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template
{
    public class StudentTextAnswerTemplate
    {
        public Guid Id { get; set; }
        public string AnswerResponse { get; set; }
        public string Question { get; set; }
        public int MaxScore { get; set; }
        public bool WasEvaluated { get; set; }
    }

}
