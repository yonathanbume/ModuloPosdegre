using System;

namespace AKDEMIC.INTRANET.ViewModels.StudentInformationViewModels
{
    public class AnswerViewModel
    {
        public Guid Id{ get; set; }
        public string Description { get; set; }
        public int Score { get; set; }
        public Guid? SelectedAnswer { get; set; }
    }
}
