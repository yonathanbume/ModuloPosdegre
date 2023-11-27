using AKDEMIC.ENTITIES.Models.Tutoring;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Tutoring
{
    public class TutorSeed
    {
        public static Tutor[] Seed(AkdemicContext context)
        {
            var teachers = context.Teachers.ToList();
            var careers = context.Careers.ToList();
            var result = new List<Tutor>()
            {
                new Tutor { UserId = teachers.Where(x => x.User.UserName == "docente").First().UserId, CareerId = careers.First().Id },
                new Tutor { UserId = teachers.Where(x => x.User.UserName == "frdocente").First().UserId, CareerId = careers.First().Id },
                new Tutor { UserId = teachers.Where(x => x.User.UserName == "agdocente").First().UserId, CareerId = careers.First().Id }
            };
            return result.ToArray();
        }
    }
}
