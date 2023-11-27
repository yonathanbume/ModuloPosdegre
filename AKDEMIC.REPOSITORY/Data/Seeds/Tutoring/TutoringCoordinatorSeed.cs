using AKDEMIC.ENTITIES.Models.Tutoring;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Tutoring
{
    public class TutoringCoordinatorSeed
    {
        public static TutoringCoordinator[] Seed(AkdemicContext context)
        {
            //var careers = context.Careers.ToList();
            //var users = context.Users.ToList();
            var result = new List<TutoringCoordinator>();
            //{
            //    new TutoringCoordinator { CareerId = careers.First().Id, UserId = users.First(x => x.UserName == "coordinador.tutorias").Id }
            //};

            return result.ToArray();
        }
    }
}
