using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.EvaluationReportViewModels
{
    public class LoadEvaluationReportViewModel
    {
        public Guid? EvaluationReportId { get; set; }
        public string Code { get; set; }
        public string Term { get; set; }
        public Guid TermId { get; set; }
        public string ReceptionDate { get; set; }
        public string Observations { get; set; }
        public bool Valid { get; set; }
        public bool New { get; set; }
    }
}
