using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey
{
    public class SurveyUserTemplate
    {
        public Guid SurveyUserId { get; set; }
        public string SurveyName { get; set; }
        public string PublicationDate { get; set; }
        public string UserName { get; set; }
        public bool Solved { get; set; }
        public bool Expired { get; set; }
        public List<SurveyItemTemplate> SurveyItems { get; set; }
    }

    public class SurveyItemTemplate
    {
        public string Title { get; set; }
        public bool IsLikert { get; set; }
        public List<QuestionsTemplate> Questions { get; set; }
    }


    public class QuestionsTemplate
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Response { get; set; }
        public List<AnswersTemplate> Answers { get; set; }
        public List<Guid> Selection { get; set; }
        public List<string> LikertSelection { get; set; }
    }

    public class AnswersTemplate
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
    }
}
