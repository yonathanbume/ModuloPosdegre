using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.GradeViewModels
{
    public class DetailViewModel
    {
        public Guid SectionId { get; set; }
        public string Section { get; set; }
        public string Course { get; set; }
        public bool EvaluationsByUnits { get; set; }
        public bool EnableBulkSaveGrades { get; set; }
        public List<EvaluationViewModel> Evaluations { get; set; }
        public List<StudentViewModel> Students { get; set; }
        public List<CourseUnitModel> CourseUnits { get; set; }
        public string JsonColumns { get; set; }
        public bool AllEvaluationsWithGrades { get; set; }
        public bool AvailableToPrint { get; set; }
        public bool EvaluationReportGenerated { get; set; }
        public string EvaluationReportCode { get; set; }
        public bool HasMeritOrder { get; set; }
        public bool HasAcademicHistories { get; set; }
        public bool GradesCanOnlyPublishedByPrincipalTeacher { get; set; }
        public bool IsPrincipalTeacher { get; set; }
    }

    public class EvaluationViewModel
    {
        public Guid Id { get; set; }
        public bool Taken { get; set; }
        public string Name { get; set; }
        public int? Week { get; set; }
        public int? Percentage { get; set; }
        public string Description { get; set; }
        public byte? CourseUnitNumber { get; set; }
        public string EvaluationSelectName => Week.HasValue ? $"{Name} (Sem. {Week})" : Name;
        public string SelectName => CourseUnitNumber.HasValue ? $"U{CourseUnitNumber} - {EvaluationSelectName}" : EvaluationSelectName;
        public string FormattedName => Name.Length > 15 ? $"{Name.Substring(0, 15)}..." : Name;
        public string TableName => CourseUnitNumber.HasValue ? $"U{CourseUnitNumber} - {FormattedName}" : FormattedName;
        public string Grade { get; set; }
        public bool Published { get; set; }
        public bool? Attended { get; set; }
    }

    public class StudentViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Withdrawn { get; set; } = false;
        public int FinalGrade { get; set; }
        public List<decimal> Grades { get; set; }
        public bool DPI { get; set; }
        public int Absents { get; set; }

        public List<EvaluationGradeViewModel> EvaluationGrades { get; set; }
    }

    public class JsonDatatbleColumn
    {
        public string field { get; set; }
        public string title { get; set; }
        public string width { get; set; }
        public string textAlign { get; set; }
    }

    public class CourseUnitModel
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public int WeekStart { get; set; }
        public int WeekEnd { get; set; }
        public Guid Id { get; set; }
    }

    public class EvaluationGradeViewModel
    {
        public Guid EvaluationId { get; set; }
        public decimal? Grade { get; set; }
    }
}
