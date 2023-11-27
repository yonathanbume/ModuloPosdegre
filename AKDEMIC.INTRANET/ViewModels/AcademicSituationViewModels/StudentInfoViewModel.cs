using System;

namespace AKDEMIC.INTRANET.ViewModels.AcademicSituationViewModels
{
    public class StudentInfoViewModel
    {
        public Guid Id { get; set; }

        public string CareerName { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public decimal WeightedFinalGrade { get; set; }

        public decimal CumulativeWeightedFinalGrade { get; set; }

        public int Order { get; set; }

        public string Observations { get; set; }

        public string Term { get; set; }

        public string Status { get; set; }

        public decimal ApprovedCredits { get; set; }

        public int AcademicYear { get; set; }

        public string Dni { get; set; }

        public decimal RequiredApprovedCredits { get; set; }
        public decimal RequiredCredits { get; set; }
        public decimal ElectiveApprovedCredits { get; set; }
        public decimal ElectiveCredits { get; set; }
    }
}
