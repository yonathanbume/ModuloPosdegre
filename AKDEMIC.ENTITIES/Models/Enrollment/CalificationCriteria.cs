using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CalificationCriteria : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public decimal Score { get; set; }
    }
}
