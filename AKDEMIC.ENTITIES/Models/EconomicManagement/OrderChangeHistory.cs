using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class OrderChangeHistory : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }

        public string Description { get; set; }
        public int StatusPre { get; set; }
        public int StatusPost { get; set; }
        public string NumberFreckle { get; set; }
        public string Datetime { get; set; }
        public string NumberGuide { get; set; }

        public Order Order { get; set; }
        public ICollection<OrderChangeFileHistory> OrderChangeFiles { get; set; }
    }
}
