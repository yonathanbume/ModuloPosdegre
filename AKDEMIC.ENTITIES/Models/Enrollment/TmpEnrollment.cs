using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class TmpEnrollment : Entity, ITimestamp, ISoftDelete
    {
        public Guid Id { get; set; }
        
        public Guid SectionId { get; set; }
        public Section Section { get; set; }

        public Guid? SectionGroupId { get; set; }
        public SectionGroup SectionGroup { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public bool IsAdminRectification { get; set; }
        public bool WasApplied { get; set; }
        public bool IsParallelCourse { get; set; }

    }
}
