using System;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class UnitResource
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Week { get; set; }

        public Guid CourseUnitId { get; set; }

        public CourseUnit CourseUnit { get; set; }
    }
}
