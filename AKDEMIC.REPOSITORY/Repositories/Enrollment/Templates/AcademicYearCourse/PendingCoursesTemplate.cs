using System;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.AcademicYearCourse
{
    public class PendingCoursesTemplate
    {
        public Guid id { get;  set; }
        public string Code { get;  set; }
        public string text { get;  set; }
        public decimal Credits { get;  set; }
    }
}
