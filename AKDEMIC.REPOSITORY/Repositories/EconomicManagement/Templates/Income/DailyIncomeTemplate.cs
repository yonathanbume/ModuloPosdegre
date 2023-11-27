namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Income
{
    public class DailyIncomeTemplate
    {
        public string date { get;  set; }
        public string siaf { get;  set; }
        public string invoice { get;  set; }
        public string document { get;  set; }
        public string order { get;  set; }
        public string concept { get;  set; }
        public string month { get;  set; }
        public decimal provision { get;  set; }
        public decimal income { get;  set; }
        public decimal expense { get;  set; }
    }
}
