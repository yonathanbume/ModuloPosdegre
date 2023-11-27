using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Section
{
    public class SectionIncompleteScheduleReportTemplate
    {
        public string Term { get; set; }
        public string Career { get; set; }
        public string Curriculum { get; set; }

        public string Img { get; set; }

        public List<SectionIncompleteScheduleDetailReportTemplate> Sections { get; set; }
    }
}
