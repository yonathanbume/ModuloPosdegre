using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class ScaleLicenseAuthorization : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid LicenseResolutionTypeId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime BeginDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DateTime ExpeditionDate { get; set; }

        [Required]
        public byte DocumentType { get; set; }

        [Required]
        public bool IsRemunerated { get; set; }

        [StringLength(100)]
        public string IssueAgency { get; set; }

        [StringLength(500)]
        public string Observation { get; set; }

        [Required]
        [StringLength(255)]
        public string Reason { get; set; }

        [Required]
        public string ResolutionDocument { get; set; }

        [Required]
        [StringLength(50)]
        public string ResolutionNumber { get; set; }

        [NotMapped]
        public string ExpeditionFormattedDate { get; set; }

        [NotMapped]
        public string FormattedBeginDate { get; set; }

        [NotMapped]
        public string FormattedEndDate { get; set; }
        
        public ApplicationUser User { get; set; }
        public LicenseResolutionType LicenseResolutionType { get; set; }
    }
}
