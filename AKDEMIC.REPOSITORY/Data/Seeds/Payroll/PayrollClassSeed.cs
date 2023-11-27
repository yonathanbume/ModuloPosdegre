using AKDEMIC.ENTITIES.Models.Payroll;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Payroll
{
    public class PayrollClassSeed
    {
        public static PayrollClass[]  Seed(AkdemicContext context)
        {
            var result = new List<PayrollClass>()
            {
                new PayrollClass { Code = "HB", Name = "Haberes" },
                new PayrollClass { Code = "CF", Name = "Cafae" },
                new PayrollClass { Code = "CAS", Name = "CAS" },
                new PayrollClass { Code = "FAG", Name = "FAG" },
                new PayrollClass { Code = "SG", Name = "Secigra" },
                new PayrollClass { Code = "PRP", Name = "Práctivas Pre-Profesionales" }
            };

            return result.ToArray();
        }
    }
}
