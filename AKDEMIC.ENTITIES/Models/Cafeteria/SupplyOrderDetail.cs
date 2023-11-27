using System;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class SupplyOrderDetail
    {
        public Guid Id { get; set; }
        public string SupplyName { get; set; }
        public Guid ProviderSupplyId { get; set; }
        public Guid SupplyOrderId { get; set; }
        public bool State { get; set; } = false;

        public ProviderSupply ProviderSupply { get; set; }
        public SupplyOrder SupplyOrder { get; set; }
        public decimal Quantity { get; set; }
        public string Commentary { get; set; }
        public byte TurnId { get; set; }
    }
}
