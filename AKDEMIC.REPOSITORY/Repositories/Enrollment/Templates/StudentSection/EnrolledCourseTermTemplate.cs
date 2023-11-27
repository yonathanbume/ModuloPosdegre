using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class EnrolledCourseTermTemplate
    {
        public string Code { get; set; }
        public string Course { get; set; }
        public byte AcademicYear { get; set; }
        public string AcademicProgram { get; set; }
        public int Students { get; set; }
        public string Area { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
    }
}
