using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class SectionPartialGradesReportTemplate
    {
        public string Section { get; set; }
        public Guid SectionId { get; set; }
        public List<PartialGradeEvaluationTemplate> Evaluations { get; set; }
        public List<StudentPartialGradesReportTemplate> Students { get; set; }
    }

    public class StudentPartialGradesReportTemplate
    {
        public Guid StudentSectionId { get; set; }
        public Guid StudentId { get; set; }
        public string FullName { get; set; }
        public int Status { get; set; }
        public string Username { get; set; }
        public int FinalGrade { get; set; }
        public List<PartialGradesReportTemplate> Grades { get; set; }
    }

    public class PartialGradeEvaluationTemplate
    {
        public Guid EvaluationId { get; set; }
        public string Evaluation { get; set; }
        public int Percentage { get; set; }
        public bool HasGradeRegistration { get; set; }
        public DateTime GradeRegistrationDate { get; set; }
        public bool GradeRegistationPublished { get; set; }
        public string User { get; set; }
    }

    public class PartialGradesReportTemplate
    {
        public Guid Id { get; set; }
        public Guid EvaluationId { get; set; }
        public decimal Value { get; set; }
        public List<GradeCorrection> Test { get; set; }
        public bool HasGradeCorrection { get; set; }
        public decimal? LastGrade { get; set; }
    }
    
}
