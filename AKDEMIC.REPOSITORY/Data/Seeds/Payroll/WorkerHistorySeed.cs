using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Payroll
{
    public class WorkerHistorySeed
    {
        public static WorkerHistory[] Seed(AkdemicContext context)
        {
            var result = context.Workers.Select(x => new WorkerHistory
            {
                StartDate = DateTime.UtcNow.AddYears(-1),
                WorkerId = x.Id,
                IsFinished = false
            });
            return result.ToArray();
        }
    }
}
