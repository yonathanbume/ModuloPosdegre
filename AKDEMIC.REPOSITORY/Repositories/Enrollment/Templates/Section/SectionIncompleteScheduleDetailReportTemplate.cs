using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Section
{
    public class SectionIncompleteScheduleDetailReportTemplate
    {
        public string CourseCode { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
        public int Enrolled { get; set; }
        public byte TheoricalHours { get; set; }
        public byte PracticalHours { get; set; }
        public decimal Credits { get; set; }
        public List<ScheduleReportTemplate> Schedules { get; set; }
        public List<string> Teachers { get; set; }
    }
}
