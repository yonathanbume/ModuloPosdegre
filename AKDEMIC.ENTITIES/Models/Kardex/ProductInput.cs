using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Kardex
{
    public class ProductInput : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        [Required]
        public DateTime RegisterAt { get; set; }
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int Existence { get; set; }
        public float UnitCost { get; set; }
        public string PurchaseOrder { get; set; }
        //another table
        public string Voucher { get; set; }
    }
}
