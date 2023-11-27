using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class SectionApprovedDataTemplate
    {
        public string Term { get; set; }
        public string Career { get; set; }
        public string Curriculum { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Section { get; set; }
        public decimal Credits { get; set; }
        public int AcademicYear { get; set; }
        
        public int Enrolled { get; set; }
        public int Approved { get; set; }
        public int Disapproved { get; set; }
        public string Teacher { get; set; }
        public string TeacherReport { get; set; }
    }
}
