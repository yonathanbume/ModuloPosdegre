using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.ViewModels.StudentInformationViewModels
{
    public class SectionViewModel
    {
        public Guid? Id { get; set; }
        public bool CanEdit { get; set; }
        public Guid? RecordId { get; set; }
        public string Title { get; set; }
        public int MaxScore { get; set; }
        public bool CanActive { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }
}
