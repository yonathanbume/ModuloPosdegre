using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Data.Seeds.EconomicManagement
{
    public class SupplierCategorySeed
    {
        public static SupplierCategory[] Seed(AkdemicContext context)
        {
            var result = new List<SupplierCategory>()
            {
                new SupplierCategory { Name = "CAT01" }
            };

            return result.ToArray();
        }
    }
}
