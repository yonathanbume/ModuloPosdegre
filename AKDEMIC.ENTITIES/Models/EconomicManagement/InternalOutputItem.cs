using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class InternalOutputItem : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public Guid CatalogItemId { get; set; }
        public Guid InternalOutputId { get; set; }
        public CatalogItem CatalogItem { get; set; }
        public InternalOutput InternalOutput { get; set; }

    }
}
