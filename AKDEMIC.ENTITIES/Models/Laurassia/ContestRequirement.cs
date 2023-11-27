using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class ContestRequirement : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ContestId { get; set; }
        public Contest Contest { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
