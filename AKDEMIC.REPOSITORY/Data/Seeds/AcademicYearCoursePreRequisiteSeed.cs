using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class AcademicYearCoursePreRequisiteSeed
    {
        public static AcademicYearCoursePreRequisite[] Seed(AkdemicContext context)
        {
            var academicYearCourses = context.AcademicYearCourses.ToList();
            var courses = context.Courses.ToList();

            var result = new List<AcademicYearCoursePreRequisite>()
            {
                //// MALLA 2018-1
                ////Matematica II - Requiere Matematica I
                //new AcademicYearCoursePreRequisite { AcademicYearCourseId = academicYearCourses[3].Id, CourseId = courses[0].Id },
                ////Lenguaje II - Lenguaje I
                //new AcademicYearCoursePreRequisite { AcademicYearCourseId = academicYearCourses[4].Id, CourseId = courses[2].Id },
                ////Estructura de Datos - Requiere Programacion I
                //new AcademicYearCoursePreRequisite { AcademicYearCourseId = academicYearCourses[5].Id, CourseId = courses[4].Id },
                ////Seminario II - Requiere Seminario I
                //new AcademicYearCoursePreRequisite { AcademicYearCourseId = academicYearCourses[8].Id, CourseId = courses[6].Id },

                //// MALLA 2017-2
                ////Matematica II - Requiere Matematica I
                //new AcademicYearCoursePreRequisite { AcademicYearCourseId = academicYearCourses[12].Id, CourseId = courses[0].Id },
                ////Lenguaje II - Lenguaje I
                //new AcademicYearCoursePreRequisite { AcademicYearCourseId = academicYearCourses[13].Id, CourseId = courses[2].Id },
                ////Estructura de Datos - Requiere Programacion I
                //new AcademicYearCoursePreRequisite { AcademicYearCourseId = academicYearCourses[14].Id, CourseId = courses[4].Id },
                ////Seminario II - Requiere Seminario I
                //new AcademicYearCoursePreRequisite { AcademicYearCourseId = academicYearCourses[17].Id, CourseId = courses[6].Id }
            };

            return result.ToArray();
        }
    }
}
