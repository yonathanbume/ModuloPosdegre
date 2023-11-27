using System;

namespace AKDEMIC.ENTITIES.Models.Matricula
{
    public class TeachingHoursRange
    {
        public Guid Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
