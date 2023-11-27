using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.TeachingManagement;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class UnitResourceSeed
    {
        public static UnitResource[] Seed(AkdemicContext apiContext)
        {
            var courseUnits = apiContext.CourseUnits.ToList();

            var result = new List<UnitResource>()
            {
                new UnitResource { CourseUnitId = courseUnits.First(cu => cu.CourseSyllabus.Course.Name == "Programación I" && cu.CourseSyllabus.Term.Name == "2018-1" && cu.Name == "Conceptos Básicos de Programación").Id, Week = 1, Name = "Ivor Horton's beginning Visual C++ 2012" },
                new UnitResource { CourseUnitId = courseUnits.First(cu => cu.CourseSyllabus.Course.Name == "Programación I" && cu.CourseSyllabus.Term.Name == "2018-1" && cu.Name == "Conceptos Básicos de Programación").Id, Week = 2, Name = "C: algoritmos, programación y estructuras de datos" },
                new UnitResource { CourseUnitId = courseUnits.First(cu => cu.CourseSyllabus.Course.Name == "Programación I" && cu.CourseSyllabus.Term.Name == "2018-1" && cu.Name == "Conceptos Básicos de Programación").Id, Week = 2, Name = "El Lenguaje de Programación C" },
                new UnitResource { CourseUnitId = courseUnits.First(cu => cu.CourseSyllabus.Course.Name == "Programación I" && cu.CourseSyllabus.Term.Name == "2018-1" && cu.Name == "Estructuras de Control").Id, Week = 5, Name = "C++ programming : from problem analysis to program design" },
                new UnitResource { CourseUnitId = courseUnits.First(cu => cu.CourseSyllabus.Course.Name == "Programación I" && cu.CourseSyllabus.Term.Name == "2018-1" && cu.Name == "Memoria Dinámica").Id, Week = 10, Name = "Manual imprescindible de C/C++" },

                new UnitResource { CourseUnitId = courseUnits.First(cu => cu.CourseSyllabus.Course.Name == "Programación I" && cu.CourseSyllabus.Term.Name == "2018-2" && cu.Name == "Conceptos Básicos de Programación").Id, Week = 1, Name = "Ivor Horton's beginning Visual C++ 2012" },
                new UnitResource { CourseUnitId = courseUnits.First(cu => cu.CourseSyllabus.Course.Name == "Programación I" && cu.CourseSyllabus.Term.Name == "2018-2" && cu.Name == "Conceptos Básicos de Programación").Id, Week = 2, Name = "C: algoritmos, programación y estructuras de datos" },
                new UnitResource { CourseUnitId = courseUnits.First(cu => cu.CourseSyllabus.Course.Name == "Programación I" && cu.CourseSyllabus.Term.Name == "2018-2" && cu.Name == "Conceptos Básicos de Programación").Id, Week = 2, Name = "El Lenguaje de Programación C" },
                new UnitResource { CourseUnitId = courseUnits.First(cu => cu.CourseSyllabus.Course.Name == "Programación I" && cu.CourseSyllabus.Term.Name == "2018-2" && cu.Name == "Estructuras de Control").Id, Week = 5, Name = "C++ programming : from problem analysis to program design" },
                new UnitResource { CourseUnitId = courseUnits.First(cu => cu.CourseSyllabus.Course.Name == "Programación I" && cu.CourseSyllabus.Term.Name == "2018-2" && cu.Name == "Memoria Dinámica").Id, Week = 10, Name = "Manual imprescindible de C/C++" }
            };

            return result.ToArray();
        }
    }
}
