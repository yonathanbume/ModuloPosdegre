using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class UserRequirementItem
    {
        public Guid Id { get; set; }
        public Guid UserRequirementId { get; set; }
        public Guid CatalogItemId { get; set; }
        public Guid? ClassifierId { get; set; }
        public Guid? CatalogActivityId { get; set; }
        public decimal? Value { get; set; }
        public int? Quantity { get; set; }
        public decimal? Total { get; set; }
        public string Comment { get; set; }
        public UserRequirement UserRequirement { get; set; }
        public CatalogItem CatalogItem { get; set; }
        public Classifier Classifier { get; set; }
        public CatalogActivity CatalogActivity { get; set; }
    }
}
