using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.GradeViewModels.PdfViewModel
{
    public class EvaluationReportViewModel
    {
        public string Faculty { get; set; }
        public string Carrer { get; set; }
        public string Term { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public decimal Credits { get; set; }
        public string Teacher { get; set; }
        public string Section { get; set; }
        public byte Year{ get; set; }
        public byte AcademicYear { get; set; }
        public string ImagePathLogo { get; set; }

        public List<StudentGradeViewModel> Students { get; set; }
    }

    public class StudentGradeViewModel
    {
        public string Code { get; set; }
        public string FullName { get; set; }
        public int Grade { get; set; }
        public string GradeText { get; set; }
    }
}
