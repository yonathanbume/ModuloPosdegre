using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class StudentUserProcedure
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        public byte ActivityType { get; set; }

        public Guid? CareerId { get; set; }
        public Guid? StudentSectionId { get; set; }
        public Guid? AcademicProgramId { get; set; }
        public Guid? CurriculumId { get; set; }
        public Guid? TermId { get; set; }
        public Guid? CourseId { get; set; }

        public Student Student { get; set; }

        public Course Course { get; set; }
        public Career Career { get; set; }
        public StudentSection StudentSection { get; set; }
        public AcademicProgram AcademicProgram { get; set; }
        public Curriculum Curriculum { get; set; }
        public Term Term { get; set; }

        public ICollection<StudentUserProcedureDetail> StudentUserProcedureDetails { get; set; }
    }
}
