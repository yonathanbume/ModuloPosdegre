using System;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class MenuPlateSupply
    {
        public Guid Id { get; set; }
        public Guid MenuPlateId { get; set; }
        public Guid ProviderSupplyId { get; set; }
        public decimal Quantity { get; set; }
        public byte TurnId { get; set; }
        public MenuPlate MenuPlate { get; set; }        
        public ProviderSupply ProviderSupply { get; set; }
    }
}
