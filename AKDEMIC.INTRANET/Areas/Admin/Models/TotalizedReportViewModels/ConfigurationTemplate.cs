using AKDEMIC.ENTITIES.Models.Intranet;
using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.TotalizedReportViewModels
{
    public class ConfigurationTemplate
    {
        public int Id { get; set; }
        public string DescriptionTemplate { get; set; }
    }

    public class ConfigurationTemplate2
    {
        public byte Id { get; set; }
        public string DescriptionTemplate2 { get; set; }
    }

    public class ConfigurationTemplateGuid
    {
        public Guid Id { get; set; }
        public PsychologicalDiagnostic entity { get; set; }
    }
}
