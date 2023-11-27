using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class StudentObservation : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid? TermId { get; set; }

        public string UserId { get; set; }

        [StringLength(3000)]
        public string Observation { get; set; }
        public string File { get; set; }
        public byte Type { get; set; }

        [NotMapped]
        public string CreatedFormattedDate { get; set; }

        public ApplicationUser User { get; set; }
        public Student Student { get; set; }
        public Term Term { get; set; }
    }
}
