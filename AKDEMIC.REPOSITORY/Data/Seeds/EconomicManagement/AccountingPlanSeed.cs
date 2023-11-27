using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.EconomicManagement
{
    public class AccountingPlanSeed
    {
        public static AccountingPlan[] Seed(AkdemicContext context)
        {
            var result = new List<AccountingPlan>()
            {
                new AccountingPlan { Code = "AP001", Name = "Plan Contable 1", RelationId = "AP001" },
                new AccountingPlan { Code = "AP002", Name = "Plan Contable 2", RelationId = "AP002" },
                new AccountingPlan { Code = "AP003", Name = "Plan Contable 3", RelationId = "AP003" },
                new AccountingPlan { Code = "AP004", Name = "Plan Contable 4", RelationId = "AP004" },
                new AccountingPlan { Code = "AP005", Name = "Plan Contable 5", RelationId = "AP005" },
            };

            return result.ToArray();
        }
    }
}
