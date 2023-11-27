using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.JobExchange
{
    public class AbilitySeed
    {
        public static Ability[] Seed(AkdemicContext context)
        {
            var result = new List<Ability>()
            {
                new Ability{ Description ="ASP.NET Core 2.0", CreatedAt = DateTime.UtcNow },
                new Ability{ Description ="ASP.NET MVC", CreatedAt = DateTime.UtcNow },
                new Ability{ Description ="HTML5", CreatedAt = DateTime.UtcNow },
                new Ability{ Description ="Angular 7.0", CreatedAt = DateTime.UtcNow },
                new Ability{ Description ="Javascript", CreatedAt = DateTime.UtcNow },
            };

            return result.ToArray();
        }
    }
}
