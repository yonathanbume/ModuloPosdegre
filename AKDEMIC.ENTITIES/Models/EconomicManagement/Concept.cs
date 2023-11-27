using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class Concept : Entity, IKeyNumber, ITimestamp, ISoftDelete
    {
        public Guid Id { get; set; }
        public Guid AccountingPlanId { get; set; }
        public Guid ClassifierId { get; set; }
        public Guid? ConceptDistributionId { get; set; }
        public Guid CurrentAccountId { get; set; }
        public Guid DependencyId { get; set; }

        public decimal Amount { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        public string BankCode { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public bool IsDividedAmount { get; set; }
        public bool IsTaxed { get; set; }
        public bool IsTariff { get; set; }

        public bool IsFixedAmount { get; set; } = true;
        public bool IsEnabled { get; set; } = true;

        public string ContasoftConceptId { get; set; }

        public AccountingPlan AccountingPlan { get; set; }
        public Classifier Classifier { get; set; }
        public ConceptDistribution ConceptDistribution { get; set; }
        public CurrentAccount CurrentAccount { get; set; }
        public Dependency Dependency { get; set; }

        public ICollection<Payment> Payments { get; set; }
        public ICollection<ConceptHistory> ConceptHistories { get; set; }
    }
}
