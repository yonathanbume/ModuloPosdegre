using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ExtracurricularCourseGroup : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public Guid ExtracurricularCourseId { get; set; }
        public ExtracurricularCourse ExtracurricularCourse { get; set; }

        public Guid TermId { get; set; }
        public Term Term { get; set; }

        public ICollection<ExtracurricularCourseGroupStudent> GroupStudents { get; set; }
    }
}
