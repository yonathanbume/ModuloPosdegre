using System;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Course
{
    public sealed class CourseBTemplate
    {
        public Guid CourseTermId { get; set; }
        public string FullName { get; set; }
    }
    public class EditTemplate
    {
        public string Career { get; set; }

        public string Course { get; set; }

        public Guid CourseId { get; set; }
        public string AcademicProgram { get; set; }
    }
}