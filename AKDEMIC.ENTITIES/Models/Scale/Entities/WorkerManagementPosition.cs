using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkerManagementPosition : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public byte Status { get; set; }

        public Guid DependencyId { get; set; }
        public Dependency Dependency { get; set; }
    }
}
