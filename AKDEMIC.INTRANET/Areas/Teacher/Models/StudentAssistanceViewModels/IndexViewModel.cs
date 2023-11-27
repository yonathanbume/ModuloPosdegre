using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.StudentAssistanceViewModels
{
    public class IndexViewModel
    {
        public string ErrorMessage { get; set; }

        public bool EnableUpdateClass { get; set; }

        public ClassViewModel Class { get; set; }
    }

    public class ClassViewModel
    {
        public Guid Id { get; set; }

        public float AbsencePercentage { get; set; }

        public int ClassNumber { get; set; }

        public int WeekNumber { get; set; }
        public string SessionType { get; set; }
        public Guid? ActivityId { get; set; }
        public Guid? VirtualClassId { get; set; }
        public bool HasVirtualClass => VirtualClassId.HasValue && VirtualClassId != Guid.Empty ? true : false;

        public string Date { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public SectionViewModel Section { get; set; }
        public List<EvaluationViewModel> Evaluations { get; set; }
        public string Commentary { get; set; }
    }

    public class CourseViewModel
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public Guid Id { get; set; }
    }

    public class SectionViewModel
    {
        public Guid Id { get; set; }
        public CourseViewModel Course { get; set; }
        public string Code { get; set; }
    }

}
