using AKDEMIC.CORE.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class NonTeachingLoadSchedule
    {
        public Guid Id { get; set; }
        public Guid NonTeachingLoadId { get; set; }
        public NonTeachingLoad NonTeachingLoad { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int WeekDay { get; set; }

        [NotMapped]
        public TimeSpan StartTimeLocal => StartTime.ToLocalTimeSpanUtc();
        [NotMapped]
        public TimeSpan EndTimeLocal => EndTime.ToLocalTimeSpanUtc();
    }
}
