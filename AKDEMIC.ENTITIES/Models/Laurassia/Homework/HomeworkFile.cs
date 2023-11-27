using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class HomeworkFile : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid HomeworkId { get; set; }
        public Homework Homework { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
