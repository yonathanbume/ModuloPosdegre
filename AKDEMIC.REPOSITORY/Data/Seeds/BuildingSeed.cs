using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class BuildingSeed
    {
        public static Building[] Seed(AkdemicContext apiContext)
        {
            var campuses = apiContext.Campuses.ToList();

            var result = new List<Building>()
            {
                new Building { Name = "A", CampusId = campuses[0].Id },
                new Building { Name = "B", CampusId = campuses[0].Id },
                new Building { Name = "C", CampusId = campuses[0].Id },
                new Building { Name = "D", CampusId = campuses[0].Id },
                new Building { Name = "E", CampusId = campuses[0].Id },
                new Building { Name = "F", CampusId = campuses[0].Id },
                new Building { Name = "H", CampusId = campuses[0].Id },
                new Building { Name = "A", CampusId = campuses[1].Id },
                new Building { Name = "B", CampusId = campuses[1].Id }
            };

            return result.ToArray();
        }
    }
}
