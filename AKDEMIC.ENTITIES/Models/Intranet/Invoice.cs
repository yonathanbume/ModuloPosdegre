using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class Invoice
    {
        public Guid Id { get; set; }
        //public Guid? ClientId { get; set; }
        public Guid? ExternalUserId { get; set; }
        public string UserId { get; set; }
        public Guid? PettyCashId { get; set; }

        public bool Annulled { get; set; } = false;
        public bool Canceled { get; set; } = false;

        [Required]
        public string ClientName { get; set; }
        public string Comment { get; set; }
        //public int Correlative { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        public decimal Descount { get; set; } = 0.00M;

        [Required]
        public string Dni { get; set; }
        public byte DocumentType { get; set; } = 1; // 1 = boleta interna, 2 = boleta, 3 = factura
        public byte PdfTemplate { get; set; } = ConstantHelpers.Treasury.Invoice.PdfTemplate.MULTIPLE_CONCEPTS; // 1 = pago multiple concepto o matricula, 2 = pago unico

        [Required]
        public decimal IgvAmount { get; set; } = 0.00M;
        
        [Required]
        public int Number { get; set; }
        public byte PaymentType { get; set; } = ConstantHelpers.Treasury.Invoice.PaymentType.CASH;
        public string Series { get; set; }

        [NotMapped]
        public string InvoiceNumber => $"{Series}-{Number:00000000}";

        [Required]
        public decimal Subtotal { get; set; } = 0.00M;
        public string SunatCdrUrl { get; set; }
        public byte? SunatStatus { get; set; } = 0;
        public string SunatTicket { get; set; }
        public byte? ElectronicDocumentType { get; set; }

        [Required]
        public decimal TotalAmount { get; set; } = 0.00M;

        public string Voucher { get; set; }
        public DateTime? VoucherDate { get; set; }
        public decimal VoucherAmount { get; set; }
        
        //public Client Client { get; set; }
        public ExternalUser ExternalUser { get; set; }
        public ApplicationUser User { get; set; }
        //public CreditNote CreditNote { get; set; }
        public PettyCash PettyCash { get; set; }
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
