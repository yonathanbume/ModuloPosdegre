using System;

namespace AKDEMIC.INTRANET.ViewModels.AcademicSummaryViewModels
{
    public class StudentTermViewModel
    {
        public Guid TermId { get; set; }

        public string TermName { get; set; }

        public string CareerName { get; set; }

        public SummaryViewModel AcademicSummary { get; set; }
    }

    public class SummaryViewModel
    {
        public string TotalCredits { get; set; }

        public decimal ApprovedCredits { get; set; }

        public int StudentAcademicYear { get; set; }

        public string MeritOrder { get; set; }

        public string Order { get; set; }

        public string WeightedFinalGrade { get; set; }

        public string CumulativeWeightedFinalGrade { get; set; }

        public string Observations { get; set; }

        public string Condition { get; set; }

        public string Status { get; set; }
    }
}
