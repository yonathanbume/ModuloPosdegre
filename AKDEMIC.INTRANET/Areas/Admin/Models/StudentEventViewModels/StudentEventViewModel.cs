using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.StudentEventViewModels
{
    public class StudentEventViewModel
    {
        public AssistanceEventDetailViewModel[] List { get; set; }

    }
    public class AssistanceEventDetailViewModel
    {
        public Guid Id { get; set; }
        public bool Absent { get; set; }
    }
}
