using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds.DocumentaryProcedure
{
    public class UserExternalProcedureSeed
    {
        public static UserExternalProcedure[] Seed(AkdemicContext context)
        {
            var dependencies = context.Dependencies.ToList();
            var externalProcedures = context.ExternalProcedures.ToList();
            var externalUsers = context.ExternalUsers.ToList();
            var internalProcedures = context.InternalProcedures.ToList();
            var terms = context.Terms.ToList();

            var result = new List<UserExternalProcedure>()
            {
                new UserExternalProcedure { DependencyId = dependencies[0].Id, ExternalProcedureId = externalProcedures[0].Id, ExternalUserId = externalUsers[0].Id, Number = 1, TermId = terms[0].Id },
                new UserExternalProcedure { DependencyId = dependencies[1].Id, ExternalProcedureId = externalProcedures[1].Id, ExternalUserId = externalUsers[1].Id, Number = 2, TermId = terms[0].Id },
                new UserExternalProcedure { DependencyId = dependencies[2].Id, ExternalProcedureId = externalProcedures[2].Id, ExternalUserId = externalUsers[2].Id, Number = 3, TermId = terms[0].Id },
                new UserExternalProcedure { DependencyId = dependencies[3].Id, ExternalProcedureId = externalProcedures[3].Id, ExternalUserId = externalUsers[3].Id, Number = 4, TermId = terms[0].Id },
                new UserExternalProcedure { DependencyId = dependencies[4].Id, ExternalProcedureId = externalProcedures[4].Id, ExternalUserId = externalUsers[4].Id, Number = 5, TermId = terms[0].Id },

                new UserExternalProcedure { DependencyId = dependencies[0].Id, ExternalProcedureId = externalProcedures[0].Id, ExternalUserId = externalUsers[0].Id, Number = 6, TermId = terms[1].Id },
                new UserExternalProcedure { DependencyId = dependencies[1].Id, ExternalProcedureId = externalProcedures[1].Id, ExternalUserId = externalUsers[1].Id, Number = 7, TermId = terms[1].Id },
                new UserExternalProcedure { DependencyId = dependencies[2].Id, ExternalProcedureId = externalProcedures[2].Id, ExternalUserId = externalUsers[2].Id, Number = 8, TermId = terms[1].Id },
                new UserExternalProcedure { DependencyId = dependencies[3].Id, ExternalProcedureId = externalProcedures[3].Id, ExternalUserId = externalUsers[3].Id, Number = 9, TermId = terms[1].Id },
                new UserExternalProcedure { DependencyId = dependencies[4].Id, ExternalProcedureId = externalProcedures[4].Id, ExternalUserId = externalUsers[4].Id, Number = 10, TermId = terms[1].Id },

                new UserExternalProcedure { DependencyId = dependencies[0].Id, ExternalProcedureId = externalProcedures[0].Id, ExternalUserId = externalUsers[0].Id, Number = 11, TermId = terms[2].Id },
                new UserExternalProcedure { DependencyId = dependencies[1].Id, ExternalProcedureId = externalProcedures[1].Id, ExternalUserId = externalUsers[1].Id, Number = 12, TermId = terms[2].Id },
                new UserExternalProcedure { DependencyId = dependencies[2].Id, ExternalProcedureId = externalProcedures[2].Id, ExternalUserId = externalUsers[2].Id, Number = 13, TermId = terms[2].Id },
                new UserExternalProcedure { DependencyId = dependencies[3].Id, ExternalProcedureId = externalProcedures[3].Id, ExternalUserId = externalUsers[3].Id, Number = 14, TermId = terms[2].Id },
                new UserExternalProcedure { DependencyId = dependencies[4].Id, ExternalProcedureId = externalProcedures[4].Id, ExternalUserId = externalUsers[4].Id, Number = 15, TermId = terms[2].Id },

                new UserExternalProcedure { DependencyId = dependencies[0].Id, ExternalProcedureId = externalProcedures[0].Id, ExternalUserId = externalUsers[0].Id, Number = 16, TermId = terms[3].Id },
                new UserExternalProcedure { DependencyId = dependencies[1].Id, ExternalProcedureId = externalProcedures[1].Id, ExternalUserId = externalUsers[1].Id, Number = 17, TermId = terms[3].Id },
                new UserExternalProcedure { DependencyId = dependencies[2].Id, ExternalProcedureId = externalProcedures[2].Id, ExternalUserId = externalUsers[2].Id, Number = 18, TermId = terms[3].Id },
                new UserExternalProcedure { DependencyId = dependencies[3].Id, ExternalProcedureId = externalProcedures[3].Id, ExternalUserId = externalUsers[3].Id, Number = 19, TermId = terms[3].Id },
                new UserExternalProcedure { DependencyId = dependencies[4].Id, ExternalProcedureId = externalProcedures[4].Id, ExternalUserId = externalUsers[4].Id, Number = 20, TermId = terms[3].Id },

                new UserExternalProcedure { DependencyId = dependencies[0].Id, ExternalProcedureId = externalProcedures[0].Id, ExternalUserId = externalUsers[0].Id, InternalProcedureId = internalProcedures[0].Id, Number = 21, TermId = terms[0].Id },
                new UserExternalProcedure { DependencyId = dependencies[1].Id, ExternalProcedureId = externalProcedures[1].Id, ExternalUserId = externalUsers[1].Id, InternalProcedureId = internalProcedures[1].Id, Number = 22, TermId = terms[0].Id },
                new UserExternalProcedure { DependencyId = dependencies[2].Id, ExternalProcedureId = externalProcedures[2].Id, ExternalUserId = externalUsers[2].Id, InternalProcedureId = internalProcedures[2].Id, Number = 23, TermId = terms[0].Id },
                new UserExternalProcedure { DependencyId = dependencies[3].Id, ExternalProcedureId = externalProcedures[3].Id, ExternalUserId = externalUsers[3].Id, InternalProcedureId = internalProcedures[3].Id, Number = 24, TermId = terms[0].Id },
                new UserExternalProcedure { DependencyId = dependencies[4].Id, ExternalProcedureId = externalProcedures[4].Id, ExternalUserId = externalUsers[4].Id, InternalProcedureId = internalProcedures[4].Id, Number = 25, TermId = terms[0].Id },

                new UserExternalProcedure { DependencyId = dependencies[0].Id, ExternalProcedureId = externalProcedures[0].Id, ExternalUserId = externalUsers[0].Id, InternalProcedureId = internalProcedures[0].Id, Number = 26, TermId = terms[1].Id },
                new UserExternalProcedure { DependencyId = dependencies[1].Id, ExternalProcedureId = externalProcedures[1].Id, ExternalUserId = externalUsers[1].Id, InternalProcedureId = internalProcedures[1].Id, Number = 27, TermId = terms[1].Id },
                new UserExternalProcedure { DependencyId = dependencies[2].Id, ExternalProcedureId = externalProcedures[2].Id, ExternalUserId = externalUsers[2].Id, InternalProcedureId = internalProcedures[2].Id, Number = 28, TermId = terms[1].Id },
                new UserExternalProcedure { DependencyId = dependencies[3].Id, ExternalProcedureId = externalProcedures[3].Id, ExternalUserId = externalUsers[3].Id, InternalProcedureId = internalProcedures[3].Id, Number = 29, TermId = terms[1].Id },
                new UserExternalProcedure { DependencyId = dependencies[4].Id, ExternalProcedureId = externalProcedures[4].Id, ExternalUserId = externalUsers[4].Id, InternalProcedureId = internalProcedures[4].Id, Number = 30, TermId = terms[1].Id },

                new UserExternalProcedure { DependencyId = dependencies[0].Id, ExternalProcedureId = externalProcedures[0].Id, ExternalUserId = externalUsers[0].Id, InternalProcedureId = internalProcedures[0].Id, Number = 31, TermId = terms[2].Id },
                new UserExternalProcedure { DependencyId = dependencies[1].Id, ExternalProcedureId = externalProcedures[1].Id, ExternalUserId = externalUsers[1].Id, InternalProcedureId = internalProcedures[1].Id, Number = 32, TermId = terms[2].Id },
                new UserExternalProcedure { DependencyId = dependencies[2].Id, ExternalProcedureId = externalProcedures[2].Id, ExternalUserId = externalUsers[2].Id, InternalProcedureId = internalProcedures[2].Id, Number = 33, TermId = terms[2].Id },
                new UserExternalProcedure { DependencyId = dependencies[3].Id, ExternalProcedureId = externalProcedures[3].Id, ExternalUserId = externalUsers[3].Id, InternalProcedureId = internalProcedures[3].Id, Number = 34, TermId = terms[2].Id },
                new UserExternalProcedure { DependencyId = dependencies[4].Id, ExternalProcedureId = externalProcedures[4].Id, ExternalUserId = externalUsers[4].Id, InternalProcedureId = internalProcedures[4].Id, Number = 35, TermId = terms[2].Id },

                new UserExternalProcedure { DependencyId = dependencies[0].Id, ExternalProcedureId = externalProcedures[0].Id, ExternalUserId = externalUsers[0].Id, InternalProcedureId = internalProcedures[0].Id, Number = 36, TermId = terms[3].Id },
                new UserExternalProcedure { DependencyId = dependencies[1].Id, ExternalProcedureId = externalProcedures[1].Id, ExternalUserId = externalUsers[1].Id, InternalProcedureId = internalProcedures[1].Id, Number = 37, TermId = terms[3].Id },
                new UserExternalProcedure { DependencyId = dependencies[2].Id, ExternalProcedureId = externalProcedures[2].Id, ExternalUserId = externalUsers[2].Id, InternalProcedureId = internalProcedures[2].Id, Number = 38, TermId = terms[3].Id },
                new UserExternalProcedure { DependencyId = dependencies[3].Id, ExternalProcedureId = externalProcedures[3].Id, ExternalUserId = externalUsers[3].Id, InternalProcedureId = internalProcedures[3].Id, Number = 39, TermId = terms[3].Id },
                new UserExternalProcedure { DependencyId = dependencies[4].Id, ExternalProcedureId = externalProcedures[4].Id, ExternalUserId = externalUsers[4].Id, InternalProcedureId = internalProcedures[4].Id, Number = 40, TermId = terms[3].Id }
            };

            return result.ToArray();
        }
    }
}
