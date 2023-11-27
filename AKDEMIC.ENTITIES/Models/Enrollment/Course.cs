using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.ENTITIES.Models.TeachingManagement;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class Course : Entity, IKeyNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? AcademicProgramId { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? CareerId { get; set; }
        public Guid? SpecialtyId { get; set; }
        public Guid CourseComponentId { get; set; }
        public Guid CourseTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        public decimal Credits { get; set; } = 1.0M;

        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public string BannerImage { get; set; }
        public byte PracticalHours { get; set; } = 0;
        public byte SeminarHours { get; set; } = 0;

        [StringLength(50)]
        public string ShortCode { get; set; }
        public string ShortName { get; set; }
        public byte TheoreticalHours { get; set; } = 0;
        public byte VirtualHours { get; set; } = 0;

        [NotMapped]
        public string AreaCareer => CareerId.HasValue ? Career?.Name : Area?.Name;

        [NotMapped]
        public string FullName => $"{Code} - {Name}";

        [NotMapped]
        public Guid? SyllabusId;

        [NotMapped]
        public int TotalHours => PracticalHours + TheoreticalHours + SeminarHours + VirtualHours;

        [NotMapped]
        public int EffectiveHours => PracticalHours + TheoreticalHours;
        [NotMapped]
        public int SectionsCount { get; set; }

        public AcademicProgram AcademicProgram { get; set; }
        public Specialty Specialty { get; set; }
        public Area Area { get; set; }
        public Career Career { get; set; }
        public CourseComponent CourseComponent { get; set; }
        public CourseType CourseType { get; set; }

        public ICollection<AcademicHistory> AcademicHistories { get; set; }
        public ICollection<AcademicYearCourse> AcademicYearCourses { get; set; }
        public ICollection<EvaluationReport> EvaluationReports { get; set; }
        public ICollection<CourseRecognition> CoursesRecognition { get; set; }
        public ICollection<CourseTerm> CourseTerms { get; set; }
        public ICollection<CourseSyllabus> Sylabus { get; set; }
    }
}