using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class EnrollmentReportTemplate
    {
        public string Image { get; set; }
        public string HeaderText { get; set; }
        public string SubHeaderText { get; set; }
        public string FooterText { get; set; }

        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string SchoolName { get; set; }
        public string Semester { get; set; }
        public string FacultyName { get; set; }
        public string StudentCondition { get; set; }
        public string CurrentSemester { get; set; }

        public Guid CurriculumId { get; set; }
        public string Curriculum { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsPronabec { get; set; }
        public bool IsExtraordinaryReport { get; set; } = false;

        public string AdmissionTerm { get; set; }
        public string Group { get; set; }

        public int MaxAcademicYear { get; set; }

        public string PrintingDate { get; set; }
        public string PrintingHour { get; set; }

        public string EnrollmentDate { get; set; }
        public string EnrollmentHour { get; set; }

        public string SignatuareImgBase64 { get; set; }
        public List<EnrollmentReportCourseTemplate> Courses { get; set; }
        //public List<CoursesScheduleViewModel> Schedule { get; set; }

        public string Document { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Email { get; set; } = "";
        public string BirthDay { get; set; } = "";

        public string ImageQR { get; set; }
    }
}
