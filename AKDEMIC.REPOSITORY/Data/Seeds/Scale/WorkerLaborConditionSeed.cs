using System.Collections.Generic;
using System.Linq;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Scale
{
    public class WorkerLaborConditionSeed
    {
        public static WorkerLaborCondition[] Seed(AkdemicContext context)
        {
            var workerLaborRegimes = context.WorkerLaborRegimes.ToList();

            var result = new List<WorkerLaborCondition>()
            {
                new WorkerLaborCondition { WorkerLaborRegimeId = workerLaborRegimes[0].Id, Name = "Contratado", Status = ConstantHelpers.STATES.ACTIVE, RelationId = "WLCO001" },
                new WorkerLaborCondition { WorkerLaborRegimeId = workerLaborRegimes[1].Id, Name = "Ordinario", Status = ConstantHelpers.STATES.ACTIVE, RelationId = "WLCO002" },
                new WorkerLaborCondition { WorkerLaborRegimeId = workerLaborRegimes[2].Id, Name = "Sin Condición Laboral", Status = ConstantHelpers.STATES.ACTIVE, RelationId = "WLCO003" }
            };

            return result.ToArray();
        }
    }
}
