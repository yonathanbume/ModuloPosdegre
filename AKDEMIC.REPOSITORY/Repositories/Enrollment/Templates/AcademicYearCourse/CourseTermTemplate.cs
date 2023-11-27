using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.AcademicYearCourse
{
    public class CourseTermTemplate
    {
        public byte AcademicYear { get; set; }

        public string Teachers { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string AcademicProgram { get; set; }

        public decimal Credits { get; set; }

        public string Modality { get; set; }
    }
}
