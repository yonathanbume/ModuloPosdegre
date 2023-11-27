using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey
{
    public class SurveyTemplateA
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PublishDate { get; set; }
        public int SurveyuserCount { get; set; }
    }
}