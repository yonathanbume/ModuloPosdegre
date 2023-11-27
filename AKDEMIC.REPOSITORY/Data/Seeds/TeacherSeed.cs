using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class TeacherSeed
    {
        public static Teacher[] Seed(AkdemicContext context)
        {
            var teacherDedications = context.TeacherDedication.ToList();
            var users = context.Users.ToList();
            var careers = context.Careers.ToList();
            
           
            var result = new List<Teacher>()
            {
                new Teacher { UserId = users.FirstOrDefault(x => x.UserName == "docente").Id, TeacherDedicationId = teacherDedications.First().Id, CareerId = careers[0].Id },
                new Teacher { UserId = users.FirstOrDefault(x => x.UserName == "agdocente").Id, TeacherDedicationId = teacherDedications.First().Id, CareerId = careers[0].Id },
                new Teacher { UserId = users.FirstOrDefault(x => x.UserName == "apdocente").Id, TeacherDedicationId = teacherDedications.First().Id, CareerId = careers[0].Id },
                new Teacher { UserId = users.FirstOrDefault(x => x.UserName == "cidocente").Id, TeacherDedicationId = teacherDedications.Last().Id, CareerId = careers[0].Id },
                new Teacher { UserId = users.FirstOrDefault(x => x.UserName == "frdocente").Id, TeacherDedicationId = teacherDedications.First().Id, CareerId = careers[0].Id },
                new Teacher { UserId = users.FirstOrDefault(x => x.UserName == "jedocente").Id, TeacherDedicationId = teacherDedications.First().Id, CareerId = careers[0].Id },
                new Teacher { UserId = users.FirstOrDefault(x => x.UserName == "jmdocente").Id, TeacherDedicationId = teacherDedications.First().Id, CareerId = careers[0].Id },
                new Teacher { UserId = users.FirstOrDefault(x => x.UserName == "mcdocente").Id, TeacherDedicationId = teacherDedications.Last().Id, CareerId = careers[0].Id },
                new Teacher { UserId = users.FirstOrDefault(x => x.UserName == "radocente").Id, TeacherDedicationId = teacherDedications.First().Id, CareerId = careers[0].Id },
                new Teacher { UserId = users.FirstOrDefault(x => x.UserName == "predocente").Id, TeacherDedicationId = teacherDedications.First().Id, CareerId = careers[0].Id },
                new Teacher { UserId = users.FirstOrDefault(x => x.UserName == "predocente1").Id, TeacherDedicationId = teacherDedications.First().Id, CareerId = careers[0].Id },
                new Teacher { UserId = users.FirstOrDefault(x => x.UserName == "predocente2").Id, TeacherDedicationId = teacherDedications.First().Id, CareerId = careers[0].Id }
            };

            return result.ToArray();
        }
    }
}
