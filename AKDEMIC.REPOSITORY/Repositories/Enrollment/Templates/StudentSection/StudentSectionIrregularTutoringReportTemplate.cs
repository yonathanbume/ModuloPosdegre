using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class StudentSectionIrregularTutoringReportTemplate
    {
        public Guid StudentSectionId { get; set; }
        public string UserName { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int Sex { get; set; }
        public string Dni { get; set; }
        public int CurrentAcademicYear { get; set; }
        public string StudentCareerCode { get; set; }
        public string StudentCareerName { get; set; }
        public string StudentFacultyName { get; set; }
        public string StudentCurriculumCode { get; set; }
        public string SectionCode { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseCareerName { get; set; }
        public string AcademicProgram { get; set; }
        public string AcademicProgramCareerName { get; set; }
        public int Try { get; set; }
    }
}
