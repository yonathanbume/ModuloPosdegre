using System;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public sealed class SectionCode
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}