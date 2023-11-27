using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleExtraPerformanceEvaluationField
{
    public class PerformanceEvaluationResultTemplate
    {
        public string UserName{ get; set; }
        public string FullName { get; set; }
        public string AcademicDepartment { get; set; }
        public decimal? BaseScore { get; set; }
        public decimal? Qualification { get; set; }
        public decimal? VigesimalScaleQuaiflication { get; set; }
    }
}
