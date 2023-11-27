using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class EventCertification : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(350)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Content { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
