using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKDEMIC.REPOSITORY.Data.Seeds.EconomicManagement
{
    public class RequirementSeed
    {
        public static Requirement[] Seed(AkdemicContext context)
        {
            var orders = context.Orders.ToList();
            var suppliers = context.Suppliers.ToList();
            var userRoles = context.UserRoles
                .Where(x => new[]
                {
                    ConstantHelpers.ROLES.COST_CENTER.ToUpper()
                }
                .Contains(x.Role.Name.ToUpper()))
                .ToList();

            var result = new List<Requirement>()
            {
                new Requirement { SupplierId = suppliers[0].Id, UserId = userRoles[0].UserId, Description = "Se solicitan más materiales", Need = "Hay falta de materiales en los almacenes", Subject = "Materiales para almacenes", CodeNumber = "01" },
                new Requirement { SupplierId = null, UserId = userRoles[0].UserId, Description = "Se solicitan más materiales", Need = "Hay falta de materiales en los almacenes", Subject = "Materiales para almacenes", CodeNumber = "02" }
            };

            return result.ToArray();
        }
    }
}
