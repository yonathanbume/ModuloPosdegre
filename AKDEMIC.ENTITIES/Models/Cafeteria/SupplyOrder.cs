using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class SupplyOrder : Entity, ISoftDelete, ITimestamp, ICodeNumber
    {
        public Guid Id { get; set; }

        //public Guid ProviderId { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public string OwnerId { get; set; }
        public DateTime? Date { get; set; }
        public bool State { get; set; }
        [NotMapped]
        public string Code => $"SOL -{GeneratedId.ToString("D5")}";
        //public Provider Provider { get; set; }
        public ApplicationUser Owner { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public ICollection<SupplyOrderDetail> SupplyOrderDetail { get; set; }
    }
}
