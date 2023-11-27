using System;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Models.ReportSurveyViewModels
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
