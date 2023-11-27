using System;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class StudentSectionTemplate
    {
        public Guid SectionId { get; set; }
        public string CourseName { get; set; }
        public int ClassCount { get; set; }
        public int MaxAbsences { get; set; }
        public int Dictated { get; set; }
        public int Assisted { get; set; }
        public int Absences { get; set; }
    }
    public class ReportCourseDetailTemplate
    {
        public string Fullname { get; set; }
        public int Absences { get; set; }
        public int Assisted { get; set; }
        public int MaxAbsences { get; set; }
        public int Dictated { get; set; }
    }
}
