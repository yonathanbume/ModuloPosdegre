using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicHistory
{
    public class CourseEquivalenceTemplate
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int Grade { get; set; }
        public string OldCourseCode { get; set; }
        public string OldCourseName { get; set; }
        public string Term { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }
        public string Date { get; set; }
    }
}
