using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkerLaborRegime : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public string LegalRegulation { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public DateTime PublicationDateRegulation { get; set; }

        [Required]
        public byte Status { get; set; }

        public ICollection<WorkerLaborInformation> WorkerLaborInformation { get; set; }
        public ICollection<WorkerLaborCondition> WorkerLaborConditions { get; set; }
        public ICollection<WorkerLaborCategory> WorkerLaborCategories { get; set; }
    }
}
