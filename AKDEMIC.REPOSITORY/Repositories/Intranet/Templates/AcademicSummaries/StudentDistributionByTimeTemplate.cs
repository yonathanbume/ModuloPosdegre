using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicSummaries
{
    public class StudentDistributionByTimeTemplate
    {
        public string Term { get; set; }

        public int SixYears { get; set; }

        public int TenYears { get; set; }

        public int FifteenYears { get; set; }

        public int TwentyYears { get; set; }

        public int MoreThanTwentyYears { get; set; }

        public int Total { get; set; }
    }
}
