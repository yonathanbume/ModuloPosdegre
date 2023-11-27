using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class CareerModalityPaymentFee
    {
        public Guid CareerModalityPaymentId { get; set; }

        public byte Number { get; set; }

        public decimal Amount { get; set; }

        public bool WasGenerated { get; set; }

        public CareerModalityPayment CareerModalityPayment { get; set; }
    }
}
