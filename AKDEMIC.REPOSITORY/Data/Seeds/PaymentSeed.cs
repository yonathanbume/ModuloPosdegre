using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.EconomicManagement;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class PaymentSeed
    {
        public static Payment[] Seed(AkdemicContext context)
        {
            var result = new List<Payment>()
            {
               
            };

            return result.ToArray();
        }
    }
}
