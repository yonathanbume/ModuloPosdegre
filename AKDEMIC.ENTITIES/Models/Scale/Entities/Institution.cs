using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class Institution : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsTop1000 { get; set; }

        public byte InstitutionType { get; set; }
    }
}
