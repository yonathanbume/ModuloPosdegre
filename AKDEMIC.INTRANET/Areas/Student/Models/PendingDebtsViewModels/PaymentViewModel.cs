using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Student.Models.PendingDebtsViewModels
{
    public class PaymentViewModel
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
    }
}
