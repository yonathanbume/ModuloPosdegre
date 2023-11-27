using System;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class CafeteriaStock
    {
        public Guid Id { get; set; }
        public decimal Quantity { get; set; } = 0;
        public Guid ProviderSupplyId { get; set; }
        public byte TurnId { get; set; }
        public ProviderSupply ProviderSupply { get; set; }
    }
}


