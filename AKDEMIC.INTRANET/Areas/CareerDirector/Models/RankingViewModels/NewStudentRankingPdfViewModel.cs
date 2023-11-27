using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Models.RankingViewModels
{
    public class NewStudentRankingPdfViewModel
    {
        public NewStudentFilterInformationViewModel NewStudentFilterInformation { get; set; }
        public ICollection<NewStudentSummaryViewModel> NewStudentSummaries { get; set; }
    }

    public class NewStudentFilterInformationViewModel
    {
        public string Career { get; set; }
        public string CareerCode { get; set; }
        public string AdmissionTerm { get; set; }
        public string AdmissionTermHyphenated { get; set; }
        public string Status { get; set; }
    }

    public class NewStudentSummaryViewModel
    {
        public int Position { get; set; }
        public string Code { get; set; }
        public string Dni { get; set; }
        public string Name { get; set; }
        public string CareerCode { get; set; }
        public string CurriculumCode { get; set; }
        public string FirstCampus { get; set; }
        public string CurrentCampus { get; set; }
        public string AdmissionTerm { get; set; }
        public string LastTerm { get; set; }
        public decimal? LastWeightedAverageGrade { get; set; }
        public string GraduationTerm { get; set; }
        public decimal? GraduationWeightedAverageGrade { get; set; }
        public string Status { get; set; }
        public string CurrentAcademicYear { get; set; }
        public int? MeritType { get; set; }
    }
}
