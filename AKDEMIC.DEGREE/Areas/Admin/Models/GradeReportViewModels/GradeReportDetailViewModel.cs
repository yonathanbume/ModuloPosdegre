using System;

namespace AKDEMIC.DEGREE.Areas.Admin.Models.GradeReportViewModels
{
    public class GradeReportViewModel
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public int Year { get; set; }
        public string Number { get; set; }
        public string Date { get; set; }
        public int YearsStudied { get; set; }
        public int SemesterStudied { get; set; }
        public int AdmissionYear { get; set; }
        public int GraduationYear { get; set; }
        public decimal PromotionGrade { get; set; }
        public string Observation { get; set; }
        public Guid? ProcedureId { get; set; }
        public Guid? ConceptId { get; set; }
    }

    public class GradeReportDetailViewModel
    {
        public int Year { get; set; }
        public string Number { get; set; }
        public string Date { get; set; }
        public int YearsStudied { get; set; }
        public int SemesterStudied { get; set; }
        public Guid AdmissionTermId { get; set; }
        public Guid GraduationTermId { get; set; }
        public decimal PromotionGrade { get; set; }
        public string Observation { get; set; }
        public Guid? ProcedureId { get; set; }
        public Guid? ConceptId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string FacultyName { get; set; }
        public string CareerName { get; set; }
        public string CurricularSystem { get; set; }
        public string Curriculum { get; set; }
        public string AcademicProgram { get; set; }
        public bool IsIntegrated { get; set; }

    }
}
