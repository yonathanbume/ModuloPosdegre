using AKDEMIC.ENTITIES.Models.Tutoring;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Tutoring
{
    public class TutoringStudentSeed
    {
        public static TutoringStudent[] Seed(AkdemicContext context)
        {
            var students = context.Students.ToList();
            var terms = context.Terms.ToList();

            var result = new List<TutoringStudent>()
            {
                new TutoringStudent { StudentId = students.Where(x => x.User.UserName == "alumno").First().Id, TermId = terms.Where(x => x.Name == "2017-2").First().Id },
                new TutoringStudent { StudentId = students.Where(x => x.User.UserName == "cmalumno").First().Id, TermId = terms.Where(x => x.Name == "2018-1").First().Id },
                new TutoringStudent { StudentId = students.Where(x => x.User.UserName == "dcalumno").First().Id, TermId = terms.Where(x => x.Name == "2018-2").First().Id }
            };

            return result.ToArray();
        }
    }
}
