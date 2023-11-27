using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class DeferredExam : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public Guid? ClassroomId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public Section Section { get; set; }
        public Classroom Classroom { get; set; }

        //public string AssignedTeacherId { get; set; }
        //public Teacher AssignedTeacher { get; set; }

        public List<DeferredExamStudent> DeferredExamStudents { get; set; }
    }
}
