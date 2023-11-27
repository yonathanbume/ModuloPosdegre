namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation.PdfViewModels
{
    public class AcademicRecordViewModel
    {
        public string Student { get; set; }
        public string EnrollmentCode { get; set; }
        public string StartYear { get; set; }
        public string Speciality { get; set; }
        public string StartDateSemester { get; set; }
        public string GraduatedDateSemester { get; set; }
        public string RegularStudies { get; set; }
        public int DisapprovedCourses { get; set; }
        public int RecoveredCourses { get; set; }
        public decimal Average { get; set; }
        public string Observations { get; set; }


        public string StartInstitution { get; set; }
        public string AdmisionExamDate { get; set; }
        public string DateOfFirstSemesterStart { get; set; }
        public string TransferResolution { get; set; }
        public string PlaceIn { get; set; }
        public string QuantityOfCoursesAtStartIntitution { get; set; }
        public string QunatityOfCoursesAtThisInstitution { get; set; }


        public string Reason { get; set; }
        public string Time { get; set; }
        public string Reboot { get; set; }
        public string ChangeOfCurriculum { get; set; }
        public string Date { get; set; }
        public string ExitSemester { get; set; }
        public string SemesterOfReboot { get; set; }
        public int BreakDisapprovedCourses { get; set; }
        public int BreakRecoveredCourses { get; set; }
    }
}
