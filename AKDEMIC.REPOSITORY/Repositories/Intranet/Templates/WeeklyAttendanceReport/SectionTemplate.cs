using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.WeeklyAttendanceReport
{
    public class SectionTemplate
    {
        public string Course { get; set; }
        public string Section { get; set; }
        public string AcademicYear { get; set; }
        public int EnrolledStudents { get; set; }
        public List<WeekReportTemplate> WeekReport { get; set; }
        public decimal AttendanceAverage { get; set; }
        public decimal AttendancePercentage { get; set; }
    }
}
