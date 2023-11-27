using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluationUser
{
    public class StatisticalReportCareerTeacherPerformanceTemplate
    {
        public string Code { get; set; }
        public string Term { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Max { get; set; }
        public List<StatisticalReportCareerTeacherPerformanceQuestionTemplate> Questions { get; set; }
        public List<StatisticalReportCareerTeacherPerformanceDetailTemplate> Details { get; set; }
        public List<StatisticalReportCareerTeacherPerformanceCriterionTemplate> Criterions { get; set; }
    }
    public class StatisticalReportCareerTeacherPerformanceDetailTemplate
    {
        public string AcademicDepartment { get; set; }
        public string Faculty { get; set; }
        public List<StatisticalReportCareerTeacherPerformanceResponseTemplate> Responses { get; set; }
        public decimal Qualification { get; set; }
        public decimal QualificationPercentage { get; set; }
        public decimal RealQualification { get; set; }

        public List<StatisticalReportCareerTeacherPerformanceCriterionTemplate> CriterionDetails { get; set; }
    }

    public class StatisticalReportCareerTeacherPerformanceQuestionTemplate
    {
        public Guid Id { get; set; }
        public Guid? CriterionId { get; set; }
        public string Criterion { get; set; }
        public string Description { get; set; }
    }

    public class StatisticalReportCareerTeacherPerformanceCriterionTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Qualification { get; set; }
        public decimal QualificationPercentage { get; set; }
    }

    public class StatisticalReportCareerTeacherPerformanceResponseTemplate
    {
        public Guid QuestionId { get; set; }
        public int Value { get; set; }
        public int Quantity { get; set; }
    }
}
