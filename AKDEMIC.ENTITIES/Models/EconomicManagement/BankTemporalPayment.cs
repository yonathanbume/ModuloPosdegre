using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class BankTemporalPayment
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
        public Guid? ConceptId { get; set; }
        public Concept Concept { get; set; }

        public string CurrentAccountName { get; set; }
        public Guid? CurrentAccountId { get; set; }
        public CurrentAccount CurrentAccount { get; set; }

        public Guid? ExternalUserId { get; set; }
        public ExternalUser ExternalUser { get; set; }

        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string UserFullName { get; set; }
        public ApplicationUser User { get; set; }

        public string Type { get; set; }
        public decimal Amount { get; set; }

        public string Date { get; set; }
        public string Time { get; set; }

        public string SecuenceCode { get; set; }
        public string BankCashierCode { get; set; }
        public string BankAgentCode { get; set; }
        public string BankCondition { get; set; }

        public bool IsValid { get; set; } = true;

        public Guid? EntityLoadFormatId { get; set; }
        public EntityLoadFormat EntityLoadFormat { get; set; }
    }
}
