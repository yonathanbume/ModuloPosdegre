using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class EnrolledCourseTemplate
    {
        public Guid StudentSectionId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
    }
}
