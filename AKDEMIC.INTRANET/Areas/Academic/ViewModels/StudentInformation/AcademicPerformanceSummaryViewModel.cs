using AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class AcademicPerformanceSummaryViewModel
    {
        public string StudentName { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public string StudentCode { get; set; }
        public List<AcademicPerformanceSummaryDetailViewModel> Details { get; set; }
        public int StudentGender { get; set; }
        public string CurrentSemester { get; set; }
        public string PageCode { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public string ImageQR { get; set; }
        public UniversityInformation University { get; set; }
    }

    public class AcademicPerformanceSummaryDetailViewModel
    {
        public string Term { get; set; }
        public decimal Average { get; set; }
        public decimal ApprovedCredits { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal DisapprovedCredits { get; set; }
    }
}
