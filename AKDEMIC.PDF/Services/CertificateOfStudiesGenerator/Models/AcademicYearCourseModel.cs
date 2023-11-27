using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.PDF.Services.CertificateOfStudiesGenerator.Models
{
    public class AcademicYearCourseModel
    {
        public int AcademicYear { get; set; }
        public List<CourseModel> Courses { get; set; }
    }
}
