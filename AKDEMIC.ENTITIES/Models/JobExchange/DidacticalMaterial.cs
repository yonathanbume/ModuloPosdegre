using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class DidacticalMaterial : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(350)]
        public string Title { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }

        public ICollection<DidacticalMaterialFile> DidacticalMaterialFiles { get; set; }
    }
}
