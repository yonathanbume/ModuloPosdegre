using System;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class TeacherSchedule
    {
        public Guid Id { get; set; }
        public Guid ClassScheduleId { get; set; }
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public ClassSchedule ClassSchedule { get; set; }
    }
}