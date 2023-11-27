using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.WorkingDay
{
    public class WorkingDayConsolited
    {
        public string User { get; set; }
        public string UserId { get; set; }
        public List<WorkingDayConsolitedDetail> Details { get; set; } = new List<WorkingDayConsolitedDetail>();
    }

    public class WorkingDayConsolitedDetail
    {
        public string Date { get; set; }
        public DateTime DateTime { get; set; }
        public int Day { get; set; }
        public string DayOfWeek { get; set; }
        public string ScheduleRange { get; set; }
        public string FirstEntryStr { get; set; }
        public TimeSpan FirstEntry { get; set; }
        public string LastExitStr { get; set; }
        public string Delay { get; set; }
    }
}
