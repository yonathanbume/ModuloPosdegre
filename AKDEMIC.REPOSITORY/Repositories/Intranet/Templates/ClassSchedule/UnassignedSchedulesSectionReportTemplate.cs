using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassSchedule
{
    public class UnassignedSchedulesSectionReportTemplate
    {
        public string Career { get; set; }
        public Guid CurriculumId { get; set; }
        public string Curriculum { get; set; }
        public byte AcademicYear { get; set; }
        public string CourseCode { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
        public List<ScheduleTemplate> Schedules { get; set; }
    }
}
