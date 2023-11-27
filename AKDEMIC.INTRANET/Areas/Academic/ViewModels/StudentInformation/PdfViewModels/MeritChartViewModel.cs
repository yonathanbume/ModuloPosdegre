using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation.PdfViewModels
{
    public class MeritChartViewModel
    {
        public string StudentName { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public string StudentCode { get; set; }
        public decimal WeightedAverage { get; set; }
        public IEnumerable<MeritChartDetailViewModel> Details { get; set; }
    }

    public class MeritChartDetailViewModel
    {
        public string Term { get; set; }
        public int MeritOrder { get; set; }
        public decimal Average { get; set; }
        public int ApprovedCredits { get; set; }
        public int TotalStudents { get; set; }
        public string Observations { get; set; }
    }
}
