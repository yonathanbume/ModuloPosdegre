using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CareerParallelCourse
    {
        public Guid Id { get; set; }

        public byte AcademicYear { get; set; }

        public byte Quantity { get; set; }

        public Guid CareerId { get; set; }

        public Career Career { get; set; }

        public bool AppliesForStudents { get; set; }

        public bool AppliesForAdmin { get; set; }
    }
}
