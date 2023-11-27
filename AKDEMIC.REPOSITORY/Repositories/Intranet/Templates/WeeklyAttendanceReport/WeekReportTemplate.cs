using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.WeeklyAttendanceReport
{
    public class WeekReportTemplate
    {
        public int Week { get; set; }
        public decimal AverageAttendances { get; set; }
        public decimal AttendancePercentage { get; set; }
        public decimal AverageAbsences { get; set; }

    }
}
