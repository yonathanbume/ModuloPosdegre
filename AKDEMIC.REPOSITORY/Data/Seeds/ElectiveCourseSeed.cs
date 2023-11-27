using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class ElectiveCourseSeed
    {
        public static ElectiveCourse[] Seed(AkdemicContext context)
        {
            var courses = context.Courses.ToList();
            var careers = context.Careers.ToList();

            var result = new List<ElectiveCourse>()
            {
                new ElectiveCourse { AcademicYearNumber = 3, CareerId = careers[0].Id, CourseId = courses[9].Id },
                new ElectiveCourse { AcademicYearNumber = 3, CareerId = careers[0].Id, CourseId = courses[10].Id }
            };

            return result.ToArray();
        }
    }
}
