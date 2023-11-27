using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class ScaleExtraInvestigationField : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public string WorkType { get; set; }

        public Guid InvestigationParticipationTypeId { get; set; }
        public InvestigationParticipationType InvestigationParticipationType { get; set; }

        [StringLength(300)]
        public string WorkTitle { get; set; }

        [StringLength(300)]
        public string InstitutionPublication { get; set; }

        public DateTime? PublicationDate { get; set; }

        [StringLength(500)]
        public string WorkUrl { get; set; }

        [Required]
        public Guid ScaleResolutionId { get; set; }
        public ScaleResolution ScaleResolution { get; set; }
    }
}
