using AKDEMIC.ENTITIES.Models.Payroll;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Payroll
{
    public class PaymentMethodSeed
    {
        public static PaymentMethod[] Seed(AkdemicContext context)
        {
            var result = new List<PaymentMethod>()
            {
                new PaymentMethod { Code = "DB", Name = "Depósito Bancario", IsActive = true },
                new PaymentMethod { Code = "CQ", Name = "Cheque", IsActive = true },
                new PaymentMethod { Code = "EE", Name = "En Efectivo", IsActive = true }
            };
            return result.ToArray();
        }
    }
}
