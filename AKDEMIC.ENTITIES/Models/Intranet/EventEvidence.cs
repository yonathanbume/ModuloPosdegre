using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class EventEvidence : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public bool IsPicture { get; set; } //Picture or Video

        public string FilePath { get; set; }

        public Guid EventId { get; set; }
        public Event Event { get; set; }
    }
}
