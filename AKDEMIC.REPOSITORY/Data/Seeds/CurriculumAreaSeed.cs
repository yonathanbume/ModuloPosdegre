using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class CurriculumAreaSeed
    {
        public static CurriculumArea[] Seed(AkdemicContext apiContext)
        {
            var result = new List<CurriculumArea>
            {
                new CurriculumArea { Name = "Ciencias e Ingeniería" },
                new CurriculumArea { Name = "Gestión y Ciencias Sociales" }
            };

            return result.ToArray();
        }
    }
}
