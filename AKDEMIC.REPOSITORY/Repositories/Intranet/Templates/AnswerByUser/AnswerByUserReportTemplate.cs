using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AnswerByUser
{
    public class AnswerByUserReportTemplate
    {
        public string student { get; set; }
        public string date { get; set; }
        public Guid id { get; set; }
    }
    public class AnswerByUserTemplate
    {
        public string answerid { get; set; }
        public int countbyanswerid { get; set; }
    }
    public class ReportSurveyDetailDataTable
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string StartEndDate { get; set; }
        public int NumberAnswers { get; set; }
    }
}
