using System.Collections.Generic;

namespace AKDEMIC.INTRANET.ViewModels.SurveyViewModels
{
    public class RespondSurveyViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<QuestionViewModel> QuestionViewModels { get; set; }
    }
}
