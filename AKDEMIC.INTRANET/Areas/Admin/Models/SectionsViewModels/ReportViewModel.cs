using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SectionsViewModels
{
    public class ReportViewModel
    {
        public string Faculty { get; set; }

        public string Carrer { get; set; }

        public string Term { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public decimal Credits { get; set; }

        public string Teacher { get; set; }

        public string Section { get; set; }

        public byte Year { get; set; }

        public byte AcademicYear { get; set; }

        public string ImagePathLogo { get; set; }

        public List<StudentGradeViewModel> Students { get; set; }
    }
}
