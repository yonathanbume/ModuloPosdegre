using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkerLaborCondition : Entity, IKeyNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid WorkerLaborRegimeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public byte Status { get; set; }
        
        public WorkerLaborRegime WorkerLaborRegime { get; set; }
    }
}
