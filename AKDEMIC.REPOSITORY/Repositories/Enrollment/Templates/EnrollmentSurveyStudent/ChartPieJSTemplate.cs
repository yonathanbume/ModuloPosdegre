using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.EnrollmentSurveyStudent
{
    public class ChartPieJSTemplate
    {
        public List<DataPieTemplate> Data { get; set; }
        public string Title { get; set; }
    }
    public class DataPieTemplate
    {
        public string Name { get; set; }
        public int Y { get; set; }
    }
}
