using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class CardPaymentHeader
    {
        public Guid Id { get; set; }
        public int PurchaseNumber { get; set; } //Unico Número de pedido generado por nosotros
        public decimal TotalAmount { get; set; } //Importe de la transacción Formato ####.## (dos decimales separados por punto)
        public string UserId { get; set; }
        public string Currency { get; set; } //Moneda de la transacción
        public int State { get; set; } //Estado VISA.STATES.VALUES
        public string TraceNumber { get; set; } //Número de orden de la transacción generada en los sistemas de VisaNet
        public string TransactionId { get; set; } //ID único de la transacción del sistema VisaNet (¡)
        public ApplicationUser User { get; set; }
    }
}
