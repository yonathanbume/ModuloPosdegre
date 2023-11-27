using AKDEMIC.ENTITIES.Models.LanguageCenter;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class LanguageCourseSeed
    {
        public static LanguageCourse[] Seed(AkdemicContext _context)
        {
            List<LanguageCourse> result = new List<LanguageCourse>()
            {
                new LanguageCourse{ Name = "Ingles" },
                new LanguageCourse{ Name = "Frances" },
                new LanguageCourse{ Name = "Aleman" },
                new LanguageCourse{ Name = "Italiano" }
            };
            return result.ToArray();
        }
    }
}
