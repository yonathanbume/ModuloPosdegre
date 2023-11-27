using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ClassroomTypeSeed
    {
        public static ClassroomType[] Seed(AkdemicContext apiContext)
        {
            var result = new List<ClassroomType>()
            {
                new ClassroomType { Name = "Teoría" },
                new ClassroomType { Name = "Laboratorio" }
            };

            return result.ToArray();
        }
    }
}
