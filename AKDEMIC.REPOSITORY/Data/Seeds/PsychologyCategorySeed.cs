using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class PsychologyCategorySeed
    {
        public static PsychologyCategory[] Seed(AkdemicContext context)
        {

            var result = new List<PsychologyCategory>()
            {
                new PsychologyCategory() { Name = "CATEGORIA A"}
            };

            return result.ToArray();
        }
    }
}
