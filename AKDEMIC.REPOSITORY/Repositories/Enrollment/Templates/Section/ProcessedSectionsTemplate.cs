using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Section
{
    public class ProcessedSectionsTemplate
    {
        public string Term { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
        public List<EvalutaionReportTemplate> Units { get; set; }

        public ProcessedSectionsTemplate()
        {
            Units = new List<EvalutaionReportTemplate>();
        }
    }
    public class EvalutaionReportTemplate
    {
        public bool Complete { get; set; }
        public bool Waslate { get; set; }
    }
}
