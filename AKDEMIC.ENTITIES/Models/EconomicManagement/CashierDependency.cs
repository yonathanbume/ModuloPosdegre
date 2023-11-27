using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class CashierDependency
    {
        public Guid DependencyId { get; set; }
        public string UserId { get; set; }
        
        public Dependency Dependency { get; set; }
        public ApplicationUser User { get; set; }
    }
}
