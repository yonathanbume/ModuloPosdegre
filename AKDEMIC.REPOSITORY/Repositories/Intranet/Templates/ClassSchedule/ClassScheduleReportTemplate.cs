using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassSchedule
{
    public class ClassScheduleReportTemplate
    {
        public string Image { get; set; }
        public string HeaderText { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string TermName { get; set; }
        public string CareerName { get; set; }
        public string CurriculumCode { get; set; }
        public List<(TimeSpan, TimeSpan, string)> ScheduleParameters { get; set; }
        public List<SectionScheduleTemplate> SectionSchedules { get; set; }
    }

    public class SectionScheduleTemplate
    {
        public string Code { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int WeekDay { get; set; }
        public string TimeText { get; set; }
    }
}
