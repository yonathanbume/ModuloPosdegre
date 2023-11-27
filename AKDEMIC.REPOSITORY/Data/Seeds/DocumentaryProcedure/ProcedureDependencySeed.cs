using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.REPOSITORY.Data.Seeds.DocumentaryProcedure
{
    public class ProcedureDependencySeed
    {
        public static ProcedureDependency[] Seed(AkdemicContext context)
        {
            var dependencies = context.Dependencies.ToList();
            var procedures = context.Procedures.ToList();

            var result = new List<ProcedureDependency>()
            {
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[0].Id },
                new ProcedureDependency { DependencyId = dependencies[1].Id, ProcedureId = procedures[1].Id },
                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[2].Id },
                new ProcedureDependency { DependencyId = dependencies[3].Id, ProcedureId = procedures[3].Id },
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[4].Id },
                new ProcedureDependency { DependencyId = dependencies[1].Id, ProcedureId = procedures[5].Id },
                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[6].Id },
                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[7].Id },
                new ProcedureDependency { DependencyId = dependencies[1].Id, ProcedureId = procedures[8].Id },
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[9].Id },
                new ProcedureDependency { DependencyId = dependencies[3].Id, ProcedureId = procedures[10].Id },
                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[11].Id },
                new ProcedureDependency { DependencyId = dependencies[1].Id, ProcedureId = procedures[12].Id },
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[13].Id },

                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[0].Id },
                new ProcedureDependency { DependencyId = dependencies[1].Id, ProcedureId = procedures[1].Id },
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[2].Id },
                new ProcedureDependency { DependencyId = dependencies[3].Id, ProcedureId = procedures[3].Id },
                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[4].Id },
                new ProcedureDependency { DependencyId = dependencies[1].Id, ProcedureId = procedures[5].Id },
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[6].Id },
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[7].Id },
                new ProcedureDependency { DependencyId = dependencies[1].Id, ProcedureId = procedures[8].Id },
                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[9].Id },
                new ProcedureDependency { DependencyId = dependencies[3].Id, ProcedureId = procedures[10].Id },
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[11].Id },
                new ProcedureDependency { DependencyId = dependencies[1].Id, ProcedureId = procedures[12].Id },
                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[13].Id },
                
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[0].Id },
                new ProcedureDependency { DependencyId = dependencies[1].Id, ProcedureId = procedures[1].Id },
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[2].Id },
                new ProcedureDependency { DependencyId = dependencies[1].Id, ProcedureId = procedures[3].Id },
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[4].Id },
                new ProcedureDependency { DependencyId = dependencies[1].Id, ProcedureId = procedures[5].Id },
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[6].Id },
                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[7].Id },
                new ProcedureDependency { DependencyId = dependencies[3].Id, ProcedureId = procedures[8].Id },
                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[9].Id },
                new ProcedureDependency { DependencyId = dependencies[3].Id, ProcedureId = procedures[10].Id },
                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[11].Id },
                new ProcedureDependency { DependencyId = dependencies[3].Id, ProcedureId = procedures[12].Id },
                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[13].Id },

                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[0].Id },
                new ProcedureDependency { DependencyId = dependencies[3].Id, ProcedureId = procedures[1].Id },
                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[2].Id },
                new ProcedureDependency { DependencyId = dependencies[3].Id, ProcedureId = procedures[3].Id },
                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[4].Id },
                new ProcedureDependency { DependencyId = dependencies[3].Id, ProcedureId = procedures[5].Id },
                new ProcedureDependency { DependencyId = dependencies[2].Id, ProcedureId = procedures[6].Id },
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[7].Id },
                new ProcedureDependency { DependencyId = dependencies[1].Id, ProcedureId = procedures[8].Id },
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[9].Id },
                new ProcedureDependency { DependencyId = dependencies[1].Id, ProcedureId = procedures[10].Id },
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[11].Id },
                new ProcedureDependency { DependencyId = dependencies[1].Id, ProcedureId = procedures[12].Id },
                new ProcedureDependency { DependencyId = dependencies[0].Id, ProcedureId = procedures[13].Id }
            };

            return result.ToArray();
        }
    }
}
