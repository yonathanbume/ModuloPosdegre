using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class UpdateEvaluationReportViewModel
    {
        public Guid AcademicHistoryId { get; set; }
        public Guid EvaluationReportId { get; set; }
        public string EvaluationReportCode { get; set; }
        public string Observation { get; set; }
        public IFormFile File { get; set; }
        public Guid TermId { get; set; }
        public string EvaluationReportReceptionDate { get; set; }
        public string EvaluationReportCreatedAt { get; set; }
        public string EvaluationReportLastGradePublished { get; set; }
    }
}
