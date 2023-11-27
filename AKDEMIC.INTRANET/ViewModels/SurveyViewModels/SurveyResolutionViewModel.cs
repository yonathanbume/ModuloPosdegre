using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.ViewModels.SurveyViewModels
{
    public class SurveyResolutionViewModel
    {
        public Guid SurveyUserId { get; set; }
        public bool Solved { get; set; }
        public List<SurveyItemViewModel> SurveyItems { get; set; }
    }

    public class SurveyItemViewModel
    {
        public string Title { get; set; }
        public List<QuestionResolutionViewModel> Questions { get; set; }
    }


    public class QuestionResolutionViewModel
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Response { get; set; }
        public List<AnswerResolutionViewModel> Answers { get; set; }
        public List<Guid> Selection { get; set; }
    }

    public class AnswerResolutionViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
    }
}
