using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Course
{
    public class CourseTeacherReportExcel
    {
        public string CourseCode { get; set; }
        public string Course { get; set; }
        public string Type { get; set; }
        public string Career { get; set; }
        public decimal Credits { get; set; }
        public int Sections { get; set; }
        public string Coordinator { get; set; }
    }
}
