using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Geo
{
    public class LaboratoryStudentAssitance
    {
        [Key]
        public Guid Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public byte Status { get; set; } = 0;

        public DateTime Time { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Guid LaboratoyRequestId { get; set; }

        public LaboratoyRequest LaboratoyRequest { get; set; }
    }
}
