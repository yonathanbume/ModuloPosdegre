using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.TeachingManagement;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class ScaleExtraPerformanceEvaluationField : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public string EvaluationType { get; set; }
        
        public decimal? BaseScore { get; set; }

        public decimal? Qualification { get; set; }
        public Guid? TermId { get; set; }

        [Required]
        public Guid ScaleResolutionId { get; set; }
        public ScaleResolution ScaleResolution { get; set; }
        public Term Term { get; set; }

        public Guid? PerformanceEvaluationId { get; set; }
        public PerformanceEvaluation PerformanceEvaluation { get; set; }
    }
}
