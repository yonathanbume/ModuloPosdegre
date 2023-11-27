using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class EntityLoadFormat
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public byte DocumentType { get; set; } = ConstantHelpers.Treasury.EntityLoadFormat.DocumentType.TXT;

        public int? EntityCodeStartIndex { get; set; }

        public int? EntityCodeLength { get; set; }

        public byte UserIdentifier { get; set; }

        public int UserNameStartIndex { get; set; }

        public int UserNameLength { get; set; }

        public int SecuenceCodeStartIndex { get; set; }

        public int SecuenceCodeLength { get; set; }

        public int? ConceptBankCodeStartIndex { get; set; }

        public int? ConceptBankCodeLength { get; set; }

        public bool IsAmountSeparated { get; set; }

        public int TotalAmountStartIndex { get; set; }

        public int TotalAmountLength { get; set; }

        public int IntegerPartAmountStartIndex { get; set; }

        public int IntegerPartAmountLength { get; set; }

        public int DecimalPartAmountStartIndex { get; set; }

        public int DecimalPartAmountLength { get; set; }

        public int DateStartIndex { get; set; }

        public int DateLength { get; set; }

        public int TimeStartIndex { get; set; }

        public int TimeLength { get; set; }

        public int? CashierCodeStartIndex { get; set; }

        public int? CashierCodeLength { get; set; }

        public int? AgentCodeStartIndex { get; set; }

        public int? AgentCodeLength { get; set; }

        public int? BankConditionStartIndex { get; set; }

        public int? BankConditionLength { get; set; }

        public int ClientDocumentStartIndex { get; set; }

        public int ClientDocumentLength { get; set; }

        public int ClientNameStartIndex { get; set; }

        public int ClientNameLength { get; set; }

        public int? CurrentAccountStartIndex { get; set; }

        public int? CurrentAccountLength { get; set; }

        public bool IsActive { get; set; }

        public Guid? TransactionConceptId { get; set; }
        public Concept TransactionConcept { get; set; }

        public decimal TransactionFee { get; set; }

        public bool IsPostgraduateFormat { get; set; }

        //public string DateStartFormat { get; set; }
    }
}
