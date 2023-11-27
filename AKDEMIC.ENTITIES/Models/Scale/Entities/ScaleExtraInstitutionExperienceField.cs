using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class ScaleExtraInstitutionExperienceField : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public string LaborPositionType { get; set; }

        [StringLength(100)]
        public string LaborPosition { get; set; }

        [StringLength(100)]
        public string Level { get; set; }

        [StringLength(300)]
        public string CessationDescription { get; set; }

        //oficina/dependencia por requerimiento se remplazo por un campo de texto
        public string Office { get; set; }

        [Required]
        public Guid ScaleResolutionId { get; set; }
        public ScaleResolution ScaleResolution { get; set; }
    }
}
