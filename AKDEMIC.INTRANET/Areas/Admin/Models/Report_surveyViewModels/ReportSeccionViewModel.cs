using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.Report_surveyViewModels
{
    public class ReportByQuestionViewModel
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public List<AlternativeViewModel> Alternatives { get; set; }
        public List<string> Answers { get; set; }
    }

    public class ReportSeccionViewModel
    {
        public string Title { get; set; }
        public List<ReportByQuestionViewModel> Reportes { get; set; }
    }

    public class AlternativeViewModel
    {
        public string Description { get; set; }
        public int Count { get; set; }
    }
}
