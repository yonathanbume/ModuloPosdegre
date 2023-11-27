using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Models.ReportSurveyViewModels
{
    public class ReportByQuestionViewModel
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public List<AlternativeViewModel> Alternatives { get; set; }
        public List<string> Answers { get; set; }
    }
}
