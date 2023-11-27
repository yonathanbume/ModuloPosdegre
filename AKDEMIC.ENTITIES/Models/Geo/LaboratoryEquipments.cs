using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Geo
{
    public class LaboratoryEquipments
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Serie { get; set; }

        public string HeritageCode { get; set; }

        public ICollection<LaboratoryEquipmentLoans> LaboratoryEquipmentLoans { get; set; }
    }
}
