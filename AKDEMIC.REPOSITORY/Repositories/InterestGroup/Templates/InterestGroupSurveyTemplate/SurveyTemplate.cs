using System;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.InterestGroupSurveyTemplate
{
    public class SurveyTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string PublicationDate { get; set; }
        public string CreatedDate { get; set; }
        public string FinishDate { get; set; }
        public string Title { get; set; }
        public Guid InterestGroupId { get; set; }
        public string InterestGroupName { get; set; }
        public string Status { get; set; }
    }
}
