using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ExtracurricularCourseGroupAssistanceStudent
    {
        public Guid GroupAssistanceId { get; set; }
        public ExtracurricularCourseGroupAssistance GroupAssistance { get; set; }

        public Guid GroupStudentId { get; set; }
        public ExtracurricularCourseGroupStudent GroupStudent { get; set; }

        public bool State { get; set; } = false;
    }
}
