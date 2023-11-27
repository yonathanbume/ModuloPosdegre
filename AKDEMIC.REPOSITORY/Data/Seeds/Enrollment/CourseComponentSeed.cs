using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Enrollment
{
    public class CourseComponentSeed
    {
        public static CourseComponent[] Seed(AkdemicContext context)
        {
            var result = new List<CourseComponent>()
            {
                new CourseComponent { Name = "Componente de 1 unidad", QuantityOfUnits = 1 },
                new CourseComponent { Name = "Componente de 2 unidades", QuantityOfUnits = 2 },
                new CourseComponent { Name = "Componente de 3 unidades", QuantityOfUnits = 3 },
                new CourseComponent { Name = "Componente de 4 unidades", QuantityOfUnits = 4 },
                new CourseComponent { Name = "Componente de 5 unidades", QuantityOfUnits = 5 },
                new CourseComponent { Name = "Componente de 6 unidades", QuantityOfUnits = 6 },
                new CourseComponent { Name = "Componente de 7 unidades", QuantityOfUnits = 7 },
                new CourseComponent { Name = "Componente de 8 unidades", QuantityOfUnits = 8 },
                new CourseComponent { Name = "Componente de 9 unidades", QuantityOfUnits = 9 }
            };

            return result.ToArray();
        }
    }
}
