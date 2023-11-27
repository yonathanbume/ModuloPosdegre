using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using Bogus;
using Bogus.DataSets;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds.EconomicManagement
{
    public class SupplierSeed
    {
        public static Supplier[] Seed(AkdemicContext context)
        {
            var company = new Company("es_MX");
            var randomizer = new Randomizer();
            var supplierCategory = context.SupplierCategories.First(x => x.Name == "CAT01");
            var userRoles = context.UserRoles.Where(x => x.Role.Name.ToUpper() == ConstantHelpers.ROLES.PROVIDER.ToUpper()).ToList();

            var result = new List<Supplier>()
            {
                new Supplier { SupplierCategoryId = supplierCategory.Id, UserId = userRoles[0].UserId, Name = company.CompanyName(), RUC = string.Join("", randomizer.Digits(11)) }
            };

            return result.ToArray();
        }
    }
}
