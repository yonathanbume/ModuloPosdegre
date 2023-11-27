using AKDEMIC.ENTITIES.Models.Payroll;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Payroll
{
    public class PayrollTypeSeed
    {
        public static PayrollType[] Seed(AkdemicContext context)
        {
            var result = new List<PayrollType>()
            {
                new PayrollType { Code = "ACT", Name = "Activo" },
                new PayrollType { Code = "PNS", Name = "Pensionista" },
                new PayrollType { Code = "BNF", Name = "Beneficiario" },
                new PayrollType { Code = "OTR", Name = "Otros" },
                new PayrollType { Code = "DSJ", Name = "Descuento Judicial" },
                new PayrollType { Code = "JUD", Name = "Judicial"}
            };

            return result.ToArray();
        }
    }
}
