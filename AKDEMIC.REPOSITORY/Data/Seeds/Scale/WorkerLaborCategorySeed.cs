using System.Collections.Generic;
using System.Linq;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Scale
{
    public class WorkerLaborCategorySeed
    {
        public static WorkerLaborCategory[] Seed(AkdemicContext context)
        {
            var workerLaborRegimes = context.WorkerLaborRegimes.ToList();

            var result = new List<WorkerLaborCategory>()
            {
                new WorkerLaborCategory { WorkerLaborRegimeId = workerLaborRegimes[0].Id, Name = "Auxiliar", Status = ConstantHelpers.STATES.ACTIVE, RelationId = "WLCA001" },
                new WorkerLaborCategory { WorkerLaborRegimeId = workerLaborRegimes[1].Id, Name = "Asociado", Status = ConstantHelpers.STATES.ACTIVE, RelationId = "WLCA002" },
                new WorkerLaborCategory { WorkerLaborRegimeId = workerLaborRegimes[2].Id, Name = "Jefe de Práctica", Status = ConstantHelpers.STATES.ACTIVE, RelationId = "WLCA003" },
                new WorkerLaborCategory { WorkerLaborRegimeId = workerLaborRegimes[2].Id, Name = "Principal", Status = ConstantHelpers.STATES.ACTIVE, RelationId = "WLCA004" },
            };
            return result.ToArray();
        }
    }
}
