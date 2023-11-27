using AKDEMIC.ENTITIES.Models.JobExchange;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.JobExchange
{
    public class LanguageSeed
    {
        public static Language[] Seed(AkdemicContext context)
        {
            var result = new List<Language>()
            {
                new Language{ Name ="Español" },
                new Language{ Name ="Inglés" },
                new Language{ Name ="Francés" },
                new Language{ Name ="Italiano" },
            };

            return result.ToArray();
        }
    }
}
