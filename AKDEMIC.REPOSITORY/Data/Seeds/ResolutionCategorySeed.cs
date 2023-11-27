using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ResolutionCategorySeed
    {
        public static ResolutionCategory[] Seed(AkdemicContext context)
        {
            var result = new List<ResolutionCategory>()
            {
                new ResolutionCategory { Name = "Comité" },
                new ResolutionCategory { Name = "Directivos" }
            };

            return result.ToArray();
        }
    }
}
