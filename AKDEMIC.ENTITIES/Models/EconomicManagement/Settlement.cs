using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class Settlement : Entity, ICodeNumber, ITimestamp
    {
        public Guid Id { get; set; }

        public string CardNumber { get; set; }
        public string CurrencyCode { get; set; }
        public string CustomerDni { get; set; }
        public string CustomerName { get; set; }
        public DateTime PaymentDate { get; set; }
        public int QuantityOfDocs { get; set; }
        public decimal TotalAmount { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public ICollection<SettlementDetail> Details { get; set; }
    }
}
