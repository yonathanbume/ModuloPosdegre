using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;

namespace AKDEMIC.ENTITIES.Models.InterestGroup
{
    public class InterestGroupSurvey
    {
        public Guid Id { get; set; }
        public Guid InterestGroupId { get; set; }
        public Guid SurveyId { get; set; }
        public Survey Survey { get; set; }
        public byte Type { get; set; } = ConstantHelpers.INTEREST_GROUP_SURVEY.TYPE.INTERNAL;
        public string SurveyLink { get; set; }
        public InterestGroup InterestGroup { get; set; }
    }
}
