using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.EnrollmentSurveyStudent
{
    public class ChartJSTemplate
    {
        public string[] Categories { get; set; }
        public List<DataTemplate> YesNoData { get; set; }
        public object InternetConnectionTypeData { get; set; }
    }
    public class DataTemplate
    {
        public string Name { get; set; }
        public int[] Data { get; set; }
    }

}
