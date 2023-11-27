using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKDEMIC.REPOSITORY.Data.Seeds.EconomicManagement
{
    public class RequirementSupplierSeed
    {
        public static RequirementSupplier[] Seed(AkdemicContext context)
        {
            var requirements = context.Requirements.ToList();
            var suppliers = context.Suppliers.ToList();

            var result = new List<RequirementSupplier>()
            {
                new RequirementSupplier { RequirementId = requirements[0].Id, SupplierId = suppliers[0].Id },
                new RequirementSupplier { RequirementId = requirements[1].Id, SupplierId = suppliers[0].Id },
            };

            return result.ToArray();
        }
    }
}
