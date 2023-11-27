using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.ViewModels.StudentInformationViewModels
{
    public class QuestionViewModel
    {
        public Guid? Id { get; set; }
        public string Description { get; set; }
        public byte? DescriptionType { get; set; }
        public bool CanEdit { get; set; }
        public bool IsStatic { get; set; }
        public int Type { get; set; }
        public int Score { get; set; }
        public List<AnswerViewModel> Answers { get; set; }
        public string AnswerDirectly { get; set; }

        public Guid? InstitutionalWelfareSectionId { get; set; }
    }
}
