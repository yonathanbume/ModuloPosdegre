using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseTerm
{
    public class CourseStatisticTemplate
    {
        public string Code { get; set; }
        public string Course { get; set; }
        public string Career { get; set; }
        public int GradeCount { get; set; }
        public double Mean { get; set; }
        public double Median { get; set; }
        public double StandardDeviation { get; set; }
        public double Percentile25 { get; set; }
        public double Percentile50 { get; set; }
        public double Percentile75 { get; set; }
    }
}
