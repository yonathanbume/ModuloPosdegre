using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.EconomicManagement;

namespace AKDEMIC.REPOSITORY.Data.Seeds.EconomicManagement
{
    public class ConceptSeed
    {
        public static Concept[] Seed(AkdemicContext context)
        {
            var accountingPlans = context.AccountingPlans.ToList();
            var classifiers = context.Classifiers.ToList();
            var currentAccounts = context.CurrentAccounts.ToList();
            var dependencies = context.Dependencies.ToList();

            var result = new List<Concept>
            {
                new Concept { AccountingPlanId = accountingPlans[0].Id, ClassifierId = classifiers[0].Id, CurrentAccountId = currentAccounts[0].Id, DependencyId = dependencies[0].Id, Code = "TEST", Description = "Concepto de Prueba", Amount = 10.00M },
            };

            return result.ToArray();
        }
    }
}
