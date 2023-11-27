using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class EconomicActivitySection : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public ICollection<EconomicActivityDivision> EconomicActivityDivisions { get; set; }
    }
}
