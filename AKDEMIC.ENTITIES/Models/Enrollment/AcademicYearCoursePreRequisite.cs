using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class AcademicYearCoursePreRequisite : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid AcademicYearCourseId { get; set; }
        public Guid CourseId { get; set; }

        public bool IsOptional { get; set; }

        public AcademicYearCourse AcademicYearCourse { get; set; }
        public Course Course { get; set; }
    }
}
