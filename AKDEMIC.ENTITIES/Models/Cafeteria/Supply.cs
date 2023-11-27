using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class Supply : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }        
        public string ClassifierCode { get; set; }
        public string ExternalCode { get; set; }
        public string Name { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public string Utility { get; set; }
        //public byte CoinType { get; set; }
        public int Year { get; set; }
        public DateTime? HighYear { get; set; }
        public DateTime? LowYear { get; set; }
        public decimal Discount { get; set; }
        public Guid UnitMeasurementId { get; set; }
        //public byte Category { get; set; }
        public string Description { get; set; }
        public Guid SupplyPackageId { get; set; }
        public UnitMeasurement UnitMeasurement { get; set; }
        public SupplyPackage SupplyPackage { get; set; }       
        
    }
}
