using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Payment
{
    public class IncomeReceiptTemplate
    {
        public DateTime Date { get; set; }

        public decimal Total { get; set; }

        public List<IncomeReceiptCategoryTemplate> Categories { get; set; }
        public List<IncomeReceiptAccountTemplate> CurrentAccounts { get; set; }
    }

    public class IncomeReceiptCategoryTemplate
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Total { get; set; }

        public List<IncomeReceiptCategoryDetailTemplate> Accounts { get; set; }

    }

    public class IncomeReceiptCategoryDetailTemplate
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Classifier { get; set; }

        public decimal Total { get; set; }

        public decimal IgvAmount { get; set; }
    }

    public class IncomeReceiptAccountTemplate
    {
        public string Key { get; set; }

        public decimal Total { get; set; }
    }
}
