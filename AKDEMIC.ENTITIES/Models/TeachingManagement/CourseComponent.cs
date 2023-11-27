using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class CourseComponent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int QuantityOfUnits { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}