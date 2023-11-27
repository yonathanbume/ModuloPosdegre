using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.ComputerCenter
{
    public class ComClassSchedule : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ComGroupId { get; set; }
        public ComGroup ComGroup { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; } 
        public int WeekDay { get; set; }

    }
}
