using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Payment
{
    public class PaymentsDetailedTemplate
    {
        public string Type { get; set; }
        public string Serie { get; set; }
        public string Number { get; set; }
        public string PaymentDate { get; set; }
        public string Client { get; set; }
        public string ClientDNI { get; set; }
        public string AccountingPlan { get; set; }
        public string Concept { get; set; }
        public decimal Quantity { get; set; }
        public string TotalAmount { get; set; }
        public decimal TotalDecimal { get; set; }
    }
}
