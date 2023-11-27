using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Section
{
    public class SectionProgressReportDetailTemplate
    {
        public DateTime? Date { get; set; }
        public string Subject { get; set; }
        public int Students { get; set; }
        public string Observation { get; set; }
        public int SessionType { get; set; }
        public Guid ActivityId { get; set; }
    }
}
