using System;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class CourseEvaluationTemplate
    {
        public Guid EvaluationId { get; set; }
        public string Name { get; set; }
        public int Percentage { get; set; }
        public bool Attended { get; set; }
        public bool Approved { get; set; }
        public decimal Grade { get; set; }
        public bool Taked { get; set; } = false;
    }
}
