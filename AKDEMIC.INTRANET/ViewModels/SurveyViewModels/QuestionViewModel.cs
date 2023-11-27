using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.ViewModels.SurveyViewModels
{
    public class QuestionViewModel
    {
        public Guid Id { get; set; } 
        public Guid SurveyId { get; set; } 
        public int Type { get; set; } 
        public String Description { get; set; } 
        public List<AnswerViewModel> Answers { get; set; }
    }
}
