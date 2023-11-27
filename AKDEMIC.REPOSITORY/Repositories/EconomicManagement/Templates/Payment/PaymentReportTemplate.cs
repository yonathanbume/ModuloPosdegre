using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Payment
{
    public class PaymentReportTemplate
    {
        public int DocumentType { get; set; }

        public string Series { get; set; }

        public string Number { get; set; }

        public DateTime Date { get; set; }

        public string Document { get; set; }

        public string ClientName { get; set; }

        public string Classifier { get; set; }

        public string Concept { get; set; }

        public decimal Quantity { get; set; }

        public decimal Total { get; set; }

        public string AccountingPlan { get; set; }

        public string Cashier { get; set; }

        public string Dependency { get; set; }
    }
}
