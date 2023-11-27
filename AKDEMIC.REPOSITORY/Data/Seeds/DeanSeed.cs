using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class DeanSeed
    {
        public static Dean[] Seed(AkdemicContext context)
        {
            var users = context.Users.ToList();
            var faculties = context.Faculties.ToList();

            var result = new List<Dean>()
            {
                new Dean { UserId = users.FirstOrDefault(x => x.UserName == "decano").Id, FacultyId = faculties.First().Id }
            };
            return result.ToArray();
        }
    }
}
