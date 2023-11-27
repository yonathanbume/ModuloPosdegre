using AKDEMIC.ENTITIES.Models.Payroll;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Payroll
{
    public class WorkAreaSeed
    {
        public static WorkArea[] Seed(AkdemicContext context)
        {
            var result = new List<WorkArea>
            {
                new WorkArea { Name = "TI" },
                new WorkArea { Name = "RRHH" },
            };

            return result.ToArray();
        }
    }
}
