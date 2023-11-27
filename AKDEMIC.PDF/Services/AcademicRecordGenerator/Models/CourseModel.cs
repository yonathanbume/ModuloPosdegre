namespace AKDEMIC.PDF.Services.AcademicRecordGenerator.Models
{
    public class CourseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string CV { get; set; }
        public bool ByDeferred { get; set; }
        public bool IsElective { get; set; }
        public decimal Credits { get; set; }
        public int FinalGrade { get; set; }
        public bool Approved { get; set; }
        public string FinalGradeText { get; set; }
    }
}
