using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class PaymentToValidate : Entity, ITimestamp
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid? ConceptId { get; set; }
        public Concept Concept { get; set; }

        public Guid? CurrentAccountId { get; set; }
        public CurrentAccount CurrentAccount { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        //public string Type { get; set; }
        public DateTime Date { get; set; }

        public string SecuenceCode { get; set; }
        public string BankCashierCode { get; set; }
        public string BankAgentCode { get; set; }
        public string BankCondition { get; set; }

        public Guid? EntityLoadFormatId { get; set; }
        public EntityLoadFormat EntityLoadFormat { get; set; }

        //USADO PARA PAGOS DE OTRA DB
        public Guid? ExternalUserId { get; set; }
        public ExternalUser ExternalUser { get; set; }
    }
}
