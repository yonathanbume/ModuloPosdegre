using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Laurassia;
using AKDEMIC.ENTITIES.Models.TeachingManagement;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class Class
    {
        public Guid Id { get; set; }
        public Guid ClassroomId { get; set; }
        public Guid ClassScheduleId { get; set; }
        public Guid SectionId { get; set; }
        public Guid? UnitActivityId { get; set; }
        public DateTime? DictatedDate { get; set; }
        public Guid? VirtualClassId { get; set; }

        public int ClassNumber { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsDictated { get; set; } = false;
        public bool IsRescheduled { get; set; } = false;
        //Para los días Feriados
        public bool NeedReschedule { get; set; } = false;
        public DateTime StartTime { get; set; }
        public int WeekNumber { get; set; }
        public string Commentary { get; set; }
        public Guid? EvaluationId { get; set; }
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public VirtualClass VirtualClass { get; set; }
        public Classroom Classroom { get; set; }
        public ClassSchedule ClassSchedule { get; set; }
        public Section Section { get; set; }
        public UnitActivity UnitActivity { get; set; }
        public Enrollment.Evaluation Evaluation { get; set; }
        public ICollection<ClassStudent> ClassStudents { get; set; }

        [NotMapped]
        public string FormattedStart => StartTime.ToLocalDateFormat();
        [NotMapped]
        public string FormattedEnd => EndTime.ToLocalDateFormat();
    }
}
