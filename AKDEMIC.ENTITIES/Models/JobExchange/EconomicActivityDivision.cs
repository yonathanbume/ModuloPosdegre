using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    // Actividad Económica (División) - SUNAT
    public class EconomicActivityDivision : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public Guid EconomicActivitySectionId { get; set; }

        public EconomicActivitySection EconomicActivitySection { get; set; }
        public ICollection<EconomicActivity> EconomicActivities { get; set; }
    }
}
