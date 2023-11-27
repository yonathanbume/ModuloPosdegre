using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ClassReschedule : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid ClassId { get; set; }
        public string UserId { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }
        public string Justification { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }
        public int Status { get; set; } = 1;

        public bool Replicate { get; set; }

        public ApplicationUser User { get; set; }
        public Class Class { get; set; }
    }
}
