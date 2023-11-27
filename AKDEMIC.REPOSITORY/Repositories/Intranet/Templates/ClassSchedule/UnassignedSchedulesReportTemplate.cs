using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassSchedule
{
    public class UnassignedSchedulesReportTemplate
    {
        public string Img { get; set; }
        public string Term { get; set; }
        public string Career { get; set; }
        public string Curriculum { get; set; }
        public List<UnassignedSchedulesSectionReportTemplate> Sections { get; set; }
    }
}
