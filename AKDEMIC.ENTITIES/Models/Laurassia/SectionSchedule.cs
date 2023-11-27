using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class SectionSchedule : Entity, ITimestamp
    {
        public Guid ScheduleId { get; set; }
        public Guid SectionId { get; set; }
        public Schedule Schedule { get; set; }
        public Section Section { get; set; }
    }
}
