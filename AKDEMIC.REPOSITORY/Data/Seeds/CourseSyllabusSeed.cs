using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class CourseSyllabusSeed
    {
        public static CourseSyllabus[] Seed(AkdemicContext apiContext)
        {
            var courses = apiContext.Courses.ToList();
            var terms = apiContext.Terms.ToList();

            var result = new List<CourseSyllabus>()
            {
                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Programación I").Id, TermId = terms.First(t => t.Name == "2020-1").Id },
                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Matemática I").Id, TermId = terms.First(t => t.Name == "2020-1").Id },
                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Lenguaje I").Id, TermId = terms.First(t => t.Name == "2020-1").Id },

                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Programación I").Id, TermId = terms.First(t => t.Name == "2019-2").Id },
                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Matemática I").Id, TermId = terms.First(t => t.Name == "2019-2").Id },
                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Lenguaje I").Id, TermId = terms.First(t => t.Name == "2019-2").Id },

                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Programación I").Id, TermId = terms.First(t => t.Name == "2019-1").Id },
                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Matemática I").Id, TermId = terms.First(t => t.Name == "2019-1").Id },
                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Lenguaje I").Id, TermId = terms.First(t => t.Name == "2019-1").Id },

                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Programación I").Id, TermId = terms.First(t => t.Name == "2018-2").Id },
                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Matemática I").Id, TermId = terms.First(t => t.Name == "2018-2").Id },
                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Lenguaje I").Id, TermId = terms.First(t => t.Name == "2018-2").Id },

                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Programación I").Id, TermId = terms.First(t => t.Name == "2018-1").Id },
                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Matemática I").Id, TermId = terms.First(t => t.Name == "2018-1").Id },
                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Lenguaje I").Id, TermId = terms.First(t => t.Name == "2018-1").Id },

                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Programación I").Id, TermId = terms.First(t => t.Name == "2017-2").Id },
                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Matemática I").Id, TermId = terms.First(t => t.Name == "2017-2").Id },
                new CourseSyllabus { CourseId = courses.First(c => c.Name == "Lenguaje I").Id, TermId = terms.First(t => t.Name == "2017-2").Id }
            };

            return result.ToArray();
        }
    }
}
