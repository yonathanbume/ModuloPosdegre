using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.TeachingResearch
{
    public class ConvocationFile : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; }
    }
}
