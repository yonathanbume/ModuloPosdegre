using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Payment
{
    public class CashierDailyIncomeTemplate
    {
        public string FirstSeries { get; set; }
        public string LastSeries { get; set; }

        public List<CashierDailyIncomeDetailTemplate> Details { get; set; }
    }

    public class CashierDailyIncomeDetailTemplate
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal PrevAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
    }
}
