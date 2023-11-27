using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.EnrollmentSurveyStudent
{
    public class DetailedReportTemplate
    {
        public string TermName { get; set; }
        public List<AnswerTemplate> Answers { get; set; }
    }

    public class AnswerTemplate
    {
        public string UserName { get; set; }
        public string Career { get; set; }
        public string FullName { get; set; }
        public string HasComputerOrLaptop { get; set; }
        public string HasSmartphone { get; set; }
        public string HasInternet { get; set; }
        public string InternetConnectionType { get; set; }
    }
}
