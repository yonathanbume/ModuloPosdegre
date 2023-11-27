using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.StudentAssistanceViewModels
{
    public class AssistanceViewModel
    {
        public Guid ClassId { get; set; }
        public Guid? ActivityId { get; set; }
        public Guid? VirtualClassId { get; set; }
        public string Commentary { get; set; }
        public AssistanceDetailViewModel[] List { get; set; }
        public EvaluationViewModel[] Evaluations { get; set; }
    }

    public class AssistanceDetailViewModel
    {
        public Guid? ClassStudentId { get; set; }
        public bool IsAbsent { get; set; }
        public Guid StudentId { get; set; }
        public bool DPI { get; set; }
    }

    public class EvaluationViewModel
    {
        public Guid EvaluationId { get; set; }
        public bool Taken { get; set; } //Asignada a la clase
        public string Name { get; set; }
        public string Description { get; set; }
        public int Percentage { get; set; }
    }
}
