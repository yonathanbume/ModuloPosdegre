using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.ConceptDistribution
{
    public class ConceptDistributionReportTemplate
    {
        public Guid Id { get; set; }
        public string Concept { get; set; }
        public List<ExcelReportConceptDistribution> ConceptDistributions { get; set; }
        public decimal Total { get; set; }
    }

    public class ExcelReportConceptDistribution
    {
        public Guid DependencyId { get; set; }
        public int Weight { get; set; }
        public string Dependency { get; set; }
        public decimal Amount { get; set; }
    }
}
