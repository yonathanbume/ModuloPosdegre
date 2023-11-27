using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class ScaleExtraBenefitField : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid BenefitTypeId { get; set; }
        public BenefitType BenefitType { get; set; }

        [StringLength(100)]
        public string Quinquennium { get; set; }
        
        public DateTime? AccomplishmentQuinquenniumDate { get; set; }

        [StringLength(100)]
        public string BenefitGranted { get; set; }

        public decimal? AmountGranted { get; set; }
        
        [Required]
        public Guid ScaleResolutionId { get; set; }
        public ScaleResolution ScaleResolution { get; set; }
    }
}
