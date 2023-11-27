using System;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.TutorialStudentAssistanceViewModels
{
    public class TutorialStudentViewModel
    {
        public TutorialStudentDetailViewModel[] List { get; set; }
    }
    public class TutorialStudentDetailViewModel
    {
        public Guid TutorialId { get; set; }
        public Guid Id { get; set; }
        public bool Absent { get; set; }
    }
}
