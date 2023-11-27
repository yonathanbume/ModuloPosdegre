using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Intranet;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class TutorialStudentSeed
    {
        public static TutorialStudent[] Seed(AkdemicContext context) {
            
            var students = context.Students.ToArray();

            var teacher = context.Teachers.Include(x => x.User).FirstOrDefault(x => x.User.UserName == "docente");
            var tutorials = context.Tutorials.Where(x => x.TeacherId == teacher.UserId).ToArray();

            var result = new List<TutorialStudent>()
            {
                new TutorialStudent { TutorialId = tutorials[0].Id, StudentId = students[0].Id, Absent = true },
                new TutorialStudent { TutorialId = tutorials[0].Id, StudentId = students[1].Id, Absent = false },
                new TutorialStudent { TutorialId = tutorials[0].Id, StudentId = students[2].Id, Absent = false },
                new TutorialStudent { TutorialId = tutorials[0].Id, StudentId = students[3].Id, Absent = false },
                new TutorialStudent { TutorialId = tutorials[0].Id, StudentId = students[4].Id, Absent = false },
                new TutorialStudent { TutorialId = tutorials[0].Id, StudentId = students[5].Id, Absent = false },
                new TutorialStudent { TutorialId = tutorials[0].Id, StudentId = students[6].Id, Absent = false },
                new TutorialStudent { TutorialId = tutorials[0].Id, StudentId = students[7].Id, Absent = false },
                new TutorialStudent { TutorialId = tutorials[1].Id, StudentId = students[8].Id, Absent = false },
                new TutorialStudent { TutorialId = tutorials[1].Id, StudentId = students[9].Id, Absent = false },
                new TutorialStudent { TutorialId = tutorials[1].Id, StudentId = students[10].Id, Absent = false },
                new TutorialStudent { TutorialId = tutorials[1].Id, StudentId = students[11].Id, Absent = false },
                new TutorialStudent { TutorialId = tutorials[1].Id, StudentId = students[12].Id, Absent = false },
                new TutorialStudent { TutorialId = tutorials[1].Id, StudentId = students[13].Id, Absent = true },
                new TutorialStudent { TutorialId = tutorials[1].Id, StudentId = students[14].Id, Absent = false },
                new TutorialStudent { TutorialId = tutorials[1].Id, StudentId = students[15].Id, Absent = true }
            };

            return result.ToArray();

        }
    }
}
