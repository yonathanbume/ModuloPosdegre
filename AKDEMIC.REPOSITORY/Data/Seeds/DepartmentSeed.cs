using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class DepartmentSeed
    {
        public static Department[] Seed(AkdemicContext context)
        {
            var country = context.Countries.First(x => x.Code == "PE");

            var result = new List<Department>()
            {
                new Department { Name = "Amazonas", CountryId = country.Id },
                new Department { Name = "Áncash", CountryId = country.Id },
                new Department { Name = "Apurímac", CountryId = country.Id },
                new Department { Name = "Arequipa", CountryId = country.Id },
                new Department { Name = "Ayacucho", CountryId = country.Id },
                new Department { Name = "Cajamarca", CountryId = country.Id },
                new Department { Name = "Callao", CountryId = country.Id },
                new Department { Name = "Cusco", CountryId = country.Id },
                new Department { Name = "Huancavelica", CountryId = country.Id },
                new Department { Name = "Huánuco", CountryId = country.Id },
                new Department { Name = "Ica", CountryId = country.Id },
                new Department { Name = "Junín", CountryId = country.Id },
                new Department { Name = "La Libertad", CountryId = country.Id },
                new Department { Name = "Lambayeque", CountryId = country.Id },
                new Department { Name = "Lima", CountryId = country.Id },
                new Department { Name = "Loreto", CountryId = country.Id },
                new Department { Name = "Madre de Dios", CountryId = country.Id },
                new Department { Name = "Moquegua", CountryId = country.Id },
                new Department { Name = "Pasco", CountryId = country.Id },
                new Department { Name = "Piura", CountryId = country.Id },
                new Department { Name = "Puno", CountryId = country.Id },
                new Department { Name = "San Martín", CountryId = country.Id },
                new Department { Name = "Tacna", CountryId = country.Id },
                new Department { Name = "Tumbes", CountryId = country.Id },
                new Department { Name = "Ucayali", CountryId = country.Id }
            };

            return result.ToArray();
        }
    }
}
