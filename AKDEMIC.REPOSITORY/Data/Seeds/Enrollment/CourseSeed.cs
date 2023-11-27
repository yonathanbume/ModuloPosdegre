using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class CourseSeed
    {
        public static Course[] Seed(AkdemicContext context)
        {
            var areas = context.Areas.ToList();
            var careers = context.Careers.ToList();
            var courseComponents = context.CourseComponents.ToList();
            var courseTypes = context.CourseTypes.ToList();

            var result = new List<Course>()
            {
                new Course { AreaId = areas[0].Id, CareerId = careers[0].Id, CourseComponentId = courseComponents[0].Id, CourseTypeId = courseTypes[0].Id, Code = "MA101", Credits = 5, Name = "Matemática I", PracticalHours = 4, TheoreticalHours = 2 },
                new Course { AreaId = areas[0].Id, CareerId = careers[0].Id, CourseComponentId = courseComponents[0].Id, CourseTypeId = courseTypes[0].Id, Code = "MA102", Credits = 5, Name = "Matemática II", PracticalHours = 4, TheoreticalHours = 2 },
                new Course { AreaId = areas[1].Id, CareerId = careers[0].Id, CourseComponentId = courseComponents[0].Id, CourseTypeId = courseTypes[0].Id, Code = "HU03", Credits = 3, Name = "Lenguaje I", PracticalHours = 2, TheoreticalHours = 2 },
                new Course { AreaId = areas[1].Id, CareerId = careers[0].Id, CourseComponentId = courseComponents[0].Id, CourseTypeId = courseTypes[0].Id, Code = "HU04", Credits = 4, Name = "Lenguaje II", PracticalHours = 2, TheoreticalHours = 2 },
                new Course { AreaId = areas[2].Id, CareerId = careers[0].Id, CourseComponentId = courseComponents[1].Id, CourseTypeId = courseTypes[1].Id, Code = "CC47", Credits = 4, Name = "Programación I", PracticalHours = 4, TheoreticalHours = 2 },
                new Course { AreaId = areas[2].Id, CareerId = careers[0].Id, CourseComponentId = courseComponents[1].Id, CourseTypeId = courseTypes[1].Id, Code = "CC68", Credits = 4, Name = "Estructuras de Datos", PracticalHours = 4, TheoreticalHours = 2 },
                new Course { AreaId = areas[1].Id, CareerId = careers[0].Id, CourseComponentId = courseComponents[0].Id, CourseTypeId = courseTypes[0].Id, Code = "HU159", Credits = 3, Name = "Seminario I", PracticalHours = 2, TheoreticalHours = 2 },
                new Course { AreaId = areas[1].Id, CareerId = careers[0].Id, CourseComponentId = courseComponents[0].Id, CourseTypeId = courseTypes[0].Id, Code = "IN210", Credits = 3, Name = "Seminario II", PracticalHours = 2, TheoreticalHours = 2 },
                new Course { AreaId = areas[1].Id, CareerId = careers[0].Id, CourseComponentId = courseComponents[0].Id, CourseTypeId = courseTypes[0].Id, Code = "HU316", Credits = 2, Name = "Ética y Ciudadanía", PracticalHours = 0, TheoreticalHours = 2 },
                new Course { AreaId = areas[1].Id, CareerId = careers[0].Id, CourseComponentId = courseComponents[0].Id, CourseTypeId = courseTypes[0].Id, Code = "AD687", Credits = 2, Name = "Design Thinking", PracticalHours = 0, TheoreticalHours = 2 },
                new Course { AreaId = areas[2].Id, CareerId = careers[0].Id, CourseComponentId = courseComponents[1].Id, CourseTypeId = courseTypes[1].Id, Code = "SI422", Credits = 3, Name = "Desarrollo de Aplicaciones", PracticalHours = 2, TheoreticalHours = 3 },
                new Course { AreaId = areas[2].Id, CareerId = careers[0].Id, CourseComponentId = courseComponents[2].Id, CourseTypeId = courseTypes[2].Id, Code = "SI408", Credits = 5, Name = "Taller de Proyecto I", PracticalHours = 6, TheoreticalHours = 0 },
                new Course { AreaId = areas[2].Id, CareerId = careers[0].Id, CourseComponentId = courseComponents[2].Id, CourseTypeId = courseTypes[2].Id, Code = "SI410", Credits = 5, Name = "Taller de Proyecto II", PracticalHours = 6, TheoreticalHours = 0 }
            };

            return result.ToArray();
        }
    }
}
