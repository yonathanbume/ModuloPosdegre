using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class ConditionedStudentSectionTemplate
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Term { get; set; }
        public string Course { get; set; }
        public string Career { get; set; }
        public int Try { get; set; }
    }
}
