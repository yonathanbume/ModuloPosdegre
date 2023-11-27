using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Laurassia;
using AKDEMIC.ENTITIES.Models.TeachingManagement;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CourseUnit : Entity, IKeyNumber, ITimestamp, ISoftDelete
    {
        public Guid Id { get; set; }
        public Guid CourseSyllabusId { get; set; }

        public int AcademicProgressPercentage { get; set; }
        public string EssentialKnowledge { get; set; }
        public DateTime? GradeEntryDate { get; set; }
        public string LearningAchievements { get; set; }

        [Required]
        public string Name { get; set; }
        public byte Number { get; set; }
        public string PerformanceCriterion { get; set; }
        public string PerformanceEvidence { get; set; }
        public string Techniques { get; set; }
        public string Tools { get; set; }
        public int WeekNumberEnd { get; set; }
        public int WeekNumberStart { get; set; }
        public int Weighing { get; set; }
        public decimal VirtualHours { get; set; }
        public CourseSyllabus CourseSyllabus { get; set; }
        public ICollection<Evaluation> Evaluations { get; set; }
        public ICollection<UnitActivity> UnitActivities { get; set; }
        public ICollection<UnitResource> UnitResources { get; set; }
        public ICollection<Content> Contents { get; set; }

        [NotMapped]
        public string GradeEntryDateFormatted => GradeEntryDate.HasValue ? GradeEntryDate.ToLocalDateFormat() : null;

        [NotMapped]
        public int TotalWeeks => WeekNumberStart - (WeekNumberEnd == 0 ? 0 : (WeekNumberEnd - 1));
    }
}