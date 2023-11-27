using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.AcademicExchange
{
    public class AeGeneralFile : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public DateTime? PublicationDate { get; set; }

        public byte Status { get; set; }

        public byte Number { get; set; }
    }
}
