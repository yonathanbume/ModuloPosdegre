using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class RubricItemDetail : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid RubricItemId { get; set; }
        public RubricItem RubricItem { get; set; }
        public decimal Score { get; set; }
    }
}
