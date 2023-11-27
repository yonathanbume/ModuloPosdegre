using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class PurchaseOrderDetail: Entity, ICodeNumber
    {
        
        public Guid Id { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public Guid ProviderSupplyId { get; set; }        

        public bool State { get; set; } = false;
        public decimal Quantity { get; set; }
        public int Item { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }
        public ProviderSupply ProviderSupply { get; set; } 
    }
}
