using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public sealed class CreditNoteDetail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal UnitaryPrice { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal Quantity { get; set; }
        public decimal DiscountPercentage { get; set; }

        public Guid CreditNoteId { get; set; }
        public CreditNote CreditNote { get; set; }
    }
}