using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.TeacherSection
{
    public sealed class TeacherSectionTemplateC
    {
        public string Teacher { get; set; }
        public string Category { get; set; }
        public string Grade { get; set; }
        public string Condition { get; set; }
        public string Course { get; set; }
        public string CourseArea { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public decimal Credits{ get; set; }
        public string AcademicDepartment { get; set; }
        public string Cycle { get; set; }
        public double HT { get; set; }
        public double HP { get; set; }
        public double HS { get; set; }
        public double HV { get; set; }
        public string Group { get; set; }
        public double TotalHours { get; set; }
        public string Turn { get; set; }
        public string Section { get; set; }
        public int ClassRoom { get; set; }
        public string ClassRoomName { get; set; }
        public int NumberStudents { get; set; }
        public byte AcademicYear { get; set; }
        public string CareerCode { get; set; }
        public List<SessionTypesTemplate> SessionTypes { get; set; }

        //public TimeSpan StartTime { get; set; }
        //public TimeSpan EndTime { get; set; }
        //public int WeekDay { get; set; }
    }
    public class SessionTypesTemplate
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int WeekDay { get; set; }
        public int SessionType { get; set; }

        public double Duration { get; set; }
    }
}