using AKDEMIC.CORE.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Reservations
{
    public class EnvironmentSchedule
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EnvironmentId { get; set; }

        [ForeignKey("EnvironmentId")]
        public virtual Environment Environment { get; set; }
        public int WeekDay { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        [NotMapped] public string StartTimeText => StartTime.ToLocalDateTimeFormatUtc();
        [NotMapped] public string EndTimeText => EndTime.ToLocalDateTimeFormatUtc();
    }
}
