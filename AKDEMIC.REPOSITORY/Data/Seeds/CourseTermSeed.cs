using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class CourseTermSeed
    {
        public static CourseTerm[] Seed(AkdemicContext context)
        {
            List<Course> courses = context.Courses.ToList();
            List<Term> terms = context.Terms.ToList();

            List<CourseTerm> result = new List<CourseTerm>()
            {
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Desarrollo de Aplicaciones").Id, TermId = terms.SingleOrDefault(x => x.Name == "2020-1").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Estructuras de Datos").Id, TermId = terms.SingleOrDefault(x => x.Name == "2020-1").Id },
                new CourseTerm { Temary = "", WeekHours = 3, CourseId = courses.First(x => x.Name == "Ética y Ciudadanía").Id, TermId = terms.SingleOrDefault(x => x.Name == "2020-1").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Lenguaje I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2020-1").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Lenguaje II").Id, TermId = terms.SingleOrDefault(x => x.Name == "2020-1").Id },
                new CourseTerm { Temary = "", WeekHours = 8, CourseId = courses.First(x => x.Name == "Matemática I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2020-1").Id },
                new CourseTerm { Temary = "", WeekHours = 8, CourseId = courses.First(x => x.Name == "Matemática II").Id, TermId = terms.SingleOrDefault(x => x.Name == "2020-1").Id },
                new CourseTerm { Temary = "", WeekHours = 6, CourseId = courses.First(x => x.Name == "Programación I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2020-1").Id },
                new CourseTerm { Temary = "", WeekHours = 3, CourseId = courses.First(x => x.Name == "Seminario I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2020-1").Id },

                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Desarrollo de Aplicaciones").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-2").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Estructuras de Datos").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-2").Id },
                new CourseTerm { Temary = "", WeekHours = 3, CourseId = courses.First(x => x.Name == "Ética y Ciudadanía").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-2").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Lenguaje I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-2").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Lenguaje II").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-2").Id },
                new CourseTerm { Temary = "", WeekHours = 8, CourseId = courses.First(x => x.Name == "Matemática I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-2").Id },
                new CourseTerm { Temary = "", WeekHours = 8, CourseId = courses.First(x => x.Name == "Matemática II").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-2").Id },
                new CourseTerm { Temary = "", WeekHours = 6, CourseId = courses.First(x => x.Name == "Programación I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-2").Id },
                new CourseTerm { Temary = "", WeekHours = 3, CourseId = courses.First(x => x.Name == "Seminario I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-2").Id },

                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Desarrollo de Aplicaciones").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-1").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Estructuras de Datos").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-1").Id },
                new CourseTerm { Temary = "", WeekHours = 3, CourseId = courses.First(x => x.Name == "Ética y Ciudadanía").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-1").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Lenguaje I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-1").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Lenguaje II").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-1").Id },
                new CourseTerm { Temary = "", WeekHours = 8, CourseId = courses.First(x => x.Name == "Matemática I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-1").Id },
                new CourseTerm { Temary = "", WeekHours = 8, CourseId = courses.First(x => x.Name == "Matemática II").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-1").Id },
                new CourseTerm { Temary = "", WeekHours = 6, CourseId = courses.First(x => x.Name == "Programación I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-1").Id },
                new CourseTerm { Temary = "", WeekHours = 3, CourseId = courses.First(x => x.Name == "Seminario I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2019-1").Id },

                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Desarrollo de Aplicaciones").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-2").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Estructuras de Datos").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-2").Id },
                new CourseTerm { Temary = "", WeekHours = 3, CourseId = courses.First(x => x.Name == "Ética y Ciudadanía").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-2").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Lenguaje I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-2").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Lenguaje II").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-2").Id },
                new CourseTerm { Temary = "", WeekHours = 8, CourseId = courses.First(x => x.Name == "Matemática I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-2").Id },
                new CourseTerm { Temary = "", WeekHours = 8, CourseId = courses.First(x => x.Name == "Matemática II").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-2").Id },
                new CourseTerm { Temary = "", WeekHours = 6, CourseId = courses.First(x => x.Name == "Programación I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-2").Id },
                new CourseTerm { Temary = "", WeekHours = 3, CourseId = courses.First(x => x.Name == "Seminario I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-2").Id },

                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Desarrollo de Aplicaciones").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-1").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Estructuras de Datos").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-1").Id },
                new CourseTerm { Temary = "", WeekHours = 3, CourseId = courses.First(x => x.Name == "Ética y Ciudadanía").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-1").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Lenguaje I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-1").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Lenguaje II").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-1").Id },
                new CourseTerm { Temary = "", WeekHours = 8, CourseId = courses.First(x => x.Name == "Matemática I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-1").Id },
                new CourseTerm { Temary = "", WeekHours = 8, CourseId = courses.First(x => x.Name == "Matemática II").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-1").Id },
                new CourseTerm { Temary = "", WeekHours = 6, CourseId = courses.First(x => x.Name == "Programación I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-1").Id },
                new CourseTerm { Temary = "", WeekHours = 3, CourseId = courses.First(x => x.Name == "Seminario I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2018-1").Id },

                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Desarrollo de Aplicaciones").Id, TermId = terms.SingleOrDefault(x => x.Name == "2017-2").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Estructuras de Datos").Id, TermId = terms.SingleOrDefault(x => x.Name == "2017-2").Id },
                new CourseTerm { Temary = "", WeekHours = 3, CourseId = courses.First(x => x.Name == "Ética y Ciudadanía").Id, TermId = terms.SingleOrDefault(x => x.Name == "2017-2").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Lenguaje I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2017-2").Id },
                new CourseTerm { Temary = "", WeekHours = 4, CourseId = courses.First(x => x.Name == "Lenguaje II").Id, TermId = terms.SingleOrDefault(x => x.Name == "2017-2").Id },
                new CourseTerm { Temary = "", WeekHours = 8, CourseId = courses.First(x => x.Name == "Matemática I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2017-2").Id },
                new CourseTerm { Temary = "", WeekHours = 8, CourseId = courses.First(x => x.Name == "Matemática II").Id, TermId = terms.SingleOrDefault(x => x.Name == "2017-2").Id },
                new CourseTerm { Temary = "", WeekHours = 6, CourseId = courses.First(x => x.Name == "Programación I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2017-2").Id },
                new CourseTerm { Temary = "", WeekHours = 3, CourseId = courses.First(x => x.Name == "Seminario I").Id, TermId = terms.SingleOrDefault(x => x.Name == "2017-2").Id }
            };

            return result.ToArray();

            //return new CourseTerm[] { };
        }
    }
}
