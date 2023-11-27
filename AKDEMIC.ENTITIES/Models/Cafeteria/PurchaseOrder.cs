using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class PurchaseOrder : Entity, ISoftDelete, ITimestamp, ICodeNumber
    {
        public Guid Id { get; set; }

        public Guid ProviderId { get; set; }
        public string OwnerId { get; set; }
        public DateTime? AcceptanceDate { get; set; }
        public bool State { get; set; }
        public Provider Provider { get; set; }
        public int Number { get; set; }
        public int SelectionProcess { get; set; }
        public decimal Price { get; set; }
        public decimal IGV { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [NotMapped]
        public string Code => $"ORD -{Number.ToString("D5")}";
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    }
}
