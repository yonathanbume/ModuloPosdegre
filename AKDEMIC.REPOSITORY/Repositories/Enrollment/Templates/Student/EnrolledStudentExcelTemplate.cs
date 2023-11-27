using System;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Student
{
    public class EnrolledStudentExcelTemplate
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Career { get; set; }
        public string Curriculum { get; set; }

        public string Faculty { get; set; }

        public string Sex { get; set; }

        public string Department { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public int Age { get; set; }
        public DateTime BirthDate { get; set; }

        public int AcademicYear { get; set; }

        public decimal Credits { get; set; }
        public string Document { get; set; }    
        public string AcademicProgram { get; set; }
        public Guid Id { get; set; }
    }
}
