using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class Income
    {
        public int Id { get; set; }

        public Guid DependencyId { get; set; }

        //public Guid? RelatedInvoiceId { get; set; }

        public Guid? PaymentId { get; set; }

        public Guid? CurrentAccountId { get; set; }

        public decimal Amount { get; set; }

        public byte Type { get; set; } = ConstantHelpers.Treasury.Income.Type.INCOME; //1: Income 2:Outcome

        public string Invoice { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public bool IsBankPayment { get; set; }

        //public Invoice RelatedInvoice { get; set; }

        public Payment Payment { get; set; }

        public Dependency Dependency { get; set; }

        public CurrentAccount CurrentAccount { get; set; }
    }
}
