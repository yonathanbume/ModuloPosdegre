using System.Collections.Generic;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Scale
{
    public class WorkerCapPositionSeed
    {
        public static WorkerCapPosition[] Seed(AkdemicContext context)
        {
            var result = new List<WorkerCapPosition>()
            {
                new WorkerCapPosition { Code = "AUX", Name = "Auxiliar", Status = ConstantHelpers.STATES.ACTIVE },
                new WorkerCapPosition { Code = "TEC", Name = "Técnico", Status = ConstantHelpers.STATES.ACTIVE },
                new WorkerCapPosition { Code = "INGCON", Name = "Ingeniero de Control", Status = ConstantHelpers.STATES.ACTIVE },
                new WorkerCapPosition { Code = "ASISADMIN", Name = "Asistente Administrativo", Status = ConstantHelpers.STATES.ACTIVE },
                new WorkerCapPosition { Code = "JFISIS", Name = "Jefe de Sistemas", Status = ConstantHelpers.STATES.ACTIVE },
                new WorkerCapPosition { Code = "CONTA", Name = "Contador", Status = ConstantHelpers.STATES.ACTIVE },
            };

            return result.ToArray();
        }
    }
}
