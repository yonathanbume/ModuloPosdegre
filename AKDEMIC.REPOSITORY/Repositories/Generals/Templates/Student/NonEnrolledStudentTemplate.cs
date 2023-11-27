using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class NonEnrolledStudentTemplate
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Career { get; set; }

        public string Faculty { get; set; }

        public int AcademicYear { get; set; }

        public string Curriculum { get; set; }

        public int Try { get; set; }

        public bool HasDirectedCourse { get; set; }
    }
}
