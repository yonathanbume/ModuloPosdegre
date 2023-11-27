using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Schedule : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string DayString { get; set; }
        public TimeSpan HBegin { get; set; }
        public TimeSpan HEnd { get; set; }
        public ICollection<SectionSchedule> SectionSchedule { get; set; }
    }
}
