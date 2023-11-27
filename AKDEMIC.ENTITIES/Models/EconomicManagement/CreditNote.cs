using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public sealed class CreditNote : Entity, ITimestamp, ISoftDelete
    {
        public Guid Id { get; set; }

        public Guid ExternalUserId { get; set; }
        public ExternalUser ExternalUser { get; set; }

        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public string SunatTicket { get; set; }
        public byte SunatStatus { get; set; }
        public string SunatCdrUrl { get; set; }
        public string Serie { get; set; }
        public int Number { get; set; }
        public string ReferencedDocumentSerie { get; set; }
        public int ReferencedDocumentNumber { get; set; }
        public string ReferencedDocumentType { get; set; }
        public string CreditNoteCode { get; set; }
        public string Description { get; set; }

        public IEnumerable<CreditNoteDetail> CreditNoteDetails { get; set; }
    }
}