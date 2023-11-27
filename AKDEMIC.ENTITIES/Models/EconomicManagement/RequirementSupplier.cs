using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class RequirementSupplier
    {
        public Guid Id { get; set; }
        public Guid RequirementId { get; set; }
        public Guid SupplierId { get; set; }

        public Requirement Requirement { get; set; }
        public Supplier Supplier { get; set; }
    }
}
