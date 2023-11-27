namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation.PdfViewModels
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
    }
    public class RecordOfEnrollmentViewModel
    {
        public string StudentName { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string IncomeYear { get; set; }
        public string EnrollmentNumber { get; set; }
        public int StudentSex { get; set; }
        public string Semester { get; set; }
        public string StartDate { get; set; }
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
    }
    public class RecordOfEgressViewModel
    {
        public string StudentName { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string GraduatedYear { get; set; }
        public string EnrollmentNumber { get; set; }
        public int StudentSex { get; set; }
        public string EndDate { get; set; }
    }
}
