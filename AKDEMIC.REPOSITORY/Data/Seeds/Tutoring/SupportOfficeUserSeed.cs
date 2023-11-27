using AKDEMIC.ENTITIES.Models.Tutoring;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Tutoring
{
    public class SupportOfficeUserSeed
    {
        public static SupportOfficeUser[] Seed(AkdemicContext context)
        {
            var users = context.Users.ToList();
            var supportOffices = context.SupportOffices.ToList();
            var result = new List<SupportOfficeUser>()
            {
                new SupportOfficeUser { UserId = users.Where(x => x.UserName == "soporte.psicologia").First().Id, SupportOfficeId = supportOffices.First().Id }
            };
            return result.ToArray();
        }
    }
}
