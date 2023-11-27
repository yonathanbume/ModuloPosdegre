using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class ConceptDistribution
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<ConceptDistributionDetail> ConceptDistributionDetails { get; set; }
        public ICollection<Concept> Concepts { get; set; }
    }
}
