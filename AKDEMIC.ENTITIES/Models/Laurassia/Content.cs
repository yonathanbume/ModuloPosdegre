using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Content : Entity, ISoftDelete
    {
        public Guid Id { get; set; }
        public Guid CourseUnitId { get; set; }
        public CourseUnit CourseUnit { get; set; }
        public Guid SectionId { get; set; }
        public Section Section { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Index { get; set; }
        public ICollection<VForum> VForums { get; set; }
        public ICollection<Reading> Readings { get; set; }
        public ICollection<Resource> Resources { get; set; }
        public ICollection<Homework> Homeworks { get; set; }
        public ICollection<VExam> VExams { get; set; }
        public ICollection<VirtualClass> VirtualClasses { get; set; }
    }
}
