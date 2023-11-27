using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class Dean : Entity, ISoftDelete, ITimestamp
    {
        [Key]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public Faculty Faculty { get; set; }

        public Guid FacultyId { get; set; }
    }
}
