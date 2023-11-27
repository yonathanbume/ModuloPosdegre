using System;
using System.Collections.Generic;
using System.Linq;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.REPOSITORY.Data.Seeds.DocumentaryProcedure
{
    public class UserProcedureSeed
    {
        public static UserProcedure[] Seed(AkdemicContext context)
        {
            var dependencies = context.Dependencies.ToList();
            var procedures = context.Procedures.ToList();
            var procedureRequirements = context.ProcedureRequirements.ToList();
            var terms = context.Terms.ToList();
            var userRoles = context.UserRoles.Where(x => new []{ "Alumnos", "Docentes" }.Contains(x.Role.Name)).ToList();
            var users = context.Users.Where(x => userRoles.Contains(userRoles)).ToList();
            var random = new Random();
            var result = new List<UserProcedure>();

            for (var i = 0; i < terms.Count; i++)
            {
                var term = terms[i];

                if (term.Status == 0)
                {
                    continue;
                }

                var endDate = term.EndDate;
                var startDate = term.StartDate;

                for (var j = 0; j < procedures.Count; j++)
                {
                    var procedure = procedures[j];
                    var status = random.Next(ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED, ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS);
                    var subTotal = procedureRequirements.Where(x => x.ProcedureId == procedures[j].Id).Sum(x => x.Cost);
                    var total = subTotal * (decimal)1.18;
                    var randomDateTime = ConvertHelpers.RandomDateTime(startDate, endDate);
                    var userId = users[random.Next(users.Count)].Id;

                    var userProcedure = new UserProcedure
                    {
                        Status = status,
                        Comment = "Se solicita el trámite",
                        CreatedAt = randomDateTime,
                        UpdatedAt = randomDateTime,
                        DependencyId = dependencies[random.Next(dependencies.Count)].Id,
                        ProcedureId = procedure.Id,
                        TermId = term.Id,
                        UserId = userId
                    };

                    result.Add(userProcedure);
                }

                var currentTerm = context.Terms.Where(x => x.Status == 1).FirstOrDefault();
                var especificUsers = context.Users.Where(x => x.UserRoles.Any(s => s.Role.Name == "Alumnos")).ToList();
                var especificProcedure = context.Procedures.Where(x => x.Name == "Obtención del grado de bachiller").FirstOrDefault();

                for (int j = 1; j <= 5; j++)
                {
                    var extraUserProcedure = new UserProcedure
                    {
                        Status = ConstantHelpers.USER_PROCEDURES.STATUS.ACCEPTED,
                        Comment = "Se solicita el trámite",
                        CreatedAt = ConvertHelpers.RandomDateTime(startDate, endDate),
                        UpdatedAt = ConvertHelpers.RandomDateTime(startDate, endDate),
                        DependencyId = dependencies[random.Next(dependencies.Count)].Id,
                        ProcedureId = especificProcedure.Id,
                        TermId = currentTerm.Id,
                        UserId = especificUsers[j].Id
                    };

                    result.Add(extraUserProcedure);
                }
            }

            return result.ToArray();
        }
    }
}
