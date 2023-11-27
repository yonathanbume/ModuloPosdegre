using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class PhaseRequirement
    {
        public Guid Id { get; set; }
        public string Summoned { get; set; }
        public DateTime? SummonedDate { get; set; }
        public string Awarded { get; set; }
        public DateTime? AwardedDate { get; set; }
        public string DescriptionAwarded { get; set; }
        public byte? RatioAwarded { get; set; }
        public string GrantingGoodPro { get; set; }
        public DateTime? GrantingGoodProDate { get; set; }
        public string Spoiled { get; set; }
        public DateTime? SpoiledDate { get; set; }
        public string DescriptionSpoiled { get; set; }
        public int? ContractNumber { get; set; }
        public string ServiceOrPurchaseOrder { get; set; }
        public int? ServiceOrPurchaseOrderNumber { get; set; }
        public int? AmountAwarded { get; set; }
        public DateTime? StartDateExecution { get; set; }
        public int? TermDateExecution { get; set; }
        public DateTime? EndDateExecution { get; set; }
    }
}
