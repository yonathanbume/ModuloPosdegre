namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicSummaries
{
    public class AcademicSummaryTemplate
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public int Observations { get; set; }
        public decimal Finalgrade { get; set; }
        public bool Approbed { get; set; }
        public int Order { get; set; }
    }
    public class AcademicSummaryReportTemplate
    {
        public string approbedname { get;  set; }
        public int approbeds { get;  set; }
        public string disapprobedname { get; internal set; }
        public int disapprobeds { get; internal set; }
    }

    public class AcademicSummaryGraduatedReportTemplate
    {
        public string GraduatedInTime { get; set; }
        public string Career { get; set; }
        public int TotalGraduated { get; set; }
    }

    public class AcademicSummaryGraduatedReportNumberTemplate
    {
        public double GraduatedInTime { get; set; }
        public string Career { get; set; }
        public int TotalGraduated { get; set; }
    }

}
