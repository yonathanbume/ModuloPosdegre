using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Reading : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public bool Show { get; set; } = true;
        public string Url { get; set; }
        public Guid ContentId { get; set; }
        public Content Content { get; set; }
    }
}
