using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class InvoiceVoucher
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public decimal Amount { get; set; }

        public Guid InvoiceId { get; set; }

        public Invoice Invoice { get; set; }
    }
}
