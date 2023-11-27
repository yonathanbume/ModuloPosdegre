using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class Client
    {
        public Guid Id { get; set; }
        public string BusinessName { get; set; }
        public string Name { get; set; }
        public string PatSurname { get; set; }
        public string MatSurname { get; set; }
        public string IdentificationDocumentType { get; set; }
        public string IdentificationDocumentNumber { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
    }
}
