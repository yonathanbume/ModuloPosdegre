using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Section
{
    public class SectionProgressReportTemplate
    {
        public string Career { get; set; }
        public string AcademicDepartment { get; set; }
        public string Teacher { get; set; }
        public string Course { get; set; }
        public string Term { get; set; }
        public string Curriculum { get; set; }
        public int AcademicYear { get; set; }
        public string Section { get; set; }
        public int Enrolled { get; set; }
        public int HT { get; set; }
        public int HP { get; set; }
        public decimal Progress { get; set; }

        public Guid CourseId { get; set; }
        public Guid TermId { get; set; }

        public List<SectionProgressReportDetailTemplate> Details { get; set; }
    }
}
