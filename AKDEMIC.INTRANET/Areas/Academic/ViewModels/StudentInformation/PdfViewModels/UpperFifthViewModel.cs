using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation.PdfViewModels
{
    public class UpperFifthViewModel
    {
        public string StudentName { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public string StudentCode { get; set; }
        public decimal WeightedAverage { get; set; }
        public string RangeOfTerms { get; set; }
        public IEnumerable<UpperFifthDetailsViewModel> Details { get; set; }
    }

    public class UpperFifthDetailsViewModel
    {
        public string Term { get; set; }
        public int MeritOrder { get; set; }
        public decimal Average { get; set; }
        public int ApprovedCredits { get; set; }
        public int TotalStudentsInUpperFifth { get; set; }
        public int TotalStudents { get; set; }
        public string Observations { get; set; }
    }
}
