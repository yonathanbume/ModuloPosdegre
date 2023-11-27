using AKDEMIC.CORE.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeacherSchedule
{
    public class TeacherScheduleReportTemplate
    {
        public Guid Id { get; set; }
        public string AcademicDepartment { get; set; }
        public string TeacherFullName { get; set; }
        public string TeacherUserName { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public string Curriculum { get; set; }
        public int AcademicYear { get; set; }
        public string Section { get; set; }
        public string WeekDay { get; set; }
        public TimeSpan StartTimeUTC { get; set; }
        public TimeSpan EndTimeUTC { get; set; }
        public string StartTimeLocalFormat => StartTimeUTC.ToLocalDateTimeFormatUtc();
        public string EndTimeLocalFormat => EndTimeUTC.ToLocalDateTimeFormatUtc();
        public string Classroom { get; set; }
        public string ScheduleSessionType { get; set; }
        public int Enrolled { get; set; }
    }
}
