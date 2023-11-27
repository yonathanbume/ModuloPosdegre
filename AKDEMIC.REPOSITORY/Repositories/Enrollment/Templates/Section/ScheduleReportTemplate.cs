using AKDEMIC.CORE.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Section
{
    public class ScheduleReportTemplate
    {
        public int PedagogicalHourTime { get; set; }
        public int WeekDay { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SessionType { get; set; }
        public Guid? SectionGroupId { get; set; }
        public string SectionGroup { get; set; }
        public double Duration => PedagogicalHourTime == 0 ? 0 : Math.Round((EndTime.ToLocalTimeSpanUtc().Subtract(StartTime.ToLocalTimeSpanUtc()).TotalMinutes / PedagogicalHourTime*1.0), 1, MidpointRounding.AwayFromZero);
    }
}
