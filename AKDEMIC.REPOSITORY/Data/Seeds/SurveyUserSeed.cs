using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class SurveyUserSeed
    {
        public static SurveyUser[] Seed(AkdemicContext context)
        {
            var surveys = context.Survey.ToList();
            var users = context.Users.ToList();
            var result = new List<SurveyUser>()
            {
                new SurveyUser { SurveyId  = surveys[0].Id, UserId = users.Where(x => x.UserName == "alumno").SingleOrDefault().Id },
                new SurveyUser { SurveyId  = surveys[0].Id, UserId = users.Where(x => x.UserName == "smalumno").SingleOrDefault().Id }
            };

            return result.ToArray();
        }
    }
}
