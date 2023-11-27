using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class EvaluationSeed
    {
        public static Evaluation[] Seed(AkdemicContext context)
        {
            var courseTerms = context.CourseTerms
                .Include(x => x.Course)
                .Include(x => x.Term)
                .ToList();

            var result = new List<Evaluation>()
            {
                new Evaluation { Percentage = 8, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 10, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 12, Retrievable = true, Name = "Práctica 3", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo Parcial", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-2").Id },

                new Evaluation { Percentage = 8, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 10, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 12, Retrievable = true, Name = "Práctica 3", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo Parcial", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-2").Id },

                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Tareas Académicas", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 30, Retrievable = false, Name = "Trabajo Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2018-2").Id },

                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Evaluación de Desempeño", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 30, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-2").Id },

                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Evaluación de Desempeño", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 30, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2018-2").Id },

                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 30, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-2").Id },

                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 30, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-2").Id },

                new Evaluation { Percentage = 8, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 10, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 12, Retrievable = true, Name = "Práctica 3", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo Parcial", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-2").Id },

                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Tareas Académicas", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2018-2").Id },
                new Evaluation { Percentage = 30, Retrievable = false, Name = "Trabajo Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2018-2").Id },

                //

                new Evaluation { Percentage = 8, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 10, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 12, Retrievable = true, Name = "Práctica 3", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo Parcial", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2018-1").Id },

                new Evaluation { Percentage = 8, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 10, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 12, Retrievable = true, Name = "Práctica 3", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo Parcial", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2018-1").Id },

                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Tareas Académicas", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 30, Retrievable = false, Name = "Trabajo Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2018-1").Id },

                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Evaluación de Desempeño", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 30, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2018-1").Id },

                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Evaluación de Desempeño", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 30, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2018-1").Id },

                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 30, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2018-1").Id },

                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 30, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2018-1").Id },

                new Evaluation { Percentage = 8, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 10, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 12, Retrievable = true, Name = "Práctica 3", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo Parcial", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2018-1").Id },

                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Tareas Académicas", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2018-1").Id },
                new Evaluation { Percentage = 30, Retrievable = false, Name = "Trabajo Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2018-1").Id },

                //

                new Evaluation { Percentage = 8, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 10, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 12, Retrievable = true, Name = "Práctica 3", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo Parcial", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Desarrollo de Aplicaciones" & x.Term.Name == "2017-2").Id },

                new Evaluation { Percentage = 8, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 10, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 12, Retrievable = true, Name = "Práctica 3", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo Parcial", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Estructuras de Datos" & x.Term.Name == "2017-2").Id },

                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Tareas Académicas", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 30, Retrievable = false, Name = "Trabajo Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Ética y Ciudadanía" & x.Term.Name == "2017-2").Id },

                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Evaluación de Desempeño", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 30, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje I" & x.Term.Name == "2017-2").Id },

                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Evaluación de Desempeño", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 30, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Lenguaje II" & x.Term.Name == "2017-2").Id },

                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 30, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática I" & x.Term.Name == "2017-2").Id },

                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 30, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Matemática II" & x.Term.Name == "2017-2").Id },

                new Evaluation { Percentage = 8, Retrievable = true, Name = "Práctica 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 10, Retrievable = true, Name = "Práctica 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 12, Retrievable = true, Name = "Práctica 3", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 15, Retrievable = true, Name = "Examen Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo Parcial", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Final", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = true, Name = "Examen Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Programación I" & x.Term.Name == "2017-2").Id },

                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo 1", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 15, Retrievable = false, Name = "Trabajo 2", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Trabajo Parcial", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 20, Retrievable = false, Name = "Tareas Académicas", CourseTermId =courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2017-2").Id },
                new Evaluation { Percentage = 30, Retrievable = false, Name = "Trabajo Final", CourseTermId = courseTerms.FirstOrDefault(x => x.Course.Name == "Seminario I" & x.Term.Name == "2017-2").Id }
            };

            return result.ToArray();
        }
    }
}
