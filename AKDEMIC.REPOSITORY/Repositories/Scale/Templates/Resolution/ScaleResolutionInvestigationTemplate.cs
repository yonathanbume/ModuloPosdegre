using System;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Resolution
{
    public class ScaleResolutionInvestigationTemplate
    {

        public Guid Id { get; set; }
        public string ExpeditionFormattedDate { get; set; }
        public string ResolutionNumber { get; set; }
        public string ResolutionDocument { get; set; }
        public string InvestigationParticipationType { get; set; }
    }
}
