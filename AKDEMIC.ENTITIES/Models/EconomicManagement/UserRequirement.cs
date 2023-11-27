using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class UserRequirement
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid RequirementId { get; set; }
        public Guid? ExecuteObservationId { get; set; }
        public string RoleId { get; set; }
        public string Comment { get; set; }
        public string AcceptanceRejectionReason { get; set; }
        public decimal? Cost { get; set; }
        public bool IsForecast { get; set; } = false;
        public bool IsPAC { get; set; } = false;
        public int Status { get; set; } = 1;
        public int? SubStatus { get; set; }
        public string ProcessType { get; set; }
        public string ProcessNomenclature { get; set; }
        public string PurposeHiring { get; set; }
        public decimal AmountReferenceValue { get; set; }
        public byte? Relation { get; set; }
        public string References { get; set; }
        public DateTime? UpdateStatus { get; set; }
        public DateTime? UpdateAsignOrder { get; set; }
        public ApplicationRole Role { get; set; }
        public Requirement Requirement { get; set; }
        public Order Order { get; set; }
        public ExecuteObservation ExecuteObservation { get; set; }
        public ICollection<UserRequirementFile> UserRequirementFiles { get; set; }
        public ICollection<ReceivedOrder> ReceivedOrders { get; set; }
        public ICollection<UserRequirementItem> UserRequirementItems { get; set; }
    }
}
