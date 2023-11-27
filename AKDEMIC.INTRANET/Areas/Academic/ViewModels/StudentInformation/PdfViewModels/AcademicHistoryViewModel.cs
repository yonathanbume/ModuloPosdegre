using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation.PdfViewModels
{
    public class AcademicHistoryPdfViewModel
    {
        public string ImagePathLogo { get; set; }
        public string JsPath { get; set; }
        public DateTime Today { get; set; }
        public StudentInfoViewModel StudentInfo { get; set; }
        public ICollection<AcademicYearViewModel> AcademicYears { get; set; }
        public List<AcademicPerformanceSummaryDetailViewModel> Details { get; set; }
    }

    public class AcademicPerformanceSummaryDetailViewModel
    {
        public string Term { get; set; }
        public decimal Average { get; set; }
        public string ApprovedCredits { get; set; }
        public string TotalCredits { get; set; }
        public string DisapprovedCredits { get; set; }
        public int Year { get; set; }
        public string Number { get; set; }

        public decimal TermScore { get; set; }
        public decimal CumulativeScore { get; set; }
        public decimal CumulativeWeightedAverage { get; set; }
        public decimal WeightedAverageGrade { get; set; }

        public decimal AdditionalCredits { get; set; }
    }

    public class StudentInfoViewModel
    {
        public string Score { get; set; }
        public string Position { get; set; }
        public string RegisterNumber { get; set; }
        public string Modality { get; set; }
        public string IncomeYear { get; set; }
        public string Faculty { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string CareerName { get; set; }
        public string StudentDni { get; set; }
        public string CampusName { get; set; }
        public string CurriculumCode { get; set; }
        public string StudentRegime { get; set; }
        public string Image { get; set; }
    }

    public class AcademicYearViewModel
    {
        public int AcademicYearNumber { get; set; }
        public ICollection<CourseViewModel> Courses { get; set; }
    }

    public class CourseViewModel
    {
        public string Code { get; set; }
        public string Credits { get; set; }
        public string Name { get; set; }
        public bool IsElective { get; set; }
        public ICollection<AcademicHistoryViewModel> AcademicHistories { get; set; }
        public EvaluationReportViewModel EvaluationReports { get; set; }
    }
    public class EvaluationReportViewModel
    {
        public string GeneratedId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
    public class AcademicHistoryViewModel
    {
        public string Term { get; set; }
        public int Grade { get; set; }
        public bool IsValidated { get; set; }
        public bool Withdrawn { get; set; }
        public bool Approved { get; set; }
        public string Observation { get; set; }
        public bool IsTpm { get; set; }
    }
}
