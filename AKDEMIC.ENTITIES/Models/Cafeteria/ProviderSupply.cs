using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class ProviderSupply : Entity, ICodeNumber
    {
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }
        public Guid SupplyId { get; set; }
        public Provider Provider { get; set; }
        public Supply Supply { get; set; }
    }
}
