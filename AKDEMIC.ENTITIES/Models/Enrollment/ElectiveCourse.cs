using System;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class ElectiveCourse
    {
        public Guid Id { get; set; }

        public byte AcademicYearNumber { get; set; }

        public bool Active { get; set; } = true;

        public Guid CareerId { get; set; }

        public Guid CourseId { get; set; }

        public Career Career { get; set; }

        public Course Course { get; set; }
    }
}
