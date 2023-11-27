using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicSummaries
{
    public class AcademicSummaryByCareerTemplate
    {
        public string Faculty { get; set; }
        public string AcademicProgram { get; set; }
        public Guid CareerId { get; set; }
        public string Career { get; set; }
        public int Total { get; set; }
        public List<AcademicSummaryByCareerDetailTemplate> AcademicYears { get; set; } = new List<AcademicSummaryByCareerDetailTemplate>();

    }

    public class AcademicSummaryByCareerDetailTemplate
    {
        public int AcademicYear { get; set; }
        public int Quantity { get; set; }
    }
}
