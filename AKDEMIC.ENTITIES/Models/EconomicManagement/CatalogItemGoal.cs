using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class CatalogItemGoal
    {
        public Guid Id { get; set; }
        public Guid CatalogItemId { get; set; }
        public Guid CatalogGoalId { get; set; }
        public Guid UserRequirementId { get; set; }
        public CatalogItem CatalogItem { get; set; }
        public UserRequirement UserRequirement { get; set; }
        public CatalogGoal CatalogGoal { get; set; }
    }
}
