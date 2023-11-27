using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class StudentActivityFile : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public StudentActivity StudentActivity { get; set; }
        public Guid StudentActivityId { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
    }
}
