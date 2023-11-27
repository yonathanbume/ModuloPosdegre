using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using Section = AKDEMIC.ENTITIES.Models.Enrollment.Section;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ClassSchedule : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ClassroomId { get; set; }
        public Guid SectionId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int WeekDay { get; set; }
        public int SessionType { get; set; } = 1; //1 teorico - 2 practico - 3 virtual - 4 seminario
        public bool HasBeenRescheduled { get; set; }
        public Guid? SectionGroupId { get; set; }
        public SectionGroup SectionGroup { get; set; }
        public Classroom Classroom { get; set; }
        public Section Section { get; set; }
        public ICollection<Class> Classes { get; set; }
        public IEnumerable<TeacherSchedule> TeacherSchedules { get; set; }

        [NotMapped]
        public string StartTimeText => StartTime.ToLocalDateTimeFormatUtc();
        [NotMapped]
        public string EndTimeText => EndTime.ToLocalDateTimeFormatUtc();
        [NotMapped]
        public IEnumerable<string> TeacherNames { get; set; }

    }
}
