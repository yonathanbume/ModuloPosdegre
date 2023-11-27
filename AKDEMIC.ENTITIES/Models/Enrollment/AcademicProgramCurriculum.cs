using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class AcademicProgramCurriculum
    {
        public Guid AcademicProgramId { get; set; }
        public AcademicProgram AcademicProgram { get; set; }

        public Guid CurriculumId { get; set; }
        public Curriculum Curriculum { get; set; }
    }
}