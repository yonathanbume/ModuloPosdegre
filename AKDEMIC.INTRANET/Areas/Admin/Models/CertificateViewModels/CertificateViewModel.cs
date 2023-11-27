namespace AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels
{
    public class CertificateViewModel
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Credits  { get; set; }
        public int Grade { get; set; }
        public string TermName { get; set; }
        public decimal TermMinGrade { get; set; }
        public int AcademicYear { get; set; }
        public int AcademicNumber { get; set; }
        public string Type { get; set; }
        public string EvaluationReportDate { get; set; }
        public string Observations { get; set; }
    }

    public class UniversityInformation
    {
        public string Campus { get; set; }
        public string PhoneNumber { get; set; }
        public string WebSite { get; set; }
        public string Address { get; set; }
        public string Sender { get; set; }
        public string Office { get; set; }
        public string HeaderOffice { get; set; }
        public string YearInformation { get; set; }
    }
}
