using System.Collections.Generic;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Scale
{
    public class WorkerPositionClassificationSeed
    {
        public static WorkerPositionClassification[] Seed(AkdemicContext context)
        {
            var result = new List<WorkerPositionClassification>()
            {
                new WorkerPositionClassification { Name = "Cargo de confianza", Status = ConstantHelpers.STATES.ACTIVE },
                new WorkerPositionClassification { Name = "Régimen especial", Status = ConstantHelpers.STATES.ACTIVE },
                new WorkerPositionClassification { Name = "Servidor público", Status = ConstantHelpers.STATES.ACTIVE }
            };

            return result.ToArray();
        }
    }
}
