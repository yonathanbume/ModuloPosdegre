using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey
{

    public class SurveyItemReportTemplate
    {
        public string Title { get; set; }
        public List<QuestionReportTemplate> Reportes { get; set; }
    }

    public class QuestionReportTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public List<AlternativeTemplate> Alternatives { get; set; }
    }


    public class AlternativeTemplate
    {
        public string Description { get; set; }
        public int Count { get; set; }
    }
}
