namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicSummaries
{
    public class MeritChartDetailTemplate
    {
        public string Term { get; set; }
        public int MeritOrder { get; set; }
        public decimal Average { get; set; }
        public decimal ApprovedCredits { get; set; }
        public int TotalStudents { get; set; }
        public string Observations { get; set; }
    }
}
