using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicSummaries
{
    public class AcademicPerformanceSummaryTemplate
    {
        public string Term { get; set; }
        public decimal Average { get; set; }
        public decimal ApprovedCredits { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal ExtraCredits { get; set; }
        public int  Year { get; set; }
        public string Number { get; set; }

        public decimal DisapprovedCredits => TotalCredits - ApprovedCredits;

        public decimal TermScore { get; set; }
        public decimal ExtraScore { get; set; }
        public decimal CumulativeScore { get; set; }
        public decimal CumulativeWeightedAverage { get; set; }
        public decimal WeightedAverageGrade { get; set; }

        public DateTime TermStartDate { get; set; }
    }
}
