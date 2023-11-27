using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Student.Models.PendingDebtsViewModels
{
    public class ReportPdfViewModel
    {
        public string Image { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Career { get; set; }

        public string Term { get; set; }

        public List<PaymentViewModel> Payments { get; set; }
    }
}
