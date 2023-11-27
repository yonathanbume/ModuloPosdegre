using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class BudgetBalance
    {
        public Guid Id { get; set; }
        public decimal Expense { get; set; }
        public string CostCenter { get; set; }
        public decimal BudgetFramework { get; set; }
        public int Year { get; set; }
        public decimal Balance { get; set; }
    }
}
