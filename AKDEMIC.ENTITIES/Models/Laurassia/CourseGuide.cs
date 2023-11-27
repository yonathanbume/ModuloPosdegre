using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class CourseGuide : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
