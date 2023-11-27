using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ExtracurricularCourse : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(400)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Credits { get; set; }

        public Guid ExtracurricularAreaId { get; set; }
        public ExtracurricularArea ExtracurricularArea { get; set; }
    }
}
