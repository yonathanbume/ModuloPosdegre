using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutoringCoordinator
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid CareerId { get; set; }

        public Career Career { get; set; }

    }
}
