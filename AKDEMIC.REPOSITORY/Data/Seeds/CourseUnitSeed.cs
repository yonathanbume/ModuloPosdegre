using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class CourseUnitSeed
    {
        public static CourseUnit[] Seed(AkdemicContext apiContext)
        {
            var courseSyllabuses = apiContext.CourseSyllabus.ToList();

            var result = new List<CourseUnit>()
            {
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Lenguaje I" && s.Term.Name == "2020-1").Id, WeekNumberStart = 1, WeekNumberEnd = 7, Name = "Comprensión Lectora para la Producción" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Lenguaje I" && s.Term.Name == "2020-1").Id, WeekNumberStart = 9, WeekNumberEnd = 15, Name = "Producción de Textos Académicos" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2020-1").Id, WeekNumberStart = 3, WeekNumberEnd = 6, Name = "Cónicas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2020-1").Id, WeekNumberStart = 6, WeekNumberEnd = 10, Name = "Funciones" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2020-1").Id, WeekNumberStart = 11, WeekNumberEnd = 12, Name = "Funciones Exponenciales y Logarítmicas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2020-1").Id, WeekNumberStart = 13, WeekNumberEnd = 15, Name = "Funciones Trigonométricas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2020-1").Id, WeekNumberStart = 1, WeekNumberEnd = 3, Name = "Matrices y Sistemas de Ecuaciones" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2020-1").Id, WeekNumberStart = 12, WeekNumberEnd = 15, Name = "Arreglos" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2020-1").Id, WeekNumberStart = 1, WeekNumberEnd = 3, Name = "Conceptos Básicos de Programación" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2020-1").Id, WeekNumberStart = 4, WeekNumberEnd = 7, Name = "Estructuras de Control" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2020-1").Id, WeekNumberStart = 9, WeekNumberEnd = 11, Name = "Memoria Dinámica" },

                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Lenguaje I" && s.Term.Name == "2019-2").Id, WeekNumberStart = 1, WeekNumberEnd = 7, Name = "Comprensión Lectora para la Producción" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Lenguaje I" && s.Term.Name == "2019-2").Id, WeekNumberStart = 9, WeekNumberEnd = 15, Name = "Producción de Textos Académicos" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2019-2").Id, WeekNumberStart = 3, WeekNumberEnd = 6, Name = "Cónicas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2019-2").Id, WeekNumberStart = 6, WeekNumberEnd = 10, Name = "Funciones" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2019-2").Id, WeekNumberStart = 11, WeekNumberEnd = 12, Name = "Funciones Exponenciales y Logarítmicas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2019-2").Id, WeekNumberStart = 13, WeekNumberEnd = 15, Name = "Funciones Trigonométricas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2019-2").Id, WeekNumberStart = 1, WeekNumberEnd = 3, Name = "Matrices y Sistemas de Ecuaciones" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2019-2").Id, WeekNumberStart = 12, WeekNumberEnd = 15, Name = "Arreglos" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2019-2").Id, WeekNumberStart = 1, WeekNumberEnd = 3, Name = "Conceptos Básicos de Programación" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2019-2").Id, WeekNumberStart = 4, WeekNumberEnd = 7, Name = "Estructuras de Control" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2019-2").Id, WeekNumberStart = 9, WeekNumberEnd = 11, Name = "Memoria Dinámica" },

                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Lenguaje I" && s.Term.Name == "2019-1").Id, WeekNumberStart = 1, WeekNumberEnd = 7, Name = "Comprensión Lectora para la Producción" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Lenguaje I" && s.Term.Name == "2019-1").Id, WeekNumberStart = 9, WeekNumberEnd = 15, Name = "Producción de Textos Académicos" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2019-1").Id, WeekNumberStart = 3, WeekNumberEnd = 6, Name = "Cónicas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2019-1").Id, WeekNumberStart = 6, WeekNumberEnd = 10, Name = "Funciones" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2019-1").Id, WeekNumberStart = 11, WeekNumberEnd = 12, Name = "Funciones Exponenciales y Logarítmicas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2019-1").Id, WeekNumberStart = 13, WeekNumberEnd = 15, Name = "Funciones Trigonométricas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2019-1").Id, WeekNumberStart = 1, WeekNumberEnd = 3, Name = "Matrices y Sistemas de Ecuaciones" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2019-1").Id, WeekNumberStart = 12, WeekNumberEnd = 15, Name = "Arreglos" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2019-1").Id, WeekNumberStart = 1, WeekNumberEnd = 3, Name = "Conceptos Básicos de Programación" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2019-1").Id, WeekNumberStart = 4, WeekNumberEnd = 7, Name = "Estructuras de Control" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2019-1").Id, WeekNumberStart = 9, WeekNumberEnd = 11, Name = "Memoria Dinámica" },

                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Lenguaje I" && s.Term.Name == "2018-2").Id, WeekNumberStart = 1, WeekNumberEnd = 7, Name = "Comprensión Lectora para la Producción" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Lenguaje I" && s.Term.Name == "2018-2").Id, WeekNumberStart = 9, WeekNumberEnd = 15, Name = "Producción de Textos Académicos" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2018-2").Id, WeekNumberStart = 3, WeekNumberEnd = 6, Name = "Cónicas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2018-2").Id, WeekNumberStart = 6, WeekNumberEnd = 10, Name = "Funciones" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2018-2").Id, WeekNumberStart = 11, WeekNumberEnd = 12, Name = "Funciones Exponenciales y Logarítmicas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2018-2").Id, WeekNumberStart = 13, WeekNumberEnd = 15, Name = "Funciones Trigonométricas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2018-2").Id, WeekNumberStart = 1, WeekNumberEnd = 3, Name = "Matrices y Sistemas de Ecuaciones" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2018-2").Id, WeekNumberStart = 12, WeekNumberEnd = 15, Name = "Arreglos" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2018-2").Id, WeekNumberStart = 1, WeekNumberEnd = 3, Name = "Conceptos Básicos de Programación" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2018-2").Id, WeekNumberStart = 4, WeekNumberEnd = 7, Name = "Estructuras de Control" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2018-2").Id, WeekNumberStart = 9, WeekNumberEnd = 11, Name = "Memoria Dinámica" },

                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Lenguaje I" && s.Term.Name == "2018-1").Id, WeekNumberStart = 1, WeekNumberEnd = 7, Name = "Comprensión Lectora para la Producción" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Lenguaje I" && s.Term.Name == "2018-1").Id, WeekNumberStart = 9, WeekNumberEnd = 15, Name = "Producción de Textos Académicos" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2018-1").Id, WeekNumberStart = 3, WeekNumberEnd = 6, Name = "Cónicas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2018-1").Id, WeekNumberStart = 6, WeekNumberEnd = 10, Name = "Funciones" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2018-1").Id, WeekNumberStart = 11, WeekNumberEnd = 12, Name = "Funciones Exponenciales y Logarítmicas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2018-1").Id, WeekNumberStart = 13, WeekNumberEnd = 15, Name = "Funciones Trigonométricas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2018-1").Id, WeekNumberStart = 1, WeekNumberEnd = 3, Name = "Matrices y Sistemas de Ecuaciones" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2018-1").Id, WeekNumberStart = 12, WeekNumberEnd = 15, Name = "Arreglos" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2018-1").Id, WeekNumberStart = 1, WeekNumberEnd = 3, Name = "Conceptos Básicos de Programación" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2018-1").Id, WeekNumberStart = 4, WeekNumberEnd = 7, Name = "Estructuras de Control" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2018-1").Id, WeekNumberStart = 9, WeekNumberEnd = 11, Name = "Memoria Dinámica" },

                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Lenguaje I" && s.Term.Name == "2017-2").Id, WeekNumberStart = 1, WeekNumberEnd = 7, Name = "Comprensión Lectora para la Producción" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Lenguaje I" && s.Term.Name == "2017-2").Id, WeekNumberStart = 9, WeekNumberEnd = 15, Name = "Producción de Textos Académicos" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2017-2").Id, WeekNumberStart = 3, WeekNumberEnd = 6, Name = "Cónicas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2017-2").Id, WeekNumberStart = 6, WeekNumberEnd = 10, Name = "Funciones" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2017-2").Id, WeekNumberStart = 11, WeekNumberEnd = 12, Name = "Funciones Exponenciales y Logarítmicas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2017-2").Id, WeekNumberStart = 13, WeekNumberEnd = 15, Name = "Funciones Trigonométricas" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Matemática I" && s.Term.Name == "2017-2").Id, WeekNumberStart = 1, WeekNumberEnd = 3, Name = "Matrices y Sistemas de Ecuaciones" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2017-2").Id, WeekNumberStart = 12, WeekNumberEnd = 15, Name = "Arreglos" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2017-2").Id, WeekNumberStart = 1, WeekNumberEnd = 3, Name = "Conceptos Básicos de Programación" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2017-2").Id, WeekNumberStart = 4, WeekNumberEnd = 7, Name = "Estructuras de Control" },
                new CourseUnit { CourseSyllabusId = courseSyllabuses.First(s => s.Course.Name == "Programación I" && s.Term.Name == "2017-2").Id, WeekNumberStart = 9, WeekNumberEnd = 11, Name = "Memoria Dinámica" }
            };

            return result.ToArray();
        }
    }
}
