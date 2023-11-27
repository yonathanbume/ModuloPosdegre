using AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class ProofOfIncomeViewModel
    {
        public string StudentName { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public int IncomeYear { get; set; }
        public string Modality { get; set; }
        public int Place { get; set; }
        public int AmountOfStudents { get; set; }
        public decimal Score { get; set; }
        public string EnrollmentNumber { get; set; }
        public int StudentSex { get; set; }
        public string PageCode { get; set; }
        public string Date { get;  set; }
        public string Location { get;  set; }
        public string ImageQR { get; set; }
        public UniversityInformation University { get; set; }
    }
    public class RecordOfEnrollmentViewModel
    {
        public string StudentName { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string IncomeYear { get; set; }
        public int IncomeYearNumber { get; set; }
        public string EnrollmentNumber { get; set; }
        public int StudentSex { get; set; }
        public string Semester { get; set; }
        public string StartDate { get; set; }
        public string PageCode { get; set; }
        public string Date { get;  set; }
        public string ImageQR { get; set; }
        public string Location { get;  set; }
        public UniversityInformation University { get; set; }

        public string CustomTitle { get; set; }
        public string CustomContent { get; set; }
    }
    public class RecordOfRegularStudiesViewModel
    {
        public string StudentName { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string IncomeYear { get; set; }
        public string EnrollmentNumber { get; set; }
        public int StudentSex { get; set; }
        public string Semester { get; set; }
        public string StartDate { get; set; }
        public string StudentCondition { get; set; }
        public string PageCode { get; set; }
        public string Date { get;  set; }
        public string Location { get;  set; }
        public string ImageQR { get; set; }
        public string Term { get; set; }
        public UniversityInformation University { get; set; }

        //Custom
        public string CustomTitle { get; set; }
        public string CustomContent { get; set; }
    }
    public class RecordOfEgressViewModel
    {
        public string StudentName { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string GraduatedYear { get; set; }
        public int GraduatedYearNumber { get; set; }
        public string EnrollmentNumber { get; set; }
        public int StudentSex { get; set; }
        public string EndDate { get; set; }
        public string PageCode { get; set; }
        public string Date { get;  set; }
        public string Location { get;  set; }
        public string ImageQR { get; set; }
        public decimal TotalCredits { get; set; }
        public UniversityInformation University { get; set; }

        public string CustomTitle { get; set; }
        public string CustomContent { get; set; }
    }
}
