using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicHistory
{
    public class CertificateTemplate
    {
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Credits { get; set; }
        public int Grade { get; set; }
        public string TermName { get; set; }
        public decimal TermMinGrade { get; set; }
        public Guid TermId { get; set; }
        public string Observations { get; set; }
        public int AcademicYear { get; set; }
        public string AcademicNumber { get; set; }
        public string Type { get; set; }
        public string EvaluationReportDate { get; set; }
    }

}
