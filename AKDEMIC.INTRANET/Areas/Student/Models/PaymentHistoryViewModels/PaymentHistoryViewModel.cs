using System;

namespace AKDEMIC.INTRANET.Areas.Student.Models.PaymentHistoryViewModels
{
    public class PaymentHistoryViewModel
    {
        public string NumDocument { get; set; }
        public string CoinType { get; set; }
        public string Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal Impost { get; set; }
        public string AmountCanceled { get; set; }
        public string PaymentDate { get; set; }
        public string Term { get; set; }
        public Guid Id { get; set; }
        public bool Status { get; set; }
        public string Type { get; set; }
    }
}
