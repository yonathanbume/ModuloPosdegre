using System.Collections.Generic;

namespace AKDEMIC.INTRANET.ViewModels.TemplatesViewModels
{
    public class InvoiceViewModel
    {
        public string Number { get; set; }
        public string Date { get; set; }
        public string FullName { get; set; }
        public decimal TotalAmount { get; set; }

        public List<InvoiceDetailViewModel> Details { get; set; }
    }
}
