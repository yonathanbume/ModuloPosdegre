using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class CardPaymentDetail
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public Guid CardPaymentHeaderId { get; set; }
        public Payment Payment { get; set; }
        public CardPaymentHeader CardPaymentHeader { get; set; }
    }
}
