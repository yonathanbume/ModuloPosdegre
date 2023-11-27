using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Models.RankingViewModels
{
    public class GraduatedRankingPdfViewModel
    {
        public GraduatedFilterInformationViewModel GraduatedFilterInformation { get; set; }
        public ICollection<GraduatedSummaryViewModel> GraduatedSummaries { get; set; }
    }

    public class GraduatedFilterInformationViewModel
    {
        public string Career { get; set; }
        public string AdmissionTerm { get; set; }
        public string GraduationTerm { get; set; }
        public string AdmissionTermHyphenated { get; set; }
        public string GraduationTermHyphenated { get; set; }
    }

    public class GraduatedSummaryViewModel
    {
        public int Position { get; set; }
        public string Code { get; set; }
        public string Dni { get; set; }
        public string Career { get; set; }
        public string Name { get; set; }
        public string AdmissionTerm { get; set; }
        public string GraduationTerm { get; set; }
        public decimal WeightedAverageGrade { get; set; }
        public int MeritOrder { get; set; }
        public int? MeritType { get; set; }
    }
}
