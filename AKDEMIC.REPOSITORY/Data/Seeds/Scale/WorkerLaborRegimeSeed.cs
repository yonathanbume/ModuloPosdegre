using System;
using System.Collections.Generic;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Scale
{
    public class WorkerLaborRegimeSeed
    {
        public static WorkerLaborRegime[] Seed(AkdemicContext context)
        {
            var result = new List<WorkerLaborRegime>()
            {
                new WorkerLaborRegime { Name = "Régimen CAS", PublicationDateRegulation = DateTime.UtcNow, Status = ConstantHelpers.STATES.ACTIVE, RelationId = "WLR001" },
                new WorkerLaborRegime { Name = "Régimen Público", PublicationDateRegulation = DateTime.UtcNow, Status = ConstantHelpers.STATES.ACTIVE, RelationId = "WLR002" },
                new WorkerLaborRegime { Name = "Régimen Privado", PublicationDateRegulation = DateTime.UtcNow, Status = ConstantHelpers.STATES.ACTIVE, RelationId = "WLR003" },
            };

            return result.ToArray();
        }
    }
}
