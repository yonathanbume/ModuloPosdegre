using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class StudentScheduleViewModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string TermName { get; set; }
        public string CareerName { get; set; }
        public string CurriculumCode { get; set; }
        public List<SectionScheduleViewModel> SectionSchedules { get; set; }
    }

    public class SectionScheduleViewModel
    {
        public string CourseCode { get; set; }
        public string SectionCode { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan StartTimeLocal { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan EndTimeLocal { get; set; }
        public int WeekDay { get; set; }
        public string TimeText { get; set; }
    }
}
