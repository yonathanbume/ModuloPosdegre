namespace AKDEMIC.INTRANET.Areas.Admin.Models.TotalizedReportViewModels
{
    public class GroupConfiguration
    {
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }
        public string Description { get; set; }
    }
    public class GroupConfigurationIMC
    {
        public decimal MinimumIMC { get; set; }
        public decimal MaximumIMC { get; set; }
        public string DescriptionIMC { get; set; }        
    }

    public class GroupConfigurationBodyFatPercentage
    {
        public decimal MinimumPercentage { get; set; }
        public decimal MaximumPercentage { get; set; }
        public string Description { get; set; }
    }
}
