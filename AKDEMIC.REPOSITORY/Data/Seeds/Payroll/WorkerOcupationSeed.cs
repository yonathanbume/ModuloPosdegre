using AKDEMIC.ENTITIES.Models.Payroll;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Payroll
{
    public class WorkerOcupationSeed
    {
        public static WorkerOcupation[] Seed(AkdemicContext context)
        {
            var result = new List<WorkerOcupation>()
            {
                new WorkerOcupation { Name = "Docente" },
                new WorkerOcupation { Name = "Asesor" }
            };

            return result.ToArray();
        }
    }
}
