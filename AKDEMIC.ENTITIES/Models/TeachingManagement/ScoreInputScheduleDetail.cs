using System;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class ScoreInputScheduleDetail
    {
        public Guid Id { get; set; }

        public int NumberOfUnit { get; set; }
        public DateTime InputDate { get; set; }

        public Guid ScoreInputScheduleId { get; set; }
        public ScoreInputSchedule ScoreInputSchedule { get; set; }
    }
}