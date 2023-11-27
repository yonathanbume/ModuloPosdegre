using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport
{
    public class EvaluationReportExcelTemplate
    {
        public Guid? CareerId{ get; set; }
        public string Career { get; set; }
        public Guid CurriculumId { get; set; }
        public string Curriculum { get; set; }
        public string Code { get; set; }
        public string CreatedAt { get; set; }
        public string Section { get; set; }
        public string Course { get; set; }
        public string LastGenerated { get; set; }
        public string Status { get; set; }
        public string Teacher { get; set; }
        public string ReceptionDate { get; set; }
    }
}
