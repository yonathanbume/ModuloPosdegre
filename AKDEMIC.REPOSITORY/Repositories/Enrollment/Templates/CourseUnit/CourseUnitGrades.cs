using System;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseUnit
{
    public class CourseUnitGrades
    {
        public Guid CourseUnitId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public int? Average { get; set; }
    }
}
