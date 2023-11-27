using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.ComputerCenter
{
    public class ComGroup : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ComCourseId { get; set; }
        public ComCourse ComCourse { get; set; }
        public string Code { get; set; }
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public ICollection<ComClassSchedule> ComClassSchedules { get; set; }
    }
}
