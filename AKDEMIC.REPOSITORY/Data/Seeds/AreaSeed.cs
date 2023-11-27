using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class AreaSeed
    {
        public static Area[] Seed(AkdemicContext context)
        {
            var result = new List<Area>()
            {
                new Area { Name = "Ciencias" },
                new Area { Name = "Humanidades" },
                new Area { Name = "Tecnología" }
            };

            return result.ToArray();
        }
    }
}
