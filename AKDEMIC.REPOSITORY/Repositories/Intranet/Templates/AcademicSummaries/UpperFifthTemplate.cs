using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicSummaries
{
    public class UpperFifthTemplate
    {
        public string StudentName { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public string StudentCode { get; set; }
        public decimal WeightedAverage { get; set; }
        public string RangeOfTerms { get; set; }
        public IEnumerable<UpperFifthDetailsTemplate> Details { get; set; }
    }

    public class UpperFifthDetailsTemplate
    {
        public string Term { get; set; }
        public int MeritOrder { get; set; }
        public decimal Average { get; set; }
        public decimal ApprovedCredits { get; set; }
        public int TotalStudentsInUpperFifth { get; set; }
        public int TotalStudents { get; set; }
        public string Observations { get; set; }
    }
}
