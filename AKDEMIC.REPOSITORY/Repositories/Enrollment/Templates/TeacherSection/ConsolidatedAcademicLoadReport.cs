using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.TeacherSection
{
    public class ConsolidatedAcademicLoadReport
    {
        public string TeacherId { get; set; }
        public string Teacher { get; set; }
        public string AcademicDepartment { get; set; }
        public double TotalHours { get; set; }
        public List<ConsolidatedAcademicLoadDetailsReport> Details { get; set; }
    }

    public class ConsolidatedAcademicLoadDetailsReport
    {
        public string Course { get; set; }
        public string Section { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Staurday { get; set; }
    }
}
