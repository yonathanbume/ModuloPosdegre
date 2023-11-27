using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class SorterSeed
    {
        public static Sorter[]  Seed(AkdemicContext context)
        {
            var result = new List<Sorter>()
            {
                new Sorter { Name = "Facultad" },
                new Sorter { Name = "Consejo de Facultad" },
                new Sorter { Name = "Canal" },
                new Sorter { Name = "Rectoral" },
                new Sorter { Name = "Consejo Universitario" },
                new Sorter { Name = "Asamblea Universitaria" },
                new Sorter { Name = "Vicerrectoral de Investigación" },
                new Sorter { Name = "Escuela de Posgrado" }
            };

            return result.ToArray();
        }
    }
}
