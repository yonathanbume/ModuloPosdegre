using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Kardex
{
    public class Product : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public byte State { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public Guid ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid MeasurementUnitId { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(20)]
        public string AlternativeCode { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
    }
}
