using System;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Course
{
    public sealed class CourseATemplate
    {
        public Guid Ctid { get; set; }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid CourseTypeId { get; set; }
        public string AreaCareer { get; set; }
        public string Type { get; set; }
        public Guid? CareerId { get; set; }
    }
}