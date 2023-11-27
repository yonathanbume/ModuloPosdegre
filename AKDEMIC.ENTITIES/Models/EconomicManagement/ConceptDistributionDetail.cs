using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class ConceptDistributionDetail
    {
        public Guid Id { get; set; }
        public Guid ConceptDistributionId { get; set; }
        public Guid? DependencyId { get; set; }

        public bool IsUnit { get; set; }
        public int Weight { get; set; }

        public ConceptDistribution ConceptDistribution { get; set; }
        public Dependency Dependency { get; set; }
    }
}
