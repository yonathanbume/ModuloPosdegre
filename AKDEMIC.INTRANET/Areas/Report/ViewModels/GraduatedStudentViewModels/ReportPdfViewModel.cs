using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Report.ViewModels.GraduatedStudentViewModels
{
    public class ReportPdfViewModel
    {
        public string Logo { get; set; }
        public string Term { get; set; }
        public List<GraduatedStudentViewModel> Students { get; set; }
    }
}
