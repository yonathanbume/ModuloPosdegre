using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKDEMIC.REPOSITORY.Data.Seeds.DocumentaryProcedure
{
    public class UserRequirementSeed
    {
        public static UserRequirement[] Seed(AkdemicContext context)
        {
            var requirements = context.Requirements.ToList();
            var roles = context.Roles.Where(x => new[]
            {
                ConstantHelpers.ROLES.PROCUREMENTS_UNIT.ToUpper(),
                ConstantHelpers.ROLES.SELECTION_PROCESS_UNIT.ToUpper()
            }
            .Contains(x.Name.ToUpper()))
            .ToList();

            var result = new List<UserRequirement>()
            {
                new UserRequirement { RequirementId = requirements[0].Id, RoleId = roles[0].Id, Status = 2 },
                new UserRequirement { RequirementId = requirements[1].Id, RoleId = roles[1].Id, Status = 1 }
            };

            return result.ToArray();
        }
    }
}
