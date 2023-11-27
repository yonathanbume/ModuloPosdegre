using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class ScaleSectionResolutionType : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        public byte Status { get; set; }

        [Required]
        public Guid ScaleResolutionTypeId { get; set; }
        public ScaleResolutionType ScaleResolutionType { get; set; }

        [Required]
        public Guid ScaleSectionId { get; set; }
        public ScaleSection ScaleSection { get; set; }
    }
}
