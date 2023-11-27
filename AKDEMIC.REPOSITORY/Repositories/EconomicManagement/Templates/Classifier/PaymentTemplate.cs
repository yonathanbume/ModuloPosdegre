using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Classifier
{
    public class PaymentTemplate
    {
        public string User { get; set; }
        public string Description { get; set; }
        public string IssueDate { get; set; }
        public string PaymentDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string CreatedBy { get; set; }
        public string PaymentType { get; set; }
        public string Serie { get; set; }
        public int Number { get; set; }
    }
}
