using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CourseEquivalence : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid NewAcademicYearCourseId { get; set; }
        public Guid OldAcademicYearCourseId { get; set; }
        
        public string Comment { get; set; } = "";
        public bool IsOptional { get; set; }
        public bool ReplaceGrade { get; set; }
        
        public AcademicYearCourse NewAcademicYearCourse { get; set; }
        
        [InverseProperty("CourseEquivalences")]
        public AcademicYearCourse OldAcademicYearCourse { get; set; }
    }
}
