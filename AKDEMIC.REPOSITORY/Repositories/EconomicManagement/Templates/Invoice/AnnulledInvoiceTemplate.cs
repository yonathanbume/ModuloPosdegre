using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Invoice
{
    public class AnnulledInvoiceTemplate
    {
        public Guid Id { get; set; }

        public string Invoice { get; set; }

        public string Date { get; set; }

        public string Client { get; set; }

        public decimal Total { get; set; }

        public string Cashier { get; set; }
    }
}
