using AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class UpperFifthViewModel
    {
        public string CustomTitle { get; set; }
        public string CustomContent { get; set; }
        public string StudentName { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public string StudentCode { get; set; }
        public decimal WeightedAverage { get; set; }
        public string RangeOfTerms { get; set; }
        public IEnumerable<UpperFifthDetailsViewModel> Details { get; set; }
        public int StudentSex { get; set; }
        public bool IsGraduated { get; set; }
        public string CurrentSemester { get; set; }
        public string PageCode { get; set; }
        public string Location { get;  set; }
        public string Date { get;  set; }
        public string ImageQR { get; set; }
        public UniversityInformation University { get; set; }
    }

    public class UpperFifthDetailsViewModel
    {
        public string Term { get; set; }
        public int MeritOrder { get; set; }
        public double Average { get; set; }
        public double ApprovedCredits { get; set; }
        public int TotalStudentsInUpperFifth { get; set; }
        public int TotalStudents { get; set; }
        public string Observations { get; set; }
    }
}
