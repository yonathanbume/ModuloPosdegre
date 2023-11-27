using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class CurrentAccount : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
