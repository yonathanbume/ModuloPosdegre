using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class SettlementDetail
    {
        public Guid Id { get; set; }
        public Guid SettlementId { get; set; }
        public Guid PaymentId { get; set; }

        public Settlement Settlement { get; set; }
        public Payment Payment { get; set; }
    }
}
