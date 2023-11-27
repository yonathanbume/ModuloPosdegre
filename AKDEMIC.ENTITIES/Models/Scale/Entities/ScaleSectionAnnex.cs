using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class ScaleSectionAnnex : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(50)]
        public string DocumentNumber { get; set; }

        [StringLength(100)]
        public string IssueAgency { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        public string Type { get; set; }
        public string Condition { get; set; }

        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ExpeditionDate { get; set; }

        [NotMapped]
        public string ExpeditionFormattedDate { get; set; }

        [Required]
        public string AnnexDocument { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public Guid ScaleSectionId { get; set; }
        public ScaleSection ScaleSection { get; set; }
    }
}
