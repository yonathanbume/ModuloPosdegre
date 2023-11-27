using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class ScaleExtraMeritField : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public DateTime? MeritDate { get; set; }
        
        public decimal? MeritAmount { get; set; }

        public Guid? DependencyId { get; set; }

        [Required]
        public Guid ScaleResolutionId { get; set; }
        public ScaleResolution ScaleResolution { get; set; }
    }
}
