using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Models.Generals;


namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class TeacherAssistance
    {
        [Key]
        public Guid TeacherAssitanceId { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int Status { get; set; } = 0;

        public DateTime Time { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
