using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey
{
    public class SurveyUserReportTemplate
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Dni { get; set; }
        public string Role { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public int CurrentAcademicYear { get; set; }
        public List<SurveyAnswerReportTemplate> AnswersQuestions { get; set; }
    }
    public class SurveyAnswerReportTemplate
    {
        public Guid QuestionId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    public class QuestionExcelTemplate
    {
        public Guid QuestionId { get; set; }
        public int Type { get; set; }
        public string Question { get; set; }
    }


}
