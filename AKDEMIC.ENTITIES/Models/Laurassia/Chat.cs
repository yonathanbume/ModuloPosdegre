using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Chat : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string EmisorId { get; set; }
        public string ReceptorId { get; set; }
        public Guid SectionId { get; set; }
        public ApplicationUser Emisor { get; set; }
        public ApplicationUser Receptor { get; set; }
        public Section Section { get; set; }
        public ICollection<Message> Mensaje { get; set; }
    }
}
