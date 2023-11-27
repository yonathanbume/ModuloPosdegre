using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleExtraPerformanceEvaluationField
{
    public class TeachingPerformanceResultTemplate
    {
        public List<TeachingPerformanceTermTemplate> Terms { get; set; }
        public List<TeachingPerformanceTemplate> Teachers { get; set; }
    }


    public class TeachingPerformanceTemplate
    {
        public string UserId { get; set; }
        public string Teacher { get; set; }
        public string UserName { get; set; }
        public string AcademicDepartment { get; set; }
        public List<TeachingPerformanceTermTemplate> Terms { get; set; }
    }

    public class TeachingPerformanceTermTemplate
    {
        public Guid Id { get; set; }
        public string Term { get; set; }
        public List<TeachingPerformanceEvaluationTemplate> Evaluations { get; set; }
    }

    public class TeachingPerformanceEvaluationTemplate
    {
        public string EvaluationCode { get; set; }
        public decimal? Qualification { get; set; }
    }
}
