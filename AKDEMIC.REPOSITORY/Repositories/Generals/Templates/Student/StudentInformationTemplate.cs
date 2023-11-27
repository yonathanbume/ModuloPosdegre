using System;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class StudentInformationTemplate
    {
        public Guid Id { get; set; }
        public Guid CurriculumId { get; set; }
        public Guid AcademicProgramId { get; set; }
        public Guid CareerId { get; set; }
        public string Picture { get; set; }
        public string Campus { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string Career { get; set; }
        public string Modality { get; set; }
        public string CurrentTerm { get; set; }
        public string Dni { get; set; }
        public string Curriculum { get; set; }
        public int MeritOrder { get; set; }
        public string FirstEnrollmentDate { get; set; }
        public string AdmissionTerm { get; set; }
        public string AdmissionTermDate { get; set; }
        public string GraduationTerm { get; set; }
        public string GraduationTermDate { get; set; }
        public string IsBachelor { get; set; }
        public decimal AverageGrade { get; set; }
        public decimal CumulativeGrade { get; set; }
    }
    public class StudentInformationDataTableTemplate : StudentPersonalInformationDataTable
    {
        public bool Existfile { get; set; }
    }
    public class StudentPersonalInformationDataTable
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Paternalsurname { get; set; }
        public string Maternalsurname { get; set; }
        public string Email { get; set; }
    }

    public class StudentDesertorReportTemplate
    {
        public string DataReport { get; set; }
        public int ValueData { get; set; }
        public string PercentValueData { get; set; }
    }

    public class StudentConstancy
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string FacultyName { get; set; }
        public string CareerName { get; set; }
        public string TermName { get; set; }
        public string QRImage { get; set; }
    }
}
