using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template
{
    public class RecordUserReportTemplate
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public List<RecordAnswerReportTemplate> AnswersQuestions { get; set; }
    }
    public class RecordAnswerReportTemplate
    {
        public Guid QuestionId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    public class RecordQuestionExcelTemplate
    {
        public Guid QuestionId { get; set; }
        public int Type { get; set; }
        public string Question { get; set; }
    }
}
