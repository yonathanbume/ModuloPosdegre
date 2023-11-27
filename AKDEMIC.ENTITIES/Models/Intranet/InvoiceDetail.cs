using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class InvoiceDetail
    {
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }

        public Guid? ConceptId { get; set; }
        public Guid? CurrentAccountId { get; set; }

        [Required]
        public string Description { get; set; }

        public string Code { get; set; }

        public decimal Quantity { get; set; } = 1;

        public decimal IgvAmount { get; set; } = 0.00M;

        public decimal SubTotal { get; set; } = 0.00M;

        public decimal Total { get; set; } = 0.00M;

        public Invoice Invoice { get; set; }
        public CurrentAccount CurrentAccount { get; set; }
        public Concept Concept { get; set; }

    }
}
