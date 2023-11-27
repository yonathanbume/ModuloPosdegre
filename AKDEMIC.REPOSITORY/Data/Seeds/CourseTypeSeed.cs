using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class CourseTypeSeed
    {
        public static CourseType[] Seed(AkdemicContext context)
        {
            var result = new List<CourseType>()
            {
                new CourseType { Name = "General" },
                new CourseType { Name = "Específico" },
                new CourseType { Name = "Especialidad" }
            };

            return result.ToArray();
        }
    }
}
