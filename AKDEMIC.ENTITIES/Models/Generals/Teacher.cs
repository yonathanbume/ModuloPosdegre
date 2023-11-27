using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.ENTITIES.Models.TeachingManagement;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class Teacher : Entity, IKeyNumber, ISoftDelete, ITimestamp
    {
        [Key]
        public string UserId { get; set; }
        public Guid? AcademicDepartmentId { get; set; }
        public Guid? CareerId { get; set; }
        //public Guid? FacultyId { get; set; }
        public Guid? TeacherDedicationId { get; set; }
        public Guid? TeacherInformationId { get; set; }

        public byte Status { get; set; } = 1;
        public int? MoodleId { get; set; }

        [NotMapped]
        public double ValidatedHours { get; set; }

        public AcademicDepartment AcademicDepartment { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public Career Career { get; set; }
        //public Faculty Faculty { get; set; }
        public TeacherDedication TeacherDedication { get; set; }
        public virtual TeacherInformation TeacherInformation { get; set; }

        public virtual IEnumerable<NonTeachingLoad> NonTeachingLoads { get; set; }
        public virtual IEnumerable<TeacherSection> TeacherSections { get; set; }
        public virtual IEnumerable<TeacherSchedule> TeacherSchedules { get; set; }
        public virtual IEnumerable<PerformanceEvaluationUser> PerformanceEvaluationUsers { get; set; }
    }
}
