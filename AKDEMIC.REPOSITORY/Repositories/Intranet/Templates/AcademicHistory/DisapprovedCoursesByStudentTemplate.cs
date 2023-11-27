using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicHistory
{
    public class DisapprovedCoursesByStudentTemplate
    {
        public string UserName { get; set; }
        public string Name { get; set; }

        public string Faculty { get; set; }
        public string Career { get; set; }
        public string Term { get; set; }
        public int DisapprovedCourses { get; set; }
    }
}
