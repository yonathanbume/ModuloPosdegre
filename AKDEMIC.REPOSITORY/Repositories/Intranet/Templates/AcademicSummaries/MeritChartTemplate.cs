using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicSummaries
{
    public class MeritChartTemplate
    {
        public string StudentName { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public string StudentCode { get; set; }
        public decimal WeightedAverage { get; set; }
        public IEnumerable<MeritChartDetailTemplate> Details { get; set; }
    }
}
