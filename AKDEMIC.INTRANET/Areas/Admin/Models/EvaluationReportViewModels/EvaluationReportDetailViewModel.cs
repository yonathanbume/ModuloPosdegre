using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.EvaluationReportViewModels
{
    public class EvaluationReportDetailViewModel
    {
        public Guid Id { get; set; }
        public string Section { get; set; }
        public string Course { get; set; }
        public string EvaluationReportCode { get; set; }
        public byte Type { get; set; }
    }
}
