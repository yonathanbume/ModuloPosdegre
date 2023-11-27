using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class ScoreInputSchedule : Entity
    {
        public Guid Id { get; set; }

        public Guid? CourseComponentId { get; set; }
        public CourseComponent CourseComponent { get; set; }

        public Guid TermId { get; set; }
        public Term Term { get; set; }

        public IEnumerable<ScoreInputScheduleDetail> Details { get; set; }
    }
}