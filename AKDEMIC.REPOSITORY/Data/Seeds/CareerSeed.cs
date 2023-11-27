using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class CareerSeed
    {
        public static Career[] Seed(AkdemicContext context)
        {
            var faculties = context.Faculties.ToList();

            var result = new List<Career>()
            {
                new Career { Name = "Ingeniería de Sistemas", FacultyId = faculties[0].Id },
                new Career { Name = "Administración", FacultyId = faculties[1].Id },
                new Career { Name = "Economía", FacultyId = faculties[0].Id },
                new Career { Name = "Derecho", FacultyId = faculties[0].Id },
                new Career { Name = "Enfermería", FacultyId = faculties[0].Id },
                new Career { Name = "Arquitectura", FacultyId = faculties[0].Id }
            };

            return result.ToArray();
        }
    }
}
