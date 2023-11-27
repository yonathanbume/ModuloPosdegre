using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates
{
    public class SurveyDetailTemplate
    {
        public Guid Id { get; set; }
        public Guid? CareerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string PublicationDate { get; set; }
        public string FinishDate { get; set; }
        public bool IsRequired { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
