using System;

namespace AKDEMIC.INTRANET.ViewModels.SurveyViewModels
{
    public class AnswerByUserViewModel
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public string Description { get; set; }
    }
}
