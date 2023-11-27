using System;
using System.Collections.Generic;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class SyllabusRequest
    {
        public Guid Id { get; set; }
        public Guid TermId { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Term Term { get; set; }
        public byte Type { get; set; } = ConstantHelpers.SYLLABUS_REQUEST.TYPE.MIXED;
        public ICollection<SyllabusTeacher> SyllabusTeachers { get; set; }
    }
}
