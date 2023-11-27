using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class FacultySeed
    {
        public static Faculty[] Seed(AkdemicContext context)
        {
            var result = new List<Faculty>
            {
                new Faculty { Abbreviation = "M", Code = "01", Name = "Medicina" },
                new Faculty { Abbreviation = "I",  Code = "02", Name = "Ingeniería" },
                new Faculty { Abbreviation = "A", Code = "03", Name = "Administración" },
                new Faculty { Abbreviation = "IC", Code = "04", Name = "Ingeniería Civil" },
                new Faculty { Abbreviation = "IME", Code = "05", Name = "Ingeniería Mecánica y Eléctrica" },
                new Faculty { Abbreviation = "CEH", Code = "06", Name = "Ciencias de la Educación y Humanidades" }
            };

            return result.ToArray();
        }
    }
}
