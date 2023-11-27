using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class GroupSeed
    {
        public static Group[] Seed(AkdemicContext context)
        {

            var result = new List<Group>()
            {
                new Group { Code = "A", Vacancies = 40 },
                new Group { Code = "B", Vacancies = 40 },
                new Group { Code = "C", Vacancies = 40 }
            };

            return result.ToArray();
        }
    }
}
