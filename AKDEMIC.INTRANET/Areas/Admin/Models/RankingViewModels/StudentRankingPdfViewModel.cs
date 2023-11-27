using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.RankingViewModels
{
    public class StudentRankingPdfViewModel
    {
        public StudentFilterInformationViewModel FilterInformation { get; set; }
        public ICollection<StudentSummaryViewModel> StudentSummaries { get; set; }
    }

    public class StudentFilterInformationViewModel
    {
        public string Career { get; set; }
        public string Campus { get; set; }
        public string Term { get; set; }
        public string AcademicProgram { get; set; }
    }
    public class StudentDetailedRankingPdfViewModel
    {
        public StudentFilterInformationViewModel FilterInformation { get; set; }
        public ICollection<StudentDetailedSummaryViewModel> StudentSummaries { get; set; }
    }
    public class StudentSummaryViewModel
    {
        public int Position { get; set; }
        public string AcademicYear { get; set; }
        public string Code { get; set; }
        public string Career { get; set; }
        public string Campus { get; set; }
        public string Name { get; set; }
        public decimal WeightedAverageGrade { get; set; }
        public decimal Credits { get; set; }
        public int MeritOrder { get; set; }
        public int? MeritType { get; set; }
    }
    public class StudentDetailedSummaryViewModel
    {
        public int Position { get; set; }
        public string AcademicYear { get; set; }
        public string AcademicProgramCode { get; set; }
        public string Code { get; set; }
        public string Career { get; set; }
        public string Campus { get; set; }
        public string Name { get; set; }
        public decimal WeightedAverageGrade { get; set; }
        public decimal Credits { get; set; }
        public int MeritOrder { get; set; }
        public int? MeritType { get; set; }
        public string Plan { get;  set; }
        public string PlanCredit { get;  set; }
        public decimal  EnrollmentCredit{ get; set; }
        public decimal ApprovedCredits { get; set; }
        public decimal DisapprovedCredits { get; set; }
        public string Modality { get; set; }
        public string Condition { get; set; }
    }
}
