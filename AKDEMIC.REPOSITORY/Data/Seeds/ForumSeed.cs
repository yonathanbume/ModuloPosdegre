using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ForumSeed
    {
        public static Forum[] Seed(AkdemicContext context)
        {
            var result = new List<Forum>()
            {
                new Forum { Name = "General", Description = "Foro general de la universidad", Active = true },
                new Forum { Name = "Eventos", Description = "Eventos realizados por la universidad", Active = true }
            };

            return result.ToArray();
        }
    }
}
