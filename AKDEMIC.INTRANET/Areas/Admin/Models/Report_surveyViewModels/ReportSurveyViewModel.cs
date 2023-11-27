using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.Report_surveyViewModels
{
    public class ReportSurveyViewModel
    {
        public Guid id { get; set; }
        public int NumberAnswers { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string StartEndDate { get; set; }
    }
}
