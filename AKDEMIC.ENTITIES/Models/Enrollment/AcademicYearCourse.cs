using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class AcademicYearCourse : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid CurriculumId { get; set; }

        public byte AcademicYear { get; set; }
        public bool IsElective { get; set; }
        public bool IsExonerable { get; set; }
        public decimal RequiredCredits { get; set; } = 0;
        public Guid? CompetencieId { get; set; }
        public Competencie Competencie { get; set; }
        public Course Course { get; set; }
        public Curriculum Curriculum { get; set; }
        public ICollection<CourseEquivalence> CourseEquivalences { get; set; }
        public ICollection<AcademicYearCoursePreRequisite> PreRequisites { get; set; }
        public ICollection<AcademicYearCourseCertificate> AcademicYearCourseCertificates { get; set; }
    }
}
