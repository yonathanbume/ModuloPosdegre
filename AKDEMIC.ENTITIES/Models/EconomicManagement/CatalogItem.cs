using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class CatalogItem : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int CodeUnitMeasurement { get; set; }
        public string UnitMeasurement { get; set; }
        public byte Type { get; set; }
        public ICollection<UserRequirementItem> UserRequirementItems { get; set; }
        public ICollection<InternalOutputItem> InternalOutputItems { get; set; }
    }
}
