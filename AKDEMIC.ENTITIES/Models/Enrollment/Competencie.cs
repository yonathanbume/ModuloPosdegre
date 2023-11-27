using AKDEMIC.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class Competencie
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte Type { get; set; } = ConstantHelpers.COMPETENCIE.GENERAL;

        public ICollection<CurriculumCompetencie> CurriculumCompetencies { get; set; }
        public ICollection<AcademicYearCourse> AcademicYearCourses { get; set; }
    }
}
