using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class DidacticalMaterialFile : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public string FilePath { get; set; }

        public Guid DidacticalMaterialId { get; set; }

        public DidacticalMaterial DidacticalMaterial { get; set; }
    }
}
