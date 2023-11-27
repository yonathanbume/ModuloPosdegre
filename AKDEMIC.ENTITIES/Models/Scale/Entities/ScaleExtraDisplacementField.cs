using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class ScaleExtraDisplacementField : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public string LaborPositionType { get; set; }

        [StringLength(100)]
        public string LaborPosition { get; set; }    

        public Guid? OriginDependencyId { get; set; }
        public Guid? DestinationDependencyId { get; set; }

        [Required]
        public Guid ScaleResolutionId { get; set; }
        public ScaleResolution ScaleResolution { get; set; }
    }
}
