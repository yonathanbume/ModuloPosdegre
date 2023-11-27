using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.ClassScheduleViewModels
{
    public class AcademicChargeViewModel
    {
        public string HeaderText { get; set; }
        public int academicYear { get; set; }
        public string cycle { get; set; }
        public string term { get; set; }
        public string AuthoritySignature { get; set; }
        public string condition { get; set; }
        public string category { get; set; }
        public string fullName { get; set; }
        public string teachercode { get; set; }
        public string academicDepartment { get; set; }
        public string dedication { get; set; }
        public string AcademicDepartmentDirector { get; set; }
        public string DeanFaculty { get; set; }
        public string facultyTeacher { get; set; }
        public string careerTeacher { get; set; }
        public string teacherName { get; set; }
        public string totalHorasLectivas { get; set; }
        public string totalHorasNoLectivas { get; set; }
        public decimal totalHoras { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public string Observation { get; set; }
        public string TeacherTitle { get; set; }
        public string HighestAcademicDegree { get; set; }
        public string HighestAcademicDegreeDescription { get; set; }
        public List<(TimeSpan, TimeSpan, string)> ScheduleParameters { get; set; }
        public List<SectionViewModel> Sections { get; set; } = new List<SectionViewModel>();
        public List<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>();
        public List<NonActivityViewModel> NonActivities { get; set; } = new List<NonActivityViewModel>();
        public List<Report3Row> List { get; set; }
    }

    public class SectionViewModel
    {
        public Guid SectionId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string counter { get; set; }
        public string course { get; set; }
        public string section { get; set; }
        public string classroom { get; set; }
        public string career { get; set; }
        public string faculty { get; set; }
        public int year { get; set; }
        public int students { get; set; }
        public string hourTeoric { get; set; }
        public double hourTeoricNumber { get; set; }
        public string turn { get; set; }
        public string hourPractice { get; set; }
        public double hourPracticeNumber { get; set; }

        public string totalHours { get; set; }
        public double totalHoursNumber { get; set; }
        public string CourseTermModality { get; set; }
        public string AcademicYears { get; set; }

        public List<ClassSchedule> ClassSchedules { get; set; }

    }
    public class Report3Row
    {
        public string Teacher { get; set; }
        public string Category { get; set; }
        public string Grade { get; set; }
        public string Condition { get; set; }
        public string Course { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string CourseArea { get; set; }
        public string AcademicDepartment { get; set; }
        public string Cycle { get; set; }
        public double HT { get; set; }
        public double HP { get; set; }
        public double HS { get; set; }
        public double HV { get; set; }
        public string Group { get; set; }
        public double TotalHours { get; set; }
        public string Turn { get; set; }
        public int NumberStudents { get; set; }
        public int ClassRoom { get; set; }
        public string CareerCode { get; set; }
        public string ClassRoomName { get; set; }
        public decimal Credits { get; set; }

        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }

        public int SessionTypeMonday { get; set; }
        public int SessionTypeTuesday { get; set; }
        public int SessionTypeWednesday { get; set; }
        public int SessionTypeThursday { get; set; }
        public int SessionTypeFriday { get; set; }
        public int SessionTypeSaturday { get; set; }
        public List<int> SessionTypes { get; set; }
        public List<SessionTypeTemplate> SessionTypeTemplates { get; set; }
        public string Section { get; set; }
    }
    public class SessionTypeTemplate
    {
        public string Duration { get; set; }
        public string Range { get; set; }
        public int WeekDay { get; set; }
        public int SessionType { get; set; }
    }
    public class NonActivityViewModel
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Hours { get; set; }

        public string MondayRange { get; set; }
        public string Monday { get; set; }
        public string TuesdayRange { get; set; }
        public string Tuesday { get; set; }
        public string WednesdayRange { get; set; }
        public string Wednesday { get; set; }
        public string ThursdayRange { get; set; }
        public string Thursday { get; set; }
        public string FridayRange { get; set; }
        public string Friday { get; set; }
        public string SaturdayRange { get; set; }
        public string Saturday { get; set; }
        public int Category { get; set; }
        public string Resolution { get; set; }
        public string Location { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public Guid TeachingLoadTypeId { get; set; }
    }
    public class ClassSchedule
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan StartTimeLocal { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan EndTimeLocal { get; set; }
        public string Code { get; set; }
        public string TimeText { get; set; }
        public int WeekDay { get; set; }
        public Guid SectionId { get; set; }
        public int SessionType { get; set; }
        public double TotalHours { get; set; }
        public string SectionGroup { get; set; }
        public int SectionGroupStudentsCount { get; set; }
    }
}
