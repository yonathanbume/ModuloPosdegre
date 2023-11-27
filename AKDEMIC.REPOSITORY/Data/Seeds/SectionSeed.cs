using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class SectionSeed
    {
        public static Section[] Seed(AkdemicContext context)
        {
            List<CourseTerm> courseTerms = context.CourseTerms
                .Include(x => x.Course)
                .Include(x => x.Term)
                .ToList();
            List<Group> groups = context.Groups.ToList();
            List<ENTITIES.Models.Generals.ApplicationUser> users = context.Users.ToList();

            List<Section> result = new List<Section>()
            {
                new Section { Code = "SW-51", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2020-1").Id },
                new Section { Code = "HU-21", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2020-1").Id },
                new Section { Code = "HU-22", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2020-1").Id },
                new Section { Code = "SW-35", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2020-1").Id },
                new Section { Code = "HU-01", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2020-1").Id, GroupId = groups[1].Id },
                new Section { Code = "HU-02", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2020-1").Id, GroupId = groups[0].Id },
                new Section { Code = "HU-03", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2020-1").Id, GroupId = groups[2].Id },
                new Section { Code = "IN-01", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2020-1").Id },
                new Section { Code = "IN-02", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2020-1").Id },
                new Section { Code = "IN-03", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2020-1").Id, GroupId = groups[2].Id },
                new Section { Code = "IN-04", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2020-1").Id, GroupId = groups[1].Id },
                new Section { Code = "IN-05", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2020-1").Id, GroupId = groups[0].Id },
                new Section { Code = "IN-21", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2020-1").Id },
                new Section { Code = "IN-22", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2020-1").Id },
                new Section { Code = "IN-23", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2020-1").Id },
                new Section { Code = "CC-01", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2020-1").Id, GroupId = groups[1].Id },
                new Section { Code = "CC-02", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2020-1").Id, GroupId = groups[2].Id },
                new Section { Code = "SW-01", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2020-1").Id, GroupId = groups[0].Id },
                new Section { Code = "HU-12", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2020-1").Id },
                new Section { Code = "HU-14", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2020-1").Id },

                new Section { Code = "SW-51", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2019-2").Id },
                new Section { Code = "HU-21", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2019-2").Id },
                new Section { Code = "HU-22", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2019-2").Id },
                new Section { Code = "SW-35", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2019-2").Id },
                new Section { Code = "HU-01", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2019-2").Id, GroupId = groups[1].Id },
                new Section { Code = "HU-02", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2019-2").Id, GroupId = groups[0].Id },
                new Section { Code = "HU-03", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2019-2").Id, GroupId = groups[2].Id },
                new Section { Code = "IN-01", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2019-2").Id },
                new Section { Code = "IN-02", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2019-2").Id },
                new Section { Code = "IN-03", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2019-2").Id, GroupId = groups[2].Id },
                new Section { Code = "IN-04", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2019-2").Id, GroupId = groups[1].Id },
                new Section { Code = "IN-05", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2019-2").Id, GroupId = groups[0].Id },
                new Section { Code = "IN-21", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2019-2").Id },
                new Section { Code = "IN-22", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2019-2").Id },
                new Section { Code = "IN-23", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2019-2").Id },
                new Section { Code = "CC-01", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2019-2").Id, GroupId = groups[1].Id },
                new Section { Code = "CC-02", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2019-2").Id, GroupId = groups[2].Id },
                new Section { Code = "SW-01", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2019-2").Id, GroupId = groups[0].Id },
                new Section { Code = "HU-12", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2019-2").Id },
                new Section { Code = "HU-14", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2019-2").Id },

                new Section { Code = "SW-51", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2019-1").Id },
                new Section { Code = "HU-21", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2019-1").Id },
                new Section { Code = "HU-22", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2019-1").Id },
                new Section { Code = "SW-35", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2019-1").Id },
                new Section { Code = "HU-01", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2019-1").Id, GroupId = groups[1].Id },
                new Section { Code = "HU-02", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2019-1").Id, GroupId = groups[0].Id },
                new Section { Code = "HU-03", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2019-1").Id, GroupId = groups[2].Id },
                new Section { Code = "IN-01", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2019-1").Id },
                new Section { Code = "IN-02", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2019-1").Id },
                new Section { Code = "IN-03", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2019-1").Id, GroupId = groups[2].Id },
                new Section { Code = "IN-04", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2019-1").Id, GroupId = groups[1].Id },
                new Section { Code = "IN-05", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2019-1").Id, GroupId = groups[0].Id },
                new Section { Code = "IN-21", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2019-1").Id },
                new Section { Code = "IN-22", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2019-1").Id },
                new Section { Code = "IN-23", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2019-1").Id },
                new Section { Code = "CC-01", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2019-1").Id, GroupId = groups[1].Id },
                new Section { Code = "CC-02", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2019-1").Id, GroupId = groups[2].Id },
                new Section { Code = "SW-01", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2019-1").Id, GroupId = groups[0].Id },
                new Section { Code = "HU-12", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2019-1").Id },
                new Section { Code = "HU-14", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2019-1").Id },

                new Section { Code = "SW-51", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-2").Id },
                new Section { Code = "HU-21", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2018-2").Id },
                new Section { Code = "HU-22", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2018-2").Id },
                new Section { Code = "SW-35", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-2").Id },
                new Section { Code = "HU-01", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-2").Id, GroupId = groups[1].Id },
                new Section { Code = "HU-02", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-2").Id, GroupId = groups[0].Id },
                new Section { Code = "HU-03", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-2").Id, GroupId = groups[2].Id },
                new Section { Code = "IN-01", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-2").Id },
                new Section { Code = "IN-02", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-2").Id },
                new Section { Code = "IN-03", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-2").Id, GroupId = groups[2].Id },
                new Section { Code = "IN-04", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-2").Id, GroupId = groups[1].Id },
                new Section { Code = "IN-05", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-2").Id, GroupId = groups[0].Id },
                new Section { Code = "IN-21", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-2").Id },
                new Section { Code = "IN-22", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-2").Id },
                new Section { Code = "IN-23", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-2").Id },
                new Section { Code = "CC-01", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-2").Id, GroupId = groups[1].Id },
                new Section { Code = "CC-02", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-2").Id, GroupId = groups[2].Id },
                new Section { Code = "SW-01", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-2").Id, GroupId = groups[0].Id },
                new Section { Code = "HU-12", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2018-2").Id },
                new Section { Code = "HU-14", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2018-2").Id },

                new Section { Code = "SW-51", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-1").Id },
                new Section { Code = "HU-21", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2018-1").Id },
                new Section { Code = "HU-22", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2018-1").Id },
                new Section { Code = "SW-35", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-1").Id },
                new Section { Code = "HU-01", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-1").Id, GroupId = groups[1].Id },
                new Section { Code = "HU-02", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-1").Id, GroupId = groups[0].Id },
                new Section { Code = "HU-03", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-1").Id, GroupId = groups[2].Id },
                new Section { Code = "IN-01", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-1").Id },
                new Section { Code = "IN-02", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-1").Id },
                new Section { Code = "IN-03", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-1").Id, GroupId = groups[2].Id },
                new Section { Code = "IN-04", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-1").Id, GroupId = groups[1].Id },
                new Section { Code = "IN-05", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-1").Id, GroupId = groups[0].Id },
                new Section { Code = "IN-21", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-1").Id },
                new Section { Code = "IN-22", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-1").Id },
                new Section { Code = "IN-23", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-1").Id },
                new Section { Code = "CC-01", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-1").Id, GroupId = groups[1].Id },
                new Section { Code = "CC-02", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-1").Id, GroupId = groups[2].Id },
                new Section { Code = "SW-01", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-1").Id, GroupId = groups[0].Id },
                new Section { Code = "HU-12", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2018-1").Id },
                new Section { Code = "HU-14", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2018-1").Id },

                new Section { Code = "SW-51", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2017-2").Id },
                new Section { Code = "HU-21", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2017-2").Id },
                new Section { Code = "HU-22", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2017-2").Id },
                new Section { Code = "SW-35", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2017-2").Id },
                new Section { Code = "HU-01", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2017-2").Id, GroupId = groups[1].Id },
                new Section { Code = "HU-02", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2017-2").Id, GroupId = groups[0].Id },
                new Section { Code = "HU-03", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2017-2").Id, GroupId = groups[2].Id },
                new Section { Code = "IN-01", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2017-2").Id },
                new Section { Code = "IN-02", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2017-2").Id },
                new Section { Code = "IN-03", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2017-2").Id, GroupId = groups[2].Id },
                new Section { Code = "IN-04", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2017-2").Id, GroupId = groups[1].Id },
                new Section { Code = "IN-05", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2017-2").Id, GroupId = groups[0].Id },
                new Section { Code = "IN-21", Vacancies = 30, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2017-2").Id },
                new Section { Code = "IN-22", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2017-2").Id },
                new Section { Code = "IN-23", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2017-2").Id },
                new Section { Code = "CC-01", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2017-2").Id, GroupId = groups[1].Id },
                new Section { Code = "CC-02", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2017-2").Id, GroupId = groups[2].Id },
                new Section { Code = "SW-01", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2017-2").Id, GroupId = groups[0].Id },
                new Section { Code = "HU-12", Vacancies = 20, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2017-2").Id },
                new Section { Code = "HU-14", Vacancies = 25, CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2017-2").Id }
            };

            return result.ToArray();

            //return new Section[] { };
        }
    }
}
