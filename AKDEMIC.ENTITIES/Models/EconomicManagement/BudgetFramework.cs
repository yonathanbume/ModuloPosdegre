using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class BudgetFramework
    {
        public Guid Id { get; set; }
        public Guid DependencyId { get; set; }
        public decimal Budget { get; set; }
        public int Year { get; set; }

        public Dependency Dependency { get; set; }
    }

}
