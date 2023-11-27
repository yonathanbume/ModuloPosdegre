using System;
using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class AcademicCalendarDateSeed
    {
        public static AcademicCalendarDate[] Seed(AkdemicContext context)
        {
            var term = context.Terms.ToList();

            var result = new List<AcademicCalendarDate>()
            {
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2017-2").Id, Name = "Información de Matrícula y Prioridad", StartDate = DateTime.Parse("2017-07-30").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2017-2").Id, Name = "Matrícula y elección de cursos y horarios", StartDate = DateTime.Parse("2017-08-06").ToUniversalTime(), EndDate = DateTime.Parse("2017-08-10").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2017-2").Id, Name = "Inicio de clases", StartDate = DateTime.Parse("2017-08-21").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2017-2").Id, Name = "Exámenes Parciales", StartDate = DateTime.Parse("2017-10-09").ToUniversalTime(), EndDate = DateTime.Parse("2017-10-15").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2017-2").Id, Name = "Exámenes rezagados del parcial", StartDate = DateTime.Parse("2017-10-16").ToUniversalTime(), EndDate = DateTime.Parse("2017-10-17").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2017-2").Id, Name = "Fecha límite de retiro académico de ciclo o cursos", StartDate = DateTime.Parse("2017-11-30").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2017-2").Id, Name = "Fin de clases", StartDate = DateTime.Parse("2017-12-06").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2017-2").Id, Name = "Exámenes Finales", StartDate = DateTime.Parse("2017-12-07").ToUniversalTime(), EndDate = DateTime.Parse("2017-12-13").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2017-2").Id, Name = "Exámenes rezagados del final", StartDate = DateTime.Parse("2017-12-17").ToUniversalTime(), EndDate = DateTime.Parse("2017-12-18").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2017-2").Id, Name = "Fin del ciclo lectivo", StartDate = DateTime.Parse("2017-12-20").ToUniversalTime() },

                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-1").Id, Name = "Información de Matrícula y Prioridad", StartDate = DateTime.Parse("2018-02-28").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-1").Id, Name = "Matrícula y elección de cursos y horarios", StartDate = DateTime.Parse("2018-03-06").ToUniversalTime(), EndDate = DateTime.Parse("2018-03-10").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-1").Id, Name = "Inicio de clases", StartDate = DateTime.Parse("2018-03-21").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-1").Id, Name = "Exámenes Parciales", StartDate = DateTime.Parse("2018-05-09").ToUniversalTime(), EndDate = DateTime.Parse("2018-05-15").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-1").Id, Name = "Exámenes rezagados del parcial", StartDate = DateTime.Parse("2018-05-16").ToUniversalTime(), EndDate = DateTime.Parse("2018-05-17").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-1").Id, Name = "Fecha límite de retiro académico de ciclo o cursos", StartDate = DateTime.Parse("2018-06-30").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-1").Id, Name = "Fin de clases", StartDate = DateTime.Parse("2018-07-06").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-1").Id, Name = "Exámenes Finales", StartDate = DateTime.Parse("2018-07-07").ToUniversalTime(), EndDate = DateTime.Parse("2018-07-13").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-1").Id, Name = "Exámenes rezagados del final", StartDate = DateTime.Parse("2018-07-17").ToUniversalTime(), EndDate = DateTime.Parse("2018-07-18").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-1").Id, Name = "Fin del ciclo lectivo", StartDate = DateTime.Parse("2018-07-20").ToUniversalTime() },

                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-2").Id, Name = "Información de Matrícula y Prioridad", StartDate = DateTime.Parse("2018-07-30").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-2").Id, Name = "Matrícula y elección de cursos y horarios", StartDate = DateTime.Parse("2018-08-06").ToUniversalTime(), EndDate = DateTime.Parse("2018-08-10").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-2").Id, Name = "Inicio de clases", StartDate = DateTime.Parse("2018-08-21").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-2").Id, Name = "Exámenes Parciales", StartDate = DateTime.Parse("2018-10-09").ToUniversalTime(), EndDate = DateTime.Parse("2018-10-15").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-2").Id, Name = "Exámenes rezagados del parcial", StartDate = DateTime.Parse("2018-10-16").ToUniversalTime(), EndDate = DateTime.Parse("2018-10-17").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-2").Id, Name = "Fecha límite de retiro académico de ciclo o cursos", StartDate = DateTime.Parse("2018-11-30").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-2").Id, Name = "Fin de clases", StartDate = DateTime.Parse("2018-12-06").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-2").Id, Name = "Exámenes Finales", StartDate = DateTime.Parse("2018-12-07").ToUniversalTime(), EndDate = DateTime.Parse("2018-12-13").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-2").Id, Name = "Exámenes rezagados del final", StartDate = DateTime.Parse("2018-12-17").ToUniversalTime(), EndDate = DateTime.Parse("2018-12-18").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2018-2").Id, Name = "Fin del ciclo lectivo", StartDate = DateTime.Parse("2018-12-20").ToUniversalTime() },

                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2019-1").Id, Name = "Información de Matrícula y Prioridad", StartDate = DateTime.Parse("2019-02-28").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2019-1").Id, Name = "Matrícula y elección de cursos y horarios", StartDate = DateTime.Parse("2019-03-06").ToUniversalTime(), EndDate = DateTime.Parse("2019-03-10").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2019-1").Id, Name = "Inicio de clases", StartDate = DateTime.Parse("2019-03-21").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2019-1").Id, Name = "Exámenes Parciales", StartDate = DateTime.Parse("2019-05-09").ToUniversalTime(), EndDate = DateTime.Parse("2019-05-15").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2019-1").Id, Name = "Exámenes rezagados del parcial", StartDate = DateTime.Parse("2019-05-16").ToUniversalTime(), EndDate = DateTime.Parse("2019-05-17").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2019-1").Id, Name = "Fecha límite de retiro académico de ciclo o cursos", StartDate = DateTime.Parse("2019-06-30").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2019-1").Id, Name = "Fin de clases", StartDate = DateTime.Parse("2019-07-06").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2019-1").Id, Name = "Exámenes Finales", StartDate = DateTime.Parse("2019-07-07").ToUniversalTime(), EndDate = DateTime.Parse("2019-07-13").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2019-1").Id, Name = "Exámenes rezagados del final", StartDate = DateTime.Parse("2019-07-17").ToUniversalTime(), EndDate = DateTime.Parse("2019-07-18").ToUniversalTime() },
                new AcademicCalendarDate { TermId = term.First(x => x.Name == "2019-1").Id, Name = "Fin del ciclo lectivo", StartDate = DateTime.Parse("2019-07-20").ToUniversalTime() }
            };

            return result.ToArray();
        }
    }
}
