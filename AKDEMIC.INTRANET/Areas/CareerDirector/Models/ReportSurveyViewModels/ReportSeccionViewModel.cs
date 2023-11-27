using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Models.ReportSurveyViewModels
{
    public class ReportSeccionViewModel
    {
        public string Title { get; set; }
        public List<ReportByQuestionViewModel> Reportes { get; set; }
    }
}
