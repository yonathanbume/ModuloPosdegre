using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class Tutorial : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        
        public string TeacherId { get; set; }
        
        public Guid ClassroomId { get; set; }

        public Guid SectionId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsDictated { get; set; } = false;

        public ApplicationUser Teacher { get; set; }

        public Classroom Classroom { get; set; }

        public Section Section { get; set; }

        public ICollection<TutorialStudent> TutorialStudents { get; set; }
    }
}
