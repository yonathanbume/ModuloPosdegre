using System;
using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class EnrollmentTurnSeed
    {
        public static EnrollmentTurn[] Seed(AkdemicContext context)
        {
            var students = context.Students.ToList();
            var term = context.Terms.ToList();

            var result = new List<EnrollmentTurn>()
            {
                new EnrollmentTurn { Time = DateTime.Parse("2018-04-01").ToUniversalTime(), StudentId = students[0].Id, TermId = term.First(x => x.Status == 1).Id, CreditsLimit = 18 },
                new EnrollmentTurn { Time = DateTime.Parse("2018-04-02").ToUniversalTime(), StudentId = students[1].Id, TermId = term.First(x => x.Status == 1).Id, CreditsLimit = 18 },
                new EnrollmentTurn { Time = DateTime.Parse("2018-04-02").ToUniversalTime(), StudentId = students[2].Id, TermId = term.First(x => x.Status == 1).Id, CreditsLimit = 18 },
                new EnrollmentTurn { Time = DateTime.Parse("2018-04-12").ToUniversalTime(), StudentId = students[3].Id, TermId = term.First(x => x.Status == 1).Id, CreditsLimit = 18 }
            };

            return result.ToArray();
        }
    }
}
