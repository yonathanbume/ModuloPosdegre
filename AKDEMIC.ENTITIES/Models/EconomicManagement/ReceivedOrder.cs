using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class ReceivedOrder : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public int QuantityReceived { get; set; }
        public string UserId { get; set; }
        public Guid UserRequirementId { get; set; }
        public Guid OrderId { get; set; }
        public ApplicationUser User { get; set; }
        public UserRequirement UserRequirement { get; set; }
        public Order Order { get; set; }
    }
}
