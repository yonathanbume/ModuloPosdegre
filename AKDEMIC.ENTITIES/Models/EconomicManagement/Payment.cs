using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.ENTITIES.Models.PosDegree;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class Payment : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        //public Guid? ClientId { get; set; }
        public Guid? ConceptId { get; set; }
        public Guid? CurrentAccountId { get; set; }
        public Guid? EntityId { get; set; }
        public Guid? InvoiceId { get; set; }
        public Guid? ParentPaymentId { get; set; }
        public Guid? TermId { get; set; }
        public Guid? ExternalUserId { get; set; }
        public string UserId { get; set; }

        public int BankIdentifier { get; set; }
        public string BankAgentCode { get; set; }
        public string BankCashierCode { get; set; }
        public string BankCondition { get; set; }
        public string Description { get; set; }
        public decimal Discount { get; set; } = 0.00M;
        public decimal IgvAmount { get; set; } = 0.00M;
        
        public bool IsBankPayment { get; set; } = false;
        
        public bool WasBankPaymentUsed { get; set; } = false;
        public bool WasExonerated { get; set; } = false;

        public Guid? EntityLoadFormatId { get; set; }
        public EntityLoadFormat EntityLoadFormat { get; set; }

        public bool IsPartialPayment { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public decimal LateCharge { get; set; } = 0.00M;
        public string OperationCodeB { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal Quantity { get; set; } = 1;
        public string Receipt { get; set; }
        public byte Status { get; set; } = ConstantHelpers.PAYMENT.STATUS.PENDING;
        public decimal SubTotal { get; set; } = 0.00M;
        public decimal Total { get; set; } = 0.00M;
        public byte Type { get; set; } = ConstantHelpers.PAYMENT.TYPES.CONCEPT;
        
        public ApplicationUser User { get; set; }
        public ExternalUser ExternalUser { get; set; }
        //public Client Client { get; set; }
        public Concept Concept { get; set; }
        public CurrentAccount CurrentAccount { get; set; }
        public Invoice Invoice { get; set; }
        public Payment ParentPayment { get; set; }
        public Term Term { get; set; }

        public ICollection<Payment> Payments { get; set; }
        public ICollection<UserProcedure> UserProcedures { get; set; }
        public ICollection<UserExternalProcedure> UserExternalProcedures { get; set; }
        public ICollection<Income> Incomes { get; set; }
        public ICollection<PosdegreeDetailsPayment> PosdegreeDetailsPayment { get; set;}
    }
}
