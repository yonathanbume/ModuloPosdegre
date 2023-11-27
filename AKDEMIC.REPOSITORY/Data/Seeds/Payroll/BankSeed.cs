using AKDEMIC.ENTITIES.Models.Payroll;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Payroll
{
    public class BankSeed
    {
        public static Bank[] Seed(AkdemicContext context)
        {
            var result = new List<Bank>()
            {
                new Bank { Code = "IBK", Name = "Interbank" },
                new Bank { Code = "BCP", Name = "Banco de Crédito del Perú" },
                new Bank { Code = "BBVA", Name = "BBVA Continental" },
                new Bank { Code = "PCB", Name = "Banco Pichincha" },
                new Bank { Code = "SCB", Name = "Scotiabank" }
            };

            return result.ToArray();
        }
    }
}
