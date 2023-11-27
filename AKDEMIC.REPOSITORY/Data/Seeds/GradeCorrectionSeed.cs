using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class GradeCorrectionSeed
    {
        public static GradeCorrection[] Seed(AkdemicContext context)
        {
            var grades = context.Grades.ToList();
            var users = context.Users.ToList();

            var result = new List<GradeCorrection>()
            {
                new GradeCorrection { NewGrade = 15.3M, OldGrade = grades.First().Value, State = 1, GradeId = grades.First().Id, TeacherId = users.First(u => u.UserName == "docente").Id }
            };

            return result.ToArray();
        }
    }
}
