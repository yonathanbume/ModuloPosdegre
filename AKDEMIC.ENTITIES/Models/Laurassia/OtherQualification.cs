using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class OtherQualification : Entity, ITimestamp
    {
        public Guid SectionId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CourseUnitId { get; set; }
        public CourseUnit CourseUnit { get; set; }
        public Section Section { get; set; }
    }
}
