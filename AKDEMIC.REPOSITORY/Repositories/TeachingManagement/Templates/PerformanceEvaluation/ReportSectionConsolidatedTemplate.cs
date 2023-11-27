using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluation
{
    public class ReportSectionConsolidatedTemplate
    {
        public string EvaluationName { get; set; }
        public string Term { get; set; }
        public string AcademicDepartment { get; set; }
        public string Img { get; set; }
        public List<TeacherSectionTemplate> Teachers { get; set; }
    }

    public class TeacherSectionTemplate
    {
        public string FullName { get; set; }
        public decimal Average { get; set; }
        public List<SectionConsolidatedTemplate> Sections { get; set; }
    }

    public class SectionConsolidatedTemplate
    {
        public string Code { get; set; }
        public bool AnyEvaluationAnswered { get; set; }
        public string CourseCode { get; set; }
        public string Course { get; set; }
        public int SumResponses { get; set; }
        public int SurveysAnswered { get; set; }
        public decimal Average { get; set; }
    }

}
