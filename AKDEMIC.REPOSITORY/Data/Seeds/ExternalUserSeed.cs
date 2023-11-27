using System.Collections.Generic;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ExternalUserSeed
    {
        public static ExternalUser[] Seed(AkdemicContext context)
        {
            var result = new List<ExternalUser>()
            {
                new ExternalUser { BirthDate = ConvertHelpers.DatepickerToUtcDateTime("28/03/1990"), DocumentNumber = "70472045", DocumentType = 1, MaternalSurname = "Celis", Name = "Ivan", PaternalSurname = "Contreras", PhoneNumber = "945584234", WorkPosition = "Ingeniería de Software" },
                new ExternalUser { BirthDate = ConvertHelpers.DatepickerToUtcDateTime("28/03/1989"), DocumentNumber = "70472056", DocumentType = 1, MaternalSurname = "Fuentes", Name = "Miguel", PaternalSurname = "Gonzales", PhoneNumber = "945584231", WorkPosition = "Ingeniería de Software" },
                new ExternalUser { BirthDate = ConvertHelpers.DatepickerToUtcDateTime("28/03/1988"), DocumentNumber = "70472067", DocumentType = 1, MaternalSurname = "Moreno", Name = "Alejandro", PaternalSurname = "Yupanqui", PhoneNumber = "945584228", WorkPosition = "Ingeniería de Software" },
                new ExternalUser { BirthDate = ConvertHelpers.DatepickerToUtcDateTime("28/03/1987"), DocumentNumber = "70472078", DocumentType = 1, MaternalSurname = "Ore", Name = "Aldair", PaternalSurname = "Ore", PhoneNumber = "945584225", WorkPosition = "Ingeniería de Sistemas" },
                new ExternalUser { BirthDate = ConvertHelpers.DatepickerToUtcDateTime("28/03/1986"), DocumentNumber = "70472089", DocumentType = 1, MaternalSurname = "Vidal", Name = "Stephano", PaternalSurname = "Vidal", PhoneNumber = "945584222", WorkPosition = "Ingeniería de Sistemas" }
            };

            return result.ToArray();
        }
    }
}
