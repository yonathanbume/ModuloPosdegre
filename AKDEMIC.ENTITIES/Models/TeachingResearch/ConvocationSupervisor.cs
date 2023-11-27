using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.TeachingResearch
{
    public class ConvocationSupervisor
    {
        [Key]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Key]
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; }
    }
}
