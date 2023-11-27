using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.ContinuingEducation
{
    public class CourseType : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Course> FormationCourses { get; set; }

    }
}
