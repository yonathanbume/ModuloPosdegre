using System;

namespace AKDEMIC.INTRANET.ViewModels.SurveyViewModels
{
    public class AnswerViewModel
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public String Description { get; set; }
    }
}
