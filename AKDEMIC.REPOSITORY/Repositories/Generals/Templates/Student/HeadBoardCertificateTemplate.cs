using System;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class HeadBoardCertificateTemplate
    {
        public Guid IdStudent { get; set; }
        public string FacultyName { get; set; }
        public string CareerName { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string AdmissionYear { get; set; }
        public string GraduationYear { get; set; }
        public string UniversityName { get; set; }
        public Guid CurriculumId { get; set; }
        public string Dni { get; set; }
        public string AcademicProgram { get; set; }
        public int StudentSex { get; set; }
        public string BachelorName { get; set; }
    }
}
