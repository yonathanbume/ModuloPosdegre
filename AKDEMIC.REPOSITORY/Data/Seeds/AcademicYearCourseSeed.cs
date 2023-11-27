using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class AcademicYearCourseSeed
    {
        public static AcademicYearCourse[] Seed(AkdemicContext context)
        {
            var courses = context.Courses.ToList();
            var curriculums = context.Curriculums.ToList();

            var result = new List<AcademicYearCourse>()
            {
                new AcademicYearCourse { AcademicYear =  1, CourseId = courses.FirstOrDefault(x => x.Name == "Lenguaje I").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2016).Id },
                new AcademicYearCourse { AcademicYear =  1, CourseId = courses.FirstOrDefault(x => x.Name == "Matemática I").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2016).Id },
                new AcademicYearCourse { AcademicYear =  1, CourseId = courses.FirstOrDefault(x => x.Name == "Programación I").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2016).Id },
                new AcademicYearCourse { AcademicYear =  1, CourseId = courses.FirstOrDefault(x => x.Name == "Ética y Ciudadanía").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2016).Id },
                new AcademicYearCourse { AcademicYear =  2, CourseId = courses.FirstOrDefault(x => x.Name == "Matemática II").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2016).Id },
                new AcademicYearCourse { AcademicYear =  2, CourseId = courses.FirstOrDefault(x => x.Name == "Lenguaje II").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2016).Id },
                new AcademicYearCourse { AcademicYear =  2, CourseId = courses.FirstOrDefault(x => x.Name == "Seminario I").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2016).Id },
                new AcademicYearCourse { AcademicYear =  3, CourseId = courses.FirstOrDefault(x => x.Name == "Estructuras de Datos").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2016).Id },
                new AcademicYearCourse { AcademicYear =  5, CourseId = courses.FirstOrDefault(x => x.Name == "Desarrollo de Aplicaciones").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2016).Id },
                new AcademicYearCourse { AcademicYear =  6, CourseId = courses.FirstOrDefault(x => x.Name == "Design Thinking").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2016).Id },
                new AcademicYearCourse { AcademicYear =  8, CourseId = courses.FirstOrDefault(x => x.Name == "Seminario II").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2016).Id },
                new AcademicYearCourse { AcademicYear =  9, CourseId = courses.FirstOrDefault(x => x.Name == "Taller de Proyecto I").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2016).Id },
                new AcademicYearCourse { AcademicYear = 10, CourseId = courses.FirstOrDefault(x => x.Name == "Taller de Proyecto II").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2016).Id },

                new AcademicYearCourse { AcademicYear = 1, CourseId = courses.FirstOrDefault(x => x.Name == "Lenguaje I").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2018).Id },
                new AcademicYearCourse { AcademicYear = 1, CourseId = courses.FirstOrDefault(x => x.Name == "Matemática I").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2018).Id },
                new AcademicYearCourse { AcademicYear = 1, CourseId = courses.FirstOrDefault(x => x.Name == "Programación I").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2018).Id },
                new AcademicYearCourse { AcademicYear = 1, CourseId = courses.FirstOrDefault(x => x.Name == "Ética y Ciudadanía").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2018).Id },
                new AcademicYearCourse { AcademicYear = 2, CourseId = courses.FirstOrDefault(x => x.Name == "Matemática II").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2018).Id },
                new AcademicYearCourse { AcademicYear = 2, CourseId = courses.FirstOrDefault(x => x.Name == "Lenguaje II").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2018).Id },
                new AcademicYearCourse { AcademicYear = 2, CourseId = courses.FirstOrDefault(x => x.Name == "Seminario I").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2018).Id },
                new AcademicYearCourse { AcademicYear = 3, CourseId = courses.FirstOrDefault(x => x.Name == "Estructuras de Datos").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2018).Id },
                new AcademicYearCourse { AcademicYear = 5, CourseId = courses.FirstOrDefault(x => x.Name == "Desarrollo de Aplicaciones").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2018).Id },
                new AcademicYearCourse { AcademicYear = 6, CourseId = courses.FirstOrDefault(x => x.Name == "Design Thinking").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2018).Id },
                new AcademicYearCourse { AcademicYear = 8, CourseId = courses.FirstOrDefault(x => x.Name == "Seminario II").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2018).Id },
                new AcademicYearCourse { AcademicYear = 9, CourseId = courses.FirstOrDefault(x => x.Name == "Taller de Proyecto I").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2018).Id },
                new AcademicYearCourse { AcademicYear =10, CourseId = courses.FirstOrDefault(x => x.Name == "Taller de Proyecto II").Id, CurriculumId = curriculums.FirstOrDefault(x => x.Year == 2018).Id }
            };
            return result.ToArray();
        }
    }
}