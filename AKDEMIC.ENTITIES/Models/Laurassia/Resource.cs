using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Resource : Entity, ITimestamp, ISoftDelete
    {
        public Guid Id { get; set; }
        public Guid ContentId { get; set; }
        public Content Content { get; set; }
        public string Name { get; set; }
        public byte Type { get; set; }
        public string Url { get; set; }
        public bool Show { get; set; } = true;
    }
}
