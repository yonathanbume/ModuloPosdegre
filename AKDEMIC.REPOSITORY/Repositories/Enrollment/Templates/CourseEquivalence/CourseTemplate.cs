using System;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseEquivalence
{
    public class CourseTemplate
    {
        public Guid Id { get; set; }

        public decimal Credits { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int AcademicYear { get; set; }

        public decimal OldCredits { get; set; }

        public string OldCode { get; set; }

        public string OldName { get; set; }

        public int OldAcademicYear { get; set; }

        public Guid EquivalenceId { get; set; }

        public Guid AcademicProgramId { get; set; }
        public string AcademicProgram { get; set; }

        public bool Duplicated { get; set; } = false;

        public bool Replace { get; set; } = false;
    }
}
