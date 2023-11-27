using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.EconomicManagement
{
    public class CurrentAccountSeed
    {
        public static CurrentAccount[] Seed(AkdemicContext context)
        {
            var result = new List<CurrentAccount>()
            {
                new CurrentAccount { Code = "CA001", Name = "Cuenta Actual 1", RelationId = "CA001" },
                new CurrentAccount { Code = "CA002", Name = "Cuenta Actual 2", RelationId = "CA002" },
                new CurrentAccount { Code = "CA003", Name = "Cuenta Actual 3", RelationId = "CA003" },
                new CurrentAccount { Code = "CA004", Name = "Cuenta Actual 4", RelationId = "CA004" },
                new CurrentAccount { Code = "CA005", Name = "Cuenta Actual 5", RelationId = "CA005" },
            };

            return result.ToArray();
        }
    }
}
