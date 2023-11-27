using System;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeacherSchedule
{
    public sealed class TeacherScheduleTemplateA
    {
        public string Course { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string Section { get; set; }
        public string Classroom { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public double TeoricHours { get; set; }
        public double PracticalHours { get; set; }
        public int Students { get; set; }
        public Guid SectionId { get; set; }
        public string TeacherName { get;  set; }
        public string Subject { get;  set; }
        public byte Cycle { get;  set; }
        public string Schedule { get; set; }
        public string CourseTermModality { get; set; }
        public string AcademicYear { get; set; }
    }
}