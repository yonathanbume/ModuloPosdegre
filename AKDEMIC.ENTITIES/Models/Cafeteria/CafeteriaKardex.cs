using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class CafeteriaKardex : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ProviderSupplyId { get; set; }
        public int Type { get; set; }
        public decimal Quantity { get; set; }
        
        public ProviderSupply ProviderSupply { get; set; }

        [NotMapped]
        public string FormmatedType => (Type == 1 ? "Entrada" : "Salida");        
    }
}
