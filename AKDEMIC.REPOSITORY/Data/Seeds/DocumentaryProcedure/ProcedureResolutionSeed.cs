using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.REPOSITORY.Data.Seeds.DocumentaryProcedure
{
    public class ProcedureResolutionSeed
    {
        public static ProcedureResolution[] Seed(AkdemicContext context)
        {
            var dependencies = context.Dependencies.ToList();
            var procedures = context.Procedures.ToList();

            var result = new List<ProcedureResolution>()
            {
                new ProcedureResolution { PresentationTerm = 10, ResolutionTerm = 20, ResolutionType = 1, DependencyId = dependencies[0].Id, ProcedureId = procedures[1].Id },
                new ProcedureResolution { PresentationTerm = 10, ResolutionTerm = 10, ResolutionType = 1, DependencyId = dependencies[0].Id, ProcedureId = procedures[1].Id },
                new ProcedureResolution { PresentationTerm = 10, ResolutionTerm = 20, ResolutionType = 1, DependencyId = dependencies[0].Id, ProcedureId = procedures[1].Id },
                new ProcedureResolution { PresentationTerm = 20, ResolutionTerm = 30, ResolutionType = 2, DependencyId = dependencies[1].Id, ProcedureId = procedures[2].Id }
            };

            return result.ToArray();
        }
    }
}
