namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class CourseReportCardTemplate
    {
        public string Curriculum { get; set; }
        public byte Year { get; set; }
        public string Code { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
        public decimal Credits { get; set; }
        public byte TheoreticalHours { get; set; }
        public byte PracticalHours { get; set; }
        public int Grade { get; set; }
        public int Status { get; set; }
        public string Teacher { get; set; }
    }
}
