using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds.DocumentaryProcedure
{
    public class UserInternalProcedureSeed
    {
        public static UserInternalProcedure[] Seed(AkdemicContext context)
        {
            var dependencies = context.Dependencies.ToList();
            var internalProcedures = context.InternalProcedures.ToList();
            var users = context.Users.ToList();

            var result = new List<UserInternalProcedure>()
            {
                new UserInternalProcedure { DependencyId = dependencies[0].Id, InternalProcedureId = internalProcedures[0].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.biblioteca").Id, Status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.DISPATCHED },
                new UserInternalProcedure { DependencyId = dependencies[1].Id, InternalProcedureId = internalProcedures[1].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.centro.medico").Id, Status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.DISPATCHED },
                new UserInternalProcedure { DependencyId = dependencies[2].Id, InternalProcedureId = internalProcedures[2].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.mesa.partes").Id, Status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.DISPATCHED },
                new UserInternalProcedure { DependencyId = dependencies[3].Id, InternalProcedureId = internalProcedures[3].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.secretaria.general").Id, Status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.DISPATCHED },
                new UserInternalProcedure { DependencyId = dependencies[4].Id, InternalProcedureId = internalProcedures[4].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.tesoreria").Id, Status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.DISPATCHED },
                new UserInternalProcedure { DependencyId = dependencies[4].Id, InternalProcedureId = internalProcedures[5].Id, Status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED },
                new UserInternalProcedure { DependencyId = dependencies[3].Id, InternalProcedureId = internalProcedures[6].Id, Status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED },
                new UserInternalProcedure { DependencyId = dependencies[2].Id, InternalProcedureId = internalProcedures[7].Id, Status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.NOT_APPLICABLE },
                new UserInternalProcedure { DependencyId = dependencies[1].Id, InternalProcedureId = internalProcedures[8].Id, Status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED },
                new UserInternalProcedure { DependencyId = dependencies[0].Id, InternalProcedureId = internalProcedures[9].Id, Status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED }
            };

            return result.ToArray();
        }
    }
}
