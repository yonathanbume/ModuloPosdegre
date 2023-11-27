using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class EnrollmentReportViewModel
    {
        public string HeaderText { get; set; }
        public string Image { get; set; }
        public Guid CurriculumId { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string SchoolName { get; set; }
        public string Semester { get; set; }
        public string PrintingDate { get; set; }
        public string FacultyName { get; set; }
        public string StudentCondition { get; set; }
        public string CurrentSemester { get; set; }
        public string RegisterDate { get; set; }
        public string PrintingHour { get; set; }
        public string Curriculum { get; set; }
        public bool IsConfirmed { get; set; }
        public string Group { get; set; }
        public string AdmissionTerm { get; set; }
        public int MaxAcademicYear { get; set; }
        public bool IsPronabec { get; set; }

        public List<CourseTermViewModel> RegisterCourseTerms { get; set; }
        public List<CoursesScheduleViewModel> Schedule { get; set; }

        public string Document { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Email { get; set; } = "";
        public string BirthDay { get; set; } = "";
    }

    public class CourseTermViewModel
    {
        public Guid StudentSectionId { get; set; }
        public string Cicle { get; set; }
        public byte AcademicYear { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Section { get; set; }
        public string Credits { get; set; }
        public string Times { get; set; }
        public Guid CourseId { get; set; }
        public SectionDetailViewModel SectionDetailViewModel { get; set; }
        public string Modality { get; set; }

        public int TheoricalHours { get; set; } = 0;
        public int PracticalHours { get; set; } = 0;
        public int LaboratoryHours { get; set; } = 0;
        public string Regime { get; set; } = "";
    }

    public class CoursesScheduleViewModel
    {
        public string WeekDay { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CourseName { get; set; }
        public string Classroom { get; set; }
    }

    public class SectionDetailViewModel
    {
        public string Section { get; set; }
        public string Times { get; set; }
        public string EndTime { get; set; }
    }
}
