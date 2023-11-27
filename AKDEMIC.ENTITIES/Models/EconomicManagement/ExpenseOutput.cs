using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class ExpenseOutput : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public bool ExpenseReport { get; set; }

        public string UserId { get; set; }

        public Guid DependencyId { get; set; }

        public ApplicationUser User { get; set; }

        public Dependency Dependency { get; set; }
    }
}
