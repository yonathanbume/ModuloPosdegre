using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class ScaleResolution : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ScaleSectionResolutionTypeId { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        public byte DocumentType { get; set; }

        [Required]
        public DateTime ExpeditionDate { get; set; }

        [StringLength(100)]
        public string IssueAgency { get; set; }

        [StringLength(500)]
        public string Observation { get; set; }
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
        public ScaleSectionResolutionType ScaleSectionResolutionType { get; set; }
        public List<ScaleExtraPerformanceEvaluationField> ScaleExtraPerformanceEvaluationFields { get; set; }
    }
}
