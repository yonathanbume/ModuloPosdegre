using AKDEMIC.INTRANET.Helpers;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels
{
    public class UnamCourseCertificateViewModel
    {
        public string CourseName { get; set; }
        public decimal CourseCredits { get; set; }
        public int CourseFinalGrade { get; set; }
        public string CourseFinalGradeText => ConvertHelpers.NumberToText(CourseFinalGrade);
        public string TermName { get; set; }
        public bool IsElective { get; set; }
        public bool Approved { get; set; }
        public bool Validated { get; set; }
    }
}
