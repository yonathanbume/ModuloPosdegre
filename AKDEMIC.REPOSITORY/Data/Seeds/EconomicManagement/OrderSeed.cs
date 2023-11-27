using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.EconomicManagement
{
    public class OrderSeed
    {
        public static Order[] Seed(AkdemicContext context)
        {
            var result = new List<Order>()
            {
                new Order { Cost = 42069.00M, EndDate = DateTime.Now.AddDays(20), StartDate = DateTime.Now, Description = "En proceso", FileName = "", Path = "", Size = 0, Status = 1, Title = "Órden Nº 1" },
                new Order { Cost = 69000.00M, EndDate = DateTime.Now.AddDays(20), StartDate = DateTime.Now, Description = "Se resolvió", FileName = "", Path = "", Size = 0, Status = 2, Title = "Órden Nº 2" }
            };

            return result.ToArray();
        }
    }
}
