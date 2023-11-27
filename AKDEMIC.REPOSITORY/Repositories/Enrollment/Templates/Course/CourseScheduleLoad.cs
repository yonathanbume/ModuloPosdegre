using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Course
{
    public class CourseScheduleLoad
    {
        public string Curriculum { get; set; }
        public Guid? CurriculumId { get; set; }
        public string CourseCode { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
        public int EnrolledStudents { get; set; }
        public string AcademicYear { get; set; }
        public int AcademicYearNumber { get; set; }
        public int TheoreticalHours { get; set; }
        public int VirtualHours { get; set; }
        public int PracticalHours { get; set; }
        public int SeminarHours { get; set; }

        public double AssignedTheoreticalHours { get; set; }
        public double AssignedVirtualHours { get; set; }
        public double AssignedPracticalHours { get; set; }
        public double AssignedSeminarHours { get; set; }

        public List<CourseScheduleLoadDetail> Details { get; set; }
    }

    public class CourseScheduleLoadDetail
    {
        public int WeekDay { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ScheduleFormat => $"{StartTime}-{EndTime}";
    }
}
