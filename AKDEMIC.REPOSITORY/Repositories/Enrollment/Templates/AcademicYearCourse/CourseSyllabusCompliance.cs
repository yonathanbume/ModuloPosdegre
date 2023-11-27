using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.AcademicYearCourse
{
    public class CourseSyllabusCompliance
    {
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; }
        public string Course { get; set; }
        public string Curriculum { get; set; }
        public string Career { get; set; }
        public string AcademicYear { get; set; }
        public byte Status { get; set; }
        public string Coordinator { get; set; }
        public bool OutOfDate { get; set; }
    }
}
