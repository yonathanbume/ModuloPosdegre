using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.REPOSITORY.Data.Seeds.DocumentaryProcedure
{
    public class ProcedureRoleSeed
    {
        public static ProcedureRole[] Seed(AkdemicContext context)
        {
            var procedures = context.Procedures.ToList();
            var roles = context.Roles.ToList();

            var result = new List<ProcedureRole>()
            {
                new ProcedureRole() { ProcedureId = procedures[0].Id, RoleId = roles[0].Id },
                new ProcedureRole() { ProcedureId = procedures[0].Id, RoleId = roles[1].Id },
                new ProcedureRole() { ProcedureId = procedures[1].Id, RoleId = roles[0].Id },
                new ProcedureRole() { ProcedureId = procedures[1].Id, RoleId = roles[1].Id }
            };

            return result.ToArray();
        }
    }
}
