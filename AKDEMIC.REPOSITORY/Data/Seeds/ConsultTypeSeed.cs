using AKDEMIC.ENTITIES.Models.Laurassia;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ConsultTypeSeed
    {
        public static ConsultType[] Seed(AkdemicContext context)
        {
            var result = new List<ConsultType>()
            {
                new ConsultType { Description = "Problema con curso" },
                new ConsultType { Description = "Problema con evaluación" },
                new ConsultType { Description = "Problema con tarea" },
                new ConsultType { Description = "Problema con foro" },
                new ConsultType { Description = "Problema con lectura" }
            };
            return result.ToArray();
        }
    }
}
