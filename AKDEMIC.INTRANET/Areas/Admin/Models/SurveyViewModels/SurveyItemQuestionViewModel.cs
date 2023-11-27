using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SurveyViewModels
{
    public class SurveyItemQuestionViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsLikert { get; set; }
        public bool Sended { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }
}
