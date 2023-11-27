using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class ReceivedOrderHistory : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid ReceivedOrderId { get; set; }
        public int QuantityReceivedPrev { get; set; }
        public int QuantityReceivedPost { get; set; }
        public string Observation { get; set; }
        public ApplicationUser User { get; set; }
        public ReceivedOrder ReceivedOrder { get; set; }
    }
}
