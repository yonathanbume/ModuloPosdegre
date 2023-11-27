using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ReportTeacherAssistanceViewModels
{
    public class ReportSectionAssistanceViewModel
    {
        public string Course { get; set; }
        public string Section { get; set; }
        public int DictatedClasses { get; set; }
        public int RescheduledClasses { get; set; }
        public int TotalClasses { get; set; }

    }
}
