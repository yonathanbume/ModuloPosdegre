using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Kardex
{
    public class ProductRequest : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public byte State { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public Guid DependencyId { get; set; }
        public Dependency Dependency { get; set; }
    }
}
