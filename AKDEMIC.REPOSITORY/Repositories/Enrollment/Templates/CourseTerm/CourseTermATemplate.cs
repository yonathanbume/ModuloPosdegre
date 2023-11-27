using System;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseTerm
{
    public sealed class CourseTermATemplate
    {
        public Guid CourseId { get; set; }
        public Guid TermId { get; set; }
    }
    public class CourseTermDataTemplate
    {
        public Guid Idcourseterm { get; set; }
        public Guid Idterm { get; set; }
        public Guid Idcourse { get; set; }
        public string Code { get; set; }
        public string SectionCode { get; set; }
        public string Name { get; set; }
    }
}